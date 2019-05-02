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

        private void btnEntrada_Click(object sender, EventArgs e)
        {
            string consultaBuscarUsuario = string.Format("SELECT * FROM empleados WHERE NIF LIKE '{0}';",txtNif.Text);//Consulta para comprobar si el usuario existe
            string consultaBuscarFichaje = string.Format("SELECT * FROM fichajes WHERE NIFempleado LIKE '{0}' AND fichadoSalida LIKE 0",txtNif.Text);//Consulta para comprobar si su ultimo fichaje es de entrada o salida
            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception ex)
            {
                
            }

            MessageBox.Show("l" + Usuario.ComprobarLetraNif(txtNif.Text));
            MessageBox.Show("u" + Usuario.BuscaUsuario(bdactevalu.Conexion, consultaBuscarUsuario).Count);
            MessageBox.Show("f" + Fichaje.BuscaFichajes(bdactevalu.Conexion, consultaBuscarFichaje).Count);
            if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion, consultaBuscarUsuario).Count==1 && Fichaje.BuscaFichajes(bdactevalu.Conexion,consultaBuscarFichaje).Count==0)
            {
                Fichaje fichaje = new Fichaje(txtNif.Text, DateTime.Now.Date, DateTime.Now);
                MessageBox.Show(Convert.ToString(Fichaje.FichajeEntrada(bdactevalu.Conexion, fichaje)));
            }
            else
            {
                MessageBox.Show("ERROR");
            }
            bdactevalu.CerrarConexion();
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {

            string consultaBuscarUsuario = string.Format("SELECT * FROM empleados WHERE NIF LIKE '{0}';", txtNif.Text);
            string consultaBuscarFichaje = string.Format("SELECT * FROM fichajes WHERE NIFEmpleado LIKE '{0}' AND fichadoSalida LIKE 0", txtNif.Text);

            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception ex)
            {

            }
            if (Usuario.ComprobarLetraNif(txtNif.Text) && Usuario.BuscaUsuario(bdactevalu.Conexion, consultaBuscarUsuario).Count == 1 && Fichaje.BuscaFichajes(bdactevalu.Conexion, consultaBuscarFichaje).Count == 1)
            {
                Fichaje fichaje = new Fichaje(txtNif.Text, DateTime.Now.Date, DateTime.Now);
                MessageBox.Show(Convert.ToString(Fichaje.FichajeSalida(bdactevalu.Conexion, txtNif.Text)));
            }
            else
            {
                MessageBox.Show("NIF incorrecto");
            }
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
                dtgInfo.Columns.Clear();
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
            int horas = 0;//variable para guardar las horas totales
            int minutos = 0;//variable para guardar los minutos del resultado final
            int segundos = 0;//variable para guardar los segundos del resultado final
            int dias = 0; //variable para guardar los dias del resultado final
            List<Fichaje> fichajes = Fichaje.BuscaFichajes(bdactevalu.Conexion, "SELECT * FROM fichajes;");//obtengo todos los fichajes de la base de datos
            List<Fichaje> fResultado = new List<Fichaje>();//Lista de fichajes que están dentro del intervalo
            List<int> listaDias = new List<int>();//Lista de dias de cada fichaje
            List<int> listaHoras = new List<int>();//Lista de horas de cada fichaje
            List<int> listaMinutos = new List<int>();//Lista de minutos de cada fichaje
            List<int> listaSegundos = new List<int>();//Lista de segundos de cada fichaje
            DateTime inicio;//Fecha de inicio del intervalo
            DateTime fin;//Fecha de fin del intervalo
            int[] res = new int[4];//Array para guardar el resultado del método restaHoras
            for (int i = 0; i < fichajes.Count; i++)
            {
                if (DateTime.Compare(fichajes[i].HoraEntrada,dtpFin.Value) <=0|| DateTime.Compare(fichajes[i].HoraSalida,dtpInicio.Value)>=0)//obtengo los fichajes que estén en parte dentro del intervalo de fechas
                {
                    fResultado.Add(fichajes[i]);
                }
            }

            for (int i = 0; i < fResultado.Count; i++)
            {
                //Comprobaciones necesarias para saber el intervalo de fechas a utilizar
                if (DateTime.Compare(fResultado[i].HoraEntrada, dtpInicio.Value)>=0)
                {
                    inicio = fResultado[i].HoraEntrada;
                }
                else
                {
                    inicio = dtpInicio.Value;
                }
                if (!fResultado[i].FichadoSalida)
                {
                    fin = DateTime.Now;
                }
                else if (DateTime.Compare(fResultado[i].HoraSalida, dtpFin.Value)>0)
                {
                    fin = dtpFin.Value;
                }
                else
                {
                    fin = fResultado[i].HoraSalida;
                }
               
                res = RestaHoras(inicio, fin);
                
                listaDias.Add(res[0]);
                listaHoras.Add(res[1]);
                listaMinutos.Add(res[2]);
                listaSegundos.Add(res[3]);
            }

            //Ajuste de minutos y segundos para que no pasen de 60
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
            try
            {
                bdactevalu.AbrirConexion();
            }
            catch (Exception)
            {

                throw;
            }
            string consulta = string.Format("SELECT * FROM empleados WHERE NIF LIKE '{0}' AND claveAdmin LIKE '{1}' AND administrador = 1", txtNif.Text, txtContrasenya.Text);
            if (Usuario.BuscaUsuario(bdactevalu.Conexion, consulta).Count == 1)
            {
                frmMantenimiento mantenimiento = new frmMantenimiento();
                mantenimiento.ShowDialog();
            }
            else
            {
                MessageBox.Show("Datos incorrectos");
            }
            bdactevalu.CerrarConexion();
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// Obtiene la diferencia de tiempo entre dos fechas
        /// </summary>
        /// <param name="inicio">Fecha de inicio</param>
        /// <param name="fin">Fecha final</param>
        /// <returns>Array de 4 elementos en el que el primero son los dias, el segundo las horas, el tercero los minutos y el cuarto los segundos</returns>
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
            MessageBox.Show("DiaRes " + diaRes);

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
