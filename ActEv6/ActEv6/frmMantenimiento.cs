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
    public partial class frmMantenimiento : Form
    {
        ConexionBD bdactevalu = new ConexionBD();

        public frmMantenimiento()
        {
            InitializeComponent();
        }



        private void lblNIF_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private bool ValidarDatos()
        {
            bool ok = true;
            if(txtNIF.Text=="")
            {
                ok = false;
                errorProvider1.SetError(txtNIF,"Este campo es obligatorio");
            }else{
                errorProvider1.Clear();
            }

            if (txtNombre.Text == "")
            {
                ok = false;
                errorProvider1.SetError(txtNombre, "Este campo es obligatorio");
            }
            else
            {
                errorProvider1.Clear();
            }

            if (txtApellido.Text == "")
            {
                ok = false;
                errorProvider1.SetError(txtApellido, "Este campo es obligatorio");
            }
            else
            {
                errorProvider1.Clear();
            }

            if(chkAdmin.Checked){
                if (txtClave.Text == "")
                {
                    ok = false;
                    errorProvider1.SetError(txtNIF, "Este campo es obligatorio para crear un usuario administrador");
                }
                else
                {
                    errorProvider1.Clear();
                }
            }
            return ok;
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int resultado = 0;

            try
            {
                if (bdactevalu.AbrirConexion())
                {
                    Usuario usu = new Usuario();
                    usu.Nombre = txtNombre.Text;
                    usu.Apellidos = txtApellidos.Text;
                    
                    
                    

                    if (String.IsNullOrEmpty(txtIdentidad.Text))
                    {
                        if (usu.YaEsta(bdactevalu.Conexion, usu.Nombre, usu.Apellidos))
                        {
                            MessageBox.Show("Este usuario no se puede dar de alta. Ya existe");
                        }
                        else
                        {
                            resultado = usu.AgregarUsuario(bdactevalu.Conexion, usu);
                        }
                    }
                    else
                    {
                        resultado = usu.ActualizaUsuario(bdactevalu.Conexion, usu);
                    }

                    if (resultado > 0)
                    {
                        LimpiarControles();
                    }

                    bdactevalu.CerrarConexion();
                    
                    CargaListaUsuarios();

                }
                else
                {
                    MessageBox.Show("No se ha podido abrir la conexión con la Base de Datos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally  // en cualquier caso cierro la conexión (haya error o no)
            {
                bdatos.CerrarConexion();
            }
        }

        private void bntEliminar_Click(object sender, EventArgs e)
        {
            
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }


    }
}
