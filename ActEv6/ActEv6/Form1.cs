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
        }

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

            List<Fichaje> fichajes = Fichaje.BuscaFichajes(bdactevalu.Conexion, "SELECT * FROM fichajes;");//obtengo todos los fichajes de la base de datos
            List<Fichaje> fResultado = new List<Fichaje>();
            List<int> listaDias = new List<int>();
            List<int> listaHoras = new List<int>();
            List<int> listaMinutos = new List<int>();
            List<int> listaSegundos = new List<int>();
            DateTime inicio;
            DateTime fin;
            int[] res = new int[4];
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
                if (fResultado[i].HoraSalida < dtpFin.Value && fResultado[i].HoraEntrada > dtpInicio.Value)//compruebo si todo el tiempo del fichaje está dentro del interalo
                {
                    inicio = fResultado[i].HoraEntrada;
                    fin = fResultado[i].HoraSalida;
                }
                else if (fResultado[i].HoraEntrada > dtpInicio.Value)//compruebo si el fichaje de entrada empieza dentro del intervalo
                {
                    inicio = fResultado[i].HoraEntrada;
                    if (!fResultado[i].FichadoSalida)//compruebo si no ha fichado de salida
                    {
                        fin = DateTime.Now;
                    }
                    else if (fResultado[i].HoraSalida > dtpFin.Value)//compruebo si la hora del fichaje está fuera del intervalo
                    {
                        fin = dtpFin.Value;
                    }
                    else
                    {
                        fin = fResultado[i].HoraSalida;
                    }
                }
                else//el fichaje de entrada empieza antes que el intervalo
                {
                    inicio = dtpInicio.Value;
                    if (!fResultado[i].FichadoSalida)//compruebo si no ha fichado de salida
                    {
                        fin = DateTime.Now;
                    }
                    else if (fResultado[i].HoraSalida > dtpFin.Value)//compruebo si la hora del fichaje está fuera del intervalo
                    {
                        fin = dtpFin.Value;
                    }
                    else
                    {
                        fin = fResultado[i].HoraSalida;
                    }
                }
                res = RestaHoras(inicio, fin);
                
                listaDias.Add(res[0]);
                listaHoras.Add(res[1]);
                listaMinutos.Add(res[2]);
                listaSegundos.Add(res[3]);
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

            MessageBox.Show(Convert.ToString(fResultado.Count) + " " + Convert.ToString(listaDias.Count) + " " + Convert.ToString(listaHoras.Count) + " " + Convert.ToString(listaMinutos.Count) + " " + Convert.ToString(listaSegundos.Count) + " ");
            for (int i = 0; i < fResultado.Count; i++)
            {
                dtgInfo.Rows.Add(fResultado[i].Id, fResultado[i].NifEmpleado, fResultado[i].Dia, fResultado[i].HoraEntrada, fResultado[i].HoraSalida, listaDias[i]+" d, "+listaHoras[i]+" h, "+listaMinutos[i]+" min, "+listaSegundos[i]+" s");
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

        private int[] RestaHoras(DateTime inicio, DateTime fin)
        {
            //25/04/2019 14:13:01
            //0123456789+12345678
            int dia1;
            int dia2;
            int hora1;
            int hora2;
            int minuto1;
            int minuto2;
            int segundo1;
            int segundo2;
            string fecha1;
            string fecha2;
            int[] fechaRes = new int[4];
            int horaRes;
            int minutoRes;
            int segundoRes;
            int diaRes;

            fecha1 = Convert.ToString(inicio);
            fecha2 = Convert.ToString(fin);
            hora1 = fecha1[11] * 10 + fecha1[12];
            minuto1 = fecha1[14] * 10 + fecha1[15];
            segundo1 = fecha1[17] * 10 + fecha1[18];
            hora2 = fecha2[11] * 10 + fecha2[12];
            minuto2 = fecha2[14] * 10 + fecha2[15];
            segundo2 = fecha2[17] * 10 + fecha2[18];
            dia1 = fecha1[0] * 10 + fecha1[1];
            dia2 = fecha2[0] * 10 + fecha2[1];
            segundoRes = segundo2 - segundo1;
            minutoRes = minuto2 - minuto1;
            horaRes = hora2 - hora1;
            diaRes = dia2 - dia1;
            if (diaRes>1 || diaRes<0)
            {
                diaRes = 1;
            }

            if (segundoRes < 0)
            {
                segundoRes += 60;
                minutoRes--;
            }else if (segundoRes >= 60)
            {
                segundoRes -= 60;
                minutoRes++;
            }

            if (minutoRes < 0)
            {
                minutoRes += 60;
                horaRes--;
            }
            else if (minutoRes >= 60)
            {
                minutoRes -= 60;
                horaRes++;
            }

            if (horaRes < 0)
            {
                horaRes += 24;
                diaRes--;
            }
            else if (horaRes >= 24)
            {
                horaRes -= 24;
                diaRes++;
            }
            fechaRes[0] = diaRes;
            fechaRes[1] = horaRes;
            fechaRes[2] = minutoRes;
            fechaRes[3] = segundoRes;
            return fechaRes;
        }

    }
}
