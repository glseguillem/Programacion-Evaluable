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
        /// <returns></returns>
        public static int FichajeEntrada(MySqlConnection conexion, Fichaje fichaje)
        {
           int resultado;
           string consulta;
           consulta = string.Format("INSERT INTO fichajes (NIFempleado,dia,horaEntrada,fichadoEntrada) " +
                    "VALUES ('{0}','{1}','{2}','{3}';", fichaje.nifEmpleado, fichaje.dia, fichaje.horaEntrada,true);

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            resultado = comando.ExecuteNonQuery();
            return resultado;
        }

        public static int FichajeSalida(MySqlConnection conexion, Fichaje fichaje)//falta cambiar consulta
        {
            int resultado;
            string consulta;
            consulta = string.Format("INSERT INTO fichajes (NIFempleado,dia,horaSalida,fichadoSalida) " +
                     "VALUES ('{0}','{1}','{2}','{3}';", fichaje.nifEmpleado, fichaje.dia, fichaje.horaSalida, false);

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            resultado = comando.ExecuteNonQuery();
            return resultado;
        }

        public static List<Fichaje> Permanencia(MySqlConnection conexion, DateTime diaInicio, DateTime diaFin)
        {
            List<Fichaje> fichajes = new List<Fichaje>();
            string consulta = string.Format("SELECT * FROM fichajes WHERE dia BETWEEN '{0}' and '{1}';", diaInicio, diaFin);

            MySqlCommand commando = new MySqlCommand(consulta, conexion);
            MySqlDataReader reader = commando.ExecuteReader();
            if (reader.HasRows)
            {
                Fichaje f = new Fichaje();
                while (reader.Read())
                {
                    f.id = reader.GetInt16(0);
                    f.nifEmpleado = reader.GetString(1);
                    f.dia = reader.GetDateTime(2);
                    f.horaEntrada = reader.GetDateTime(3);
                    f.horaSalida = reader.GetDateTime(4);
                    f.fichadoEntrada = reader.GetBoolean(5);
                    f.fichadoEntrada = reader.GetBoolean(6);
                }
                reader.Close();
            }
            return fichajes;
        }
    }
}
