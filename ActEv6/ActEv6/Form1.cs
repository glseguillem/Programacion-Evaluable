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
            string consulta=string.Format("SELECT * FROM usuarios WHERE NIF LIKE '{0}';",txtNif.Text);
            try
            {
                bdactevalu.AbrirConexion();
            }catch (Exception ex)
            {
                
            }
            
            
            //if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion,consulta).Count==1)
            //{
                Fichaje fichaje = new Fichaje(txtNif.Text, DateTime.Now.Date, DateTime.Now);
                MessageBox.Show(Convert.ToString(Fichaje.FichajeEntrada(bdactevalu.Conexion, fichaje)));
            //}
            //else
            //{
            //    MessageBox.Show("datos incorrectos");
            //}
            bdactevalu.CerrarConexion();
            //ToShortDateString() --> "5/16/2001"
            //ToShortTimeString() --> "3:02 AM"
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {
            string consulta = string.Format("SELECT * FROM fichajes WHERE NIF LIKE '{0}';",txtNif.Text);
            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception ex)
            {

            }

            Fichaje fichaje = new Fichaje(txtNif.Text, DateTime.Now.Date, DateTime.Now);
            MessageBox.Show(Convert.ToString(Fichaje.FichajeSalida(bdactevalu.Conexion, fichaje)));

            bdactevalu.CerrarConexion();

        }

        private void btnPresencia_Click(object sender, EventArgs e)
        {
            string consulta = string.Format("SELECT nombre,apellido,horaEntrada FROM empleados INNER JOIN fichajes ON horaEntrada IS NOT NULL;");
            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception ex)
            {

            }

            bdactevalu.CerrarConexion();


        private void btnPermanencia_Click(object sender, EventArgs e) 
        {
            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception ex)
            {
                
            }
            int horas = 0;
            int minutos = 0;
            int segundos = 0;
            string tiempo;

            List<Fichaje> fichajes = Fichaje.BuscaFichajes(bdactevalu.Conexion, "SELECT * FROM fichajes;");//obtengo todos los fichajes de la base de datos
            List<Fichaje> fResultado = new List<Fichaje>();
            List<int> listaHoras = new List<int>();
            List<int> listaMinutos = new List<int>();
            List<int> listaSegundos = new List<int>();
            for (int i = 0; i < fichajes.Count-1; i++)
            {
                if (fichajes[i].HoraEntrada<dtpFin.Value || fichajes[i].HoraSalida>dtpInicio.Value)//obtengo los fichajes que estén en parte dentro del intervalo de fechas
                {
                    fResultado.Add(fichajes[i]);
                }
            }

            for (int i = 0; i < fResultado.Count; i++)
            {
                //Comprobación del tiempo del fichaje que está dentro del intervalo
                if (fResultado[i].HoraSalida<dtpFin.Value && fResultado[i].HoraEntrada>dtpInicio.Value)//compruebo si todo el tiempo del fichaje está dentro del interalo
                {
                    tiempo = Convert.ToString(fResultado[i].HoraSalida - fResultado[i].HoraEntrada);
                }
                else if(fResultado[i].HoraEntrada>dtpInicio.Value )//compruebo si el fichaje de entrada empieza dentro del intervalo
                {
                    if (!fResultado[i].FichadoSalida)//compruebo si no ha fichado de salida
                    {
                        tiempo = Convert.ToString(DateTime.Now - fResultado[i].HoraEntrada);
                    }
                    else if(fResultado[i].HoraSalida>dtpFin.Value)//compruebo si la hora del fichaje está fuera del intervalo
                    {
                        tiempo = Convert.ToString(dtpFin.Value - fResultado[i].HoraSalida);
                    }
                    else
                    {
                        tiempo = Convert.ToString(fResultado[i].HoraSalida - fResultado[i].HoraEntrada);
                    }
                }
                else//el fichaje de entrada empieza antes que el intervalo
                {
                    if (!fResultado[i].FichadoSalida)//compruebo si no ha fichado de salida
                    {
                        tiempo = Convert.ToString(DateTime.Now - dtpInicio.Value);
                    }
                    else if (fResultado[i].HoraSalida > dtpFin.Value)//compruebo si la hora del fichaje está fuera del intervalo
                    {
                        tiempo = Convert.ToString(dtpFin.Value - dtpInicio.Value);
                    }
                    else
                    {
                        tiempo = Convert.ToString(fResultado[i].HoraSalida - dtpInicio.Value);
                    }
                }
                
                listaHoras.Add((Convert.ToInt16(tiempo[11]) * 10) + Convert.ToInt16(tiempo[12]));
                listaMinutos.Add((Convert.ToInt16(tiempo[14]) * 10) + Convert.ToInt16(tiempo[15]));
                listaHoras.Add((Convert.ToInt16(tiempo[17]) * 10) + Convert.ToInt16(tiempo[18]));
            }

            for (int i = 0; i < fResultado.Count; i++)
            {
                horas += listaHoras[i];
                minutos += listaMinutos[i];
                if (segundos>=60)
                {
                    segundos -= 60;
                    minutos++;
                }
                if (minutos>=60)
                {
                    minutos -= 60;
                    horas++;
                }
            }
            dtgInfo.Columns.Clear();
            dtgInfo.Columns.Add("id", "ID");
            dtgInfo.Columns.Add("NIFempleado", "NIF empleado");
            dtgInfo.Columns.Add("dia", "dia");
            dtgInfo.Columns.Add("horaEntrada", "hora de entrada");
            dtgInfo.Columns.Add("horaSalida", "hora de salida");
            dtgInfo.Columns.Add("TiempoFichaje", "duración");

            for (int i = 0; i < fichajes.Count; i++)
            {
                dtgInfo.Rows.Add(fichajes[i].Id, fichajes[i].NifEmpleado, fichajes[i].Dia, fichajes[i].HoraEntrada, fichajes[i].HoraSalida, listaHoras[i]+" h, "+listaMinutos[i]+" min, "+listaSegundos[i]+" s");
            }
            bdactevalu.CerrarConexion();
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
