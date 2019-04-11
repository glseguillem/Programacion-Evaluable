using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace ActEv6
{
    class ConexionBD
    {
        private MySqlConnection conexion;

        public MySqlConnection Conexion { get { return conexion; } }

        public ConexionBD()
        {
            string server = "server=127.0.0.1;";
            string port = "port=3306;";
            string database = "database=bdactevalu;";
            string usuario = "uid=root;";
            string password = "pwd=;";
            string convert = "Convert Zero Datetime=True;";
            string connectionstring = server + port + database + usuario + password + convert;

            conexion = new MySqlConnection(connectionstring);
        }

        public bool AbrirConexion()
        {
            try
            {
                conexion.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        public bool CerrarConexion()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }
    }
}
