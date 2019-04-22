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
            string consulta=string.Format("SELECT * FROM fichajes WHERE NIF LIKE '{0}';",txtNif.Text);
            if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion,consulta).Count==1)
            {
                Fichaje fichaje = new Fichaje(txtNif.Text, Convert.ToDateTime(DateTime.Today.ToShortDateString()), Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                Fichaje.FichajeEntrada(bdactevalu.Conexion, fichaje);
            }
            //ToShortDateString() --> "5/16/2001"
            //ToShortTimeString() --> "3:02 AM"
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {
            string consulta = string.Format("SELECT * FROM fichajes WHERE NIF LIKE '{0}';",txtNif.Text);
            if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion, consulta).Count == 1)
            {
                Fichaje fichaje = new Fichaje();
                fichaje.HoraSalida = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                fichaje.FichadoEntrada = true;
                Fichaje.FichajeSalida(bdactevalu.Conexion, fichaje);
            }
        }

        private void btnPresencia_Click(object sender, EventArgs e)
        {
            string consulta = string.Format("SELECT * FROM fichajes WHERE horaEntrada IS NOT NULL and horaSalida IS NULL;");
                
        }

        private void btnPermanencia_Click(object sender, EventArgs e) 
        {
            int horas=0;
            int minutos=0; 
            string tiempo;

            List<Fichaje> fichajes = Fichaje.Permanencia(bdactevalu.Conexion,Convert.ToDateTime(dtpInicio.Value.ToShortDateString()), Convert.ToDateTime(dtpFin.Value.ToShortDateString()));
            List<int> listaHoras = new List<int>();
            List<int> listaMinutos = new List<int>();
            for (int i = 0; i < fichajes.Count; i++)
            {
                if (fichajes[i].FichadoSalida)
                {
                    tiempo = Convert.ToString(fichajes[i].HoraSalida - fichajes[i].HoraEntrada);
                }
                else
                {
                    tiempo = Convert.ToString(Convert.ToDateTime(DateTime.Today.ToShortTimeString()) - fichajes[i].HoraEntrada);
                }

                if (tiempo[1] == ':')
                {
                    listaHoras.Add(tiempo[0]);
                    listaMinutos.Add(tiempo[2] * 10 + tiempo[3]);
                    if (tiempo[6] == 'P')
                    {
                        listaHoras[i] += 12;
                    }
                }
                else
                {
                    listaHoras.Add(tiempo[0] * 10 + tiempo[1]);
                    listaMinutos.Add(tiempo[3] * 10 + tiempo[4]);
                    if (tiempo[7] == 'P')
                    {
                        listaHoras[i] += 12;
                    }
                }
            }

            for (int i = 0; i < fichajes.Count; i++)
            {
                horas += listaHoras[i];
                minutos += listaMinutos[i];
                if (minutos>=60)
                {
                    minutos -= 60;
                    horas++;
                }
            }

            dtgInfo.Columns.Add("id", "ID");
            dtgInfo.Columns.Add("NIFempleado", "NIF empleado");
            dtgInfo.Columns.Add("dia", "dia");
            dtgInfo.Columns.Add("horaEntrada", "hora de entrada");
            dtgInfo.Columns.Add("horaSalida", "hora de salida");
            dtgInfo.Columns.Add("TiempoFichaje", "duración");

            for (int i = 0; i < fichajes.Count; i++)
            {
                dtgInfo.Rows.Add(fichajes[i].Id, fichajes[i].NifEmpleado, fichajes[i].Dia, fichajes[i].HoraEntrada, fichajes[i].HoraSalida, listaHoras+" h, "+listaMinutos+" min");
            }
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
