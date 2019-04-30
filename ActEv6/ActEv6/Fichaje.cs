using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ActEv6
{
    class Fichaje
    {

        //	id	NIFempleado	dia	horaEntrada	horaSalida	fichadoEntrada	fichadoSalida
        private int id;
        private string nifEmpleado;
        private DateTime dia;
        private DateTime horaEntrada;
        private DateTime horaSalida;
        private bool fichadoEntrada = false;
        private bool fichadoSalida = false;

        public int Id { set { id = value; } get { return id; } }
        public string NifEmpleado { set { nifEmpleado = value; } get { return nifEmpleado; } }
        public DateTime Dia { set { dia = value; } get { return dia; } }
        public DateTime HoraEntrada { set { horaEntrada = value; } get { return horaEntrada; } }
        public DateTime HoraSalida { set { horaSalida = value; } get { return horaSalida; } }
        public bool FichadoEntrada { set { fichadoEntrada = value; } get { return fichadoEntrada; } }
        public bool FichadoSalida { set { fichadoSalida = value; } get { return fichadoSalida; } }

        /// <summary>
        /// Rellena los atributos necesarios para un fichaje de entrada
        /// </summary>
        /// <param name="nifEmpleado">Nif del empleado</param>
        /// <param name="dia">Dia del fichaje</param>
        /// <param name="horaEntrada">Hora en la que se realiza el fichaje</param>
        public Fichaje(string nifEmpleado, DateTime dia, DateTime horaEntrada)
        {
            this.nifEmpleado = nifEmpleado;
            this.dia = dia;
            this.horaEntrada = horaEntrada;
            this.horaSalida = DateTime.MinValue;
            fichadoEntrada = true;
        }

        /// <summary>
        /// Constructor que no recibe parámetros, se podrán introducir datos a sus atributos mediante las propiedades
        /// </summary>
        public Fichaje()
        {
        }
        
        /// <summary>
        /// Inserta un nuevo fichaje en la base de datos con los datos de un fichaje de entrada
        /// </summary>
        /// <param name="conexion">Conexión con la base de datos</param>
        /// <param name="fichaje">Fichaje a insertar</param>
        /// <returns>Número de registros afectados</returns>
        public static int FichajeEntrada(MySqlConnection conexion, Fichaje fichaje)
        {
           int resultado;
           string consulta;
           consulta = string.Format("INSERT INTO fichajes (NIFempleado,dia,horaEntrada,horaSalida,fichadoEntrada,fichadoSalida)" +
                    "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", fichaje.nifEmpleado, fichaje.dia.ToString("yyyy/MM/dd"), fichaje.horaEntrada.ToString(),fichaje.horaSalida.ToString(),1,0);

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            resultado = comando.ExecuteNonQuery();
            return resultado;
        }
        
        /// <summary>
        /// Modifica el fichaje de entrada correspondiente para dar los valores de salida
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos</param>
        /// <param name="nif">Nif del empleado</param>
        /// <returns>Número de registros afectados</returns>
        public static int FichajeSalida(MySqlConnection conexion, string nif)//falta cambiar consulta
        {
            int resultado;
            string consulta;
            // UPDATE nombretabla SET nombrecampo = valorcampo WHERE condiciones;
            // UPDATE fichajes SET fichadoEntrada=true(0)  WHERE fichadoSalida=false(0)
            //UPDATE fichajes SET fichadoSalida= false(1) Where fichadoSalida = true(0);
            //"UPDATE fichajes SET fichadoSalida = 1 AND SET horaSalida = '{0}' WHERE NIFempleado LIKE '{1}' AND fichadoSalida LIKE 0);", DateTime.Now, nif
            consulta = string.Format("UPDATE fichajes SET fichadoEntrada = 1 WHERE fichadoSalida = 0;"); 

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            resultado = comando.ExecuteNonQuery();
            return resultado;
        }

        /// <summary>
        /// Devuelve el resultado de una consulta (SELECT) en una lista de fichajes
        /// </summary>
        /// <param name="Conexion">Conexión a la base de datos</param>
        /// <param name="consulta">Consulta</param>
        /// <returns>Lista de fichajes</returns>
        public static List<Fichaje> BuscaFichajes(MySqlConnection Conexion, string consulta)
        {
            List<Fichaje> lista = new List<Fichaje>();

            MySqlCommand comando = new MySqlCommand(consulta, Conexion);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Fichaje fichaje = new Fichaje();
                    fichaje.Id = reader.GetInt16(0);
                    fichaje.NifEmpleado = reader.GetString(1);
                    fichaje.Dia = reader.GetDateTime(2);
                    fichaje.HoraEntrada = Convert.ToDateTime(reader.GetString(3));
                    fichaje.HoraSalida = Convert.ToDateTime(reader.GetString(4));
                    fichaje.fichadoEntrada = reader.GetBoolean(5);
                    fichaje.fichadoSalida=reader.GetBoolean(6);

                    lista.Add(fichaje);
                }
            }
            reader.Close();
            return lista;
        }

        /// <summary>
        /// Elimina un fichaje
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos</param>
        /// <param name="nif">Nif del empleado a eliminar</param>
        /// <returns>Número de registros afectados</returns>
        public static int EliminarFichaje(MySqlConnection conexion, string nif)
        {
            int res;
            string consulta = string.Format("DELETE FROM fichajes WHERE NIFempleado LIKE ('{0}');", nif);
            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            res = comando.ExecuteNonQuery();
            return res;
        }
    }
}
