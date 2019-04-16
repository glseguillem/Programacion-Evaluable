using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActEv6
{
    class Fichaje
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

        public Fichaje(string nif, string nombre, string apellidos, bool administrador)
        {
            this.nif = nif;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.administrador = administrador;
        }

        public Fichaje()
        {
        }
    }
}
