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

        public static List<Usuario> BuscaUsuario (MySqlConnection conexion, string consulta)
        {
            List<Usuario> lista = new List<Usuario>();

            MySqlCommand comando = new MySqlCommand(consulta, conexion);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Usuario user = new Usuario(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                        reader.GetBoolean(3));
                    lista.Add(user);
                }
            }
            return lista;
        }

        public static void AñadirUsuario(MySqlConnection conexion, Usuario usu)
        {
            string consulta;
            if (usu.administrador)
            {
                consulta = string.Format("INSERT INTO empleados NIF,nombre,apellido,administrador,claveAdmin " +
                    "VALUES ('{0}','{1}','{2}','{3}','{4}');", usu.Nif, usu.Nombre, usu.Apellidos, usu.Administrador, usu.ContrasenyaAdministrador);
            }
            else
            {
                consulta = string.Format("INSERT INTO empleados NIF,nombre,apellido,administrador " +
                    "VALUES ('{0}','{1}','{2}','{3}');", usu.Nif, usu.Nombre, usu.Apellidos, usu.Administrador);
            }
            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            comando.ExecuteNonQuery();
        }

        public static int EliminaUsuario(MySqlConnection conexion, int persona)
        {
            int retorno;
            string consulta = string.Format("DELETE FROM usuarios WHERE id={0}", persona);
            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            retorno = comando.ExecuteNonQuery();
            return retorno;
        }
    }
}

