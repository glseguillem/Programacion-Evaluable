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
    public partial class Form1 : Form
    {
        public Form1()
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

        private bool ComprobarLetraNif(string nif)
        {
            if (nif.Length==9)
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
                    if (letras[numerosNif % 23]==nif[8])
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("La letra del NIF es incorrecta");
                    }
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("NIF incorrecto");
                    
                    throw;
                }
            }
            else
            {
                MessageBox.Show("El NIF debe tener 9 carácteres");
            }
            return false;

            
        }
    }
}
