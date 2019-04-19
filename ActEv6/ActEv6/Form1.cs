using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActEv6
{
    public partial class s : Form
    {
        ConexionBD bdactevalu = new ConexionBD();

        public s()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToLongTimeString();
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToLongTimeString();
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void btnEntrada_Click(object sender, EventArgs e)//Falta probarlo, creo que dará error porque aqui utilizo string y en la base de datos date
        {
            string consulta=string.Format("SELECT * FROM fichajes WHERE NIF LIKE '{0}'",txtNif.Text);
            if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion,consulta).Count==1)
            {
                Fichaje fichaje = new Fichaje(txtNif.Text, DateTime.Today.ToString("d"), DateTime.Now.ToShortTimeString());
            }

        }

        private void btnSalida_Click(object sender, EventArgs e)//Lo hace gloria 
        {
            if (Usuario.ComprobarLetraNif(txtNif.Text))
            {

            }
        }

        private void btnPresencia_Click(object sender, EventArgs e)//esto tambien :)
        {
           
        }

        private void btnPermanencia_Click(object sender, EventArgs e)
        {
            
        }

        private void btnMantenimiento_Click(object sender, EventArgs e)//Falta comprobar errores
        {
            frmMantenimiento mantenimiento = new frmMantenimiento();
            mantenimiento.ShowDialog();
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
