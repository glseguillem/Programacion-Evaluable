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
    public partial class frmBasico : Form
    {
        ConexionBD bdactevalu = new ConexionBD();

        public frmBasico()
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

        private void btnEntrada_Click(object sender, EventArgs e)
        {
            //if (ComprobarLetraNif(txtNif.Text)){

            //}
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {
            //if (ComprobarLetraNif(txtNif.Text)){

            //}
        }

        private void btnPresencia_Click(object sender, EventArgs e)
        {
            CargarListaUsuarios();
        }

        private void btnPermanencia_Click(object sender, EventArgs e)
        {

        }

        private void btnMantenimiento_Click(object sender, EventArgs e)
        {
            //if (ComprobarLetraNif(txtNif.Text))
            //{

            //}
        }

        private void CargarListaUsuarios()
        {
            string seleccion = "Select * from usuarios";
            if (bdactevalu.AbrirConexion())
            {
                txtInformacion.Text = Usuario.BuscarUsuario(bdactevalu.Conexion, seleccion);
                bdactevalu.CerrarConexion();
            }
            else
            {
                MessageBox.Show("No se ha podido abrir la conexión con la Base de Datos");
            }
        }

    }
}
