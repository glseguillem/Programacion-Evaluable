using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


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
            string consultaBuscarUsuario = string.Format("SELECT * FROM usuarios WHERE NIF LIKE '{0}';",txtNif.Text);
            string consultaBuscarFichaje = string.Format("SELECT * FROM fichajes WHERE NIFEmpleado LIKE '{0}' AND fichadoSalida LIKE 0",txtNif.Text);
            try
            {
                bdactevalu.AbrirConexion();
            }catch (Exception ex)
            {
                
            }


            //if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion,consulta).Count==1 && Fichaje.BuscaFichajes(bdactevalu.conexion,consultaBuscarFichaje).Count==0)
            //{
                Fichaje fichaje = new Fichaje(txtNif.Text, DateTime.Now.Date, DateTime.Now);
                MessageBox.Show(Convert.ToString(Fichaje.FichajeEntrada(bdactevalu.Conexion, fichaje)));
            //}
            //else
            //{
            //    MessageBox.Show("ERROR");
            //}
            bdactevalu.CerrarConexion();
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {

            string consultaBuscarUsuario = string.Format("SELECT * FROM usuarios WHERE NIF LIKE '{0}';", txtNif.Text);
            string consultaBuscarFichaje = string.Format("SELECT * FROM fichajes WHERE NIFEmpleado LIKE '{0}' AND fichadoSalida LIKE 0", txtNif.Text);

            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception ex)
            {

            }
            //if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion,consulta).Count==1 && Fichaje.BuscaFichajes(bdactevalu.conexion,consultaBuscarFichaje).Count==1)
            //{
                Fichaje fichaje = new Fichaje(txtNif.Text, DateTime.Now.Date, DateTime.Now);
                MessageBox.Show(Convert.ToString(Fichaje.FichajeSalida(bdactevalu.Conexion, txtNif.Text)));
            //}
            //else
            //{
            //    MessageBox.Show("NIF incorrecto");
            //}
            bdactevalu.CerrarConexion();
        }

        private void btnPresencia_Click(object sender, EventArgs e)
        {
            string consulta = string.Format("SELECT nombre,apellido,horaEntrada FROM empleados INNER JOIN fichajes ON horaEntrada IS NOT NULL and horaSalida like '{0}';",DateTime.MinValue.ToString());
            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception ex)
            {

            }

            MySqlCommand comando = new MySqlCommand(consulta, bdactevalu.Conexion);
            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                //limpiar el datagreed
                dtgInfo.Columns.Clear();
                //añadir las columnas
                dtgInfo.Columns.Add("nombre", "Nombre");
                dtgInfo.Columns.Add("apellido", "Apellido");
                dtgInfo.Columns.Add("horaEntrada", "Hora de entrada");

                while (reader.Read())
                {
                    dtgInfo.Rows.Add(reader.GetString(0),reader.GetString(1),reader.GetString(2));
                }
            }
            else
            {
                MessageBox.Show("No hay empleados en este momento");
            }
            reader.Close();
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
            int dias = 0;
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
                
                //if (DateTime.Compare(fResultado[i].HoraSalida, dtpFin.Value) <= 0 && DateTime.Compare(fResultado[i].HoraEntrada, dtpInicio.Value)>=0)//compruebo si todo el tiempo del fichaje está dentro del interalo
                //{
                //    inicio = fResultado[i].HoraEntrada;
                //    fin = fResultado[i].HoraSalida;
                //}
                if (DateTime.Compare(fResultado[i].HoraEntrada, dtpInicio.Value)>=0)
                {
                    inicio = fResultado[i].HoraEntrada;
                }
                else
                {
                    inicio = dtpInicio.Value;
                }
                if (!fResultado[i].FichadoSalida)//compruebo si no ha fichado de salida
                {
                    fin = DateTime.Now;
                }
                else if (DateTime.Compare(fResultado[i].HoraSalida, dtpFin.Value)>0)//compruebo si la hora del fichaje está fuera del intervalo
                {
                    fin = dtpFin.Value;
                }
                else
                {
                    fin = fResultado[i].HoraSalida;
                }
                if (!fResultado[i].FichadoSalida)//compruebo si no ha fichado de salida
               
                res = RestaHoras(inicio, fin);
                
                listaDias.Add(res[0]);
                listaHoras.Add(res[1]);
                listaMinutos.Add(res[2]);
                listaSegundos.Add(res[3]);
            }

            for (int i = 0; i < fResultado.Count; i++)
            {
                dias += listaDias[i];
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
                while (dias > 0)
                {
                    dias--;
                    horas += 24;
                }
            }
            dtgInfo.Columns.Clear();
            dtgInfo.Columns.Add("id", "ID");
            dtgInfo.Columns.Add("NIFempleado", "NIF empleado");
            dtgInfo.Columns.Add("dia", "dia");
            dtgInfo.Columns.Add("horaEntrada", "Hora de entrada");
            dtgInfo.Columns.Add("horaSalida", "Hora de salida");
            dtgInfo.Columns.Add("TiempoFichaje", "Duración");

            MessageBox.Show(Convert.ToString(fResultado.Count) + " " + Convert.ToString(listaDias.Count) + " " + Convert.ToString(listaHoras.Count) + " " + Convert.ToString(listaMinutos.Count) + " " + Convert.ToString(listaSegundos.Count) + " ");
            for (int i = 0; i < fResultado.Count; i++)
            {
                dtgInfo.Rows.Add(fResultado[i].Id, fResultado[i].NifEmpleado, fResultado[i].Dia, fResultado[i].HoraEntrada, fResultado[i].HoraSalida, listaDias[i]+" d, "+listaHoras[i]+" h, "+listaMinutos[i]+" min, "+listaSegundos[i]+" s");
            }
            MessageBox.Show("El tiempo total de fichajes es: " + horas + "h " + minutos + "min " + segundos + "s");
            bdactevalu.CerrarConexion();
        }

        private void btnMantenimiento_Click(object sender, EventArgs e)
        {
            //if (Usuario.BuscaUsuario(bdactevalu.Conexion,"SELECT * FROM usuarios WHERE NIF LIKE "+txtNif.Text+" AND claveAdmin LIKE "+txtContrasenya.Text+" AND administrador LIKE 0").Count==1)
            //{
                frmMantenimiento mantenimiento = new frmMantenimiento();
                mantenimiento.ShowDialog();
            //}
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private int[] RestaHoras(DateTime inicio, DateTime fin)
        {
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

            hora1 = Convert.ToInt16(Convert.ToString(fecha1[fecha1.Length - 7]));//No puedo pasar directamente de char a int, da valores extraños
            if (fecha1.Length==19)
            {
                hora1 += Convert.ToInt16(Convert.ToString(fecha1[fecha1.Length - 8])) * 10;
            }
            minuto1 = Convert.ToInt16(Convert.ToString(fecha1[fecha1.Length - 5])) * 10 + Convert.ToInt16(Convert.ToString(fecha1[fecha1.Length - 4]));
            segundo1 = Convert.ToInt16(Convert.ToString(fecha1[fecha1.Length - 2])) * 10 + Convert.ToInt16(Convert.ToString(fecha1[fecha1.Length-1]));
            hora2 = Convert.ToInt16(Convert.ToString(fecha2[fecha2.Length - 7]));
            if (fecha2.Length == 19)
            {
                hora2 += Convert.ToInt16(Convert.ToString(fecha2[fecha2.Length - 8])) * 10;
                MessageBox.Show("h2 " + hora2);
            }
            minuto2 = Convert.ToInt16(Convert.ToString(fecha2[fecha2.Length - 5])) * 10 + Convert.ToInt16(Convert.ToString(fecha2[fecha2.Length - 4]));
            segundo2 = Convert.ToInt16(Convert.ToString(fecha2[fecha2.Length - 2])) * 10 + Convert.ToInt16(Convert.ToString(fecha2[fecha2.Length - 1]));
            dia1 = Convert.ToInt16(Convert.ToString(fecha1[0])) * 10 + Convert.ToInt16(Convert.ToString(fecha1[1]));
            dia2 = Convert.ToInt16(Convert.ToString(fecha2[0])) * 10 + Convert.ToInt16(Convert.ToString(fecha2[1]));
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
