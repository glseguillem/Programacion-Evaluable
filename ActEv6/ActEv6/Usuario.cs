using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ActEv6
{
    class Usuario
    {
        private string nif;
        private string nombre;
        private string apellidos;
        private bool administrador;
        private string contrasenyaAdministrador;

        public string Nif { set { nif = value; } get { return nif; } }
        public string Nombre { set { nombre = value; } get { return nombre; } }
        public string Apellidos { set { apellidos = value; } get { return apellidos; } }
        public bool Administrador { set { administrador = value; } get { return administrador; } }
        public string ContrasenyaAdministrador { set { contrasenyaAdministrador = value; } get { return contrasenyaAdministrador; } }

        public Usuario(string nif, string nombre, string apellidos, bool administrador)
        {
            this.nif = nif;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.administrador = administrador;
        }

        public Usuario(string nif, string nombre, string apellidos, bool administrador, string contrasenyaAdministrador)
        {
            this.nif = nif;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.administrador = administrador;
            this.contrasenyaAdministrador = contrasenyaAdministrador;
        }

        public Usuario()
        {
        }
        //List <Usuario> usu = new List <Usuario>();





        private static bool ComprobarLetraNif(string nif)
        {
            if (nif.Length == 9)
            {
                string nifAux = "";
                int numerosNif;

                string letras = "TRWAGMYFPDXBNJZSQVHLCKE";

                for (int i = 0; i < 7; i++)
                {
                    nifAux += nif[i];
                }

                try
                {
                    numerosNif = Convert.ToInt16(nifAux);
                    if (letras[numerosNif % 23] == nif[8])
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    

                    throw;
                }
            }
            
            return false;


        }

        public static List<Usuario> BuscarUsuario(MySqlConnection conexion, string consulta)
        {
            List<Usuario> lista = new List<Usuario>();
            // MessageBox.Show(consulta);   -Se puede activar esta línea para testear la sintaxis de la consulta.

            // Creamos el objeto command al cual le pasamos la consulta y la conexión
            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            // Ejecutamos el comando y recibimos en un objeto DataReader la lista de registros seleccionados.
            // Recordemos que un objeto DataReader es una especie de tabla de datos virtual.
            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)   // En caso que se hayan registros en el objeto reader
            {
                // Recorremos el reader (registro por registro) y cargamos la lista de usuarios.
                while (reader.Read())
                {
                    Usuario user = new Usuario(reader.GetInt16(0), reader.GetString(1), reader.GetString(2),
                        reader.GetString(3), reader.GetInt16(4), Convert.ToDateTime(reader.GetDateTime(5)), reader.GetDecimal(6));
                    lista.Add(user);
                }
            }
            // devolvemos la lista cargada con los usuarios.
            return lista;
        }
    }
}

