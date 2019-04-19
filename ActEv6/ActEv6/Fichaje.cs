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
        private string nifEmpleado;
        private string dia;
        private string horaEntrada;
        private string horaSalida;
        private bool fichadoEntrada = false;
        private bool fichadoSalida = false;

        public string NifEmpleado { set { nifEmpleado = value; } get { return nifEmpleado; } }
        public string Dia { set { dia = value; } get { return dia; } }
        public string HoraEntrada { set { horaEntrada = value; } get { return horaEntrada; } }
        public string HoraSalida { set { horaSalida = value; } get { return horaSalida; } }
        public bool FichadoEntrada { set { fichadoEntrada = value; } get { return fichadoEntrada; } }
        public bool FichadoSalida { set { fichadoSalida = value; } get { return fichadoSalida; } }

        /// <summary>
        /// Rellena los atributos necesarios para un fichaje de entrada
        /// </summary>
        /// <param name="nifEmpleado">Nif del empleado</param>
        /// <param name="dia">Dia del fichaje</param>
        /// <param name="horaEntrada">Hora en la que se realiza el fichaje</param>

        public Fichaje(string nifEmpleado, string dia, string horaEntrada)
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

        public static List<Fichaje> Permanencia(MySqlConnection conexion)
        {
            string consulta=string.Format()
        }
    }
}
