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

        /// <summary>
        /// Rellena los datos necesarios para aun usuario normal administrador
        /// </summary>
        /// <param name="nif">Nif del usuario</param>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="apellidos">Apellidos del usuario</param>
        /// <param name="administrador">Indicar si es administrador o no</param>
        public Usuario(string nif, string nombre, string apellidos, bool administrador)
        {
            this.nif = nif;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.administrador = administrador;
        }

        /// <summary>
        /// Rellena los datos necesarios para aun usuario administrador
        /// </summary>
        /// <param name="nif">Nif del usuario</param>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="apellidos">Apellidos del usuario</param>
        /// <param name="administrador">Indicar si es administrador o no</param>
        /// <param name="contrasenyaAdministrador">Contraseña de administrador del usuario</param>
        public Usuario(string nif, string nombre, string apellidos, bool administrador, string contrasenyaAdministrador)
        {
            this.nif = nif;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.administrador = administrador;
            this.contrasenyaAdministrador = contrasenyaAdministrador;
        }

        /// <summary>
        /// Constructor vacio
        /// </summary>
        public Usuario()
        {
        }


        /// <summary>
        /// Comprueba si la letra del nif es correcta
        /// </summary>
        /// <param name="nif">Nif a comprobar</param>
        /// <returns>true si es correcta, false en el caso contrario</returns>
        public static bool ComprobarLetraNif(string nif)
        {
            if (nif.Length == 9)
            {
                string nifAux = "";
                int numerosNif;

                string letras = "TRWAGMYFPDXBNJZSQVHLCKE";

                for (int i = 0; i < 8; i++)
                {
                    nifAux += nif[i];
                }

                try//Si numerosNif no se puede convertir a int daría error
                {
                    numerosNif = Convert.ToInt32(nifAux);
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

        /// <summary>
        /// Busca usuarios en la base de datos según la consulta que reciba
        /// </summary>
        /// <param name="Conexion">Conexión a la base de datos</param>
        /// <param name="consulta">Consulta abstract lanzar en la base de datos(Select)</param>
        /// <returns>Lista de usuarios como resultado de la consulta</returns>
        public static List<Usuario> BuscaUsuario (MySqlConnection Conexion, string consulta)
        {
            List<Usuario> lista = new List<Usuario>();

            MySqlCommand comando = new MySqlCommand(consulta, Conexion);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Usuario user = new Usuario(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                        reader.GetBoolean(3));
                    if (user.Administrador)
                    {
                        user.ContrasenyaAdministrador=reader.GetString(4);
                    }
                    lista.Add(user);
                }
            }
            reader.Close();
            return lista;
        }

        /// <summary>
        /// Inserta un usuario en la base de datos
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos</param>
        /// <param name="usu">usuario a insertar</param>
        /// <returns>Número de registros afectados</returns>
        public static int AñadirUsuario(MySqlConnection conexion, Usuario usu)
        {
            int resultado;
            string consulta;
            if (usu.administrador)
            {
                consulta = string.Format("INSERT INTO empleados (NIF,nombre,apellido,administrador,claveAdmin) " +
                    "VALUES ('{0}','{1}','{2}','{3}','{4}');", usu.Nif, usu.Nombre, usu.Apellidos, 1, usu.ContrasenyaAdministrador);
            }
            else
            {
                consulta = string.Format("INSERT INTO empleados (NIF,nombre,apellido,administrador) " +
                    "VALUES ('{0}','{1}','{2}','{3}');", usu.Nif, usu.Nombre, usu.Apellidos, 0);
            }
            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            resultado=comando.ExecuteNonQuery();
            return resultado;
        }

        /// <summary>
        /// Elimina un usuario de la base de datos
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos</param>
        /// <param name="persona">Nif del empleado a eliminar</param>
        /// <returns></returns>
        public static int EliminaUsuario(MySqlConnection conexion, string persona)
        {
            int retorno;
            string consulta = string.Format("DELETE FROM empleados WHERE NIF LIKE ('{0}');", persona);
            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            retorno = comando.ExecuteNonQuery();
            return retorno;
        }
    }
}

