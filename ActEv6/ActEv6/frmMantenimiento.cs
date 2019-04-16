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
            ListaUsuarios();
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

        private void btnAgregar_Click(object sender, EventArgs e)//falta comprobar errores
        {
            try
            {
                if (bdactevalu.AbrirConexion())
                {
                    string consulta = string.Format("SELECT * FROM empleados WHERE nombre LIKE ('{0}') and apellido LIKE ('{1}');"
                        , txtNombre.Text, txtApellido.Text);
                    if (Usuario.BuscaUsuario(bdactevalu.Conexion, consulta).Count == 0)
                    {
                        Usuario usu = new Usuario(txtNIF.Text,txtNombre.Text,txtApellido.Text,chkAdmin.Checked,txtClave.Text);
                        Usuario.AñadirUsuario(bdactevalu.Conexion, usu);
                        ListaUsuarios();
                    }
                    else
                    {
                        MessageBox.Show("Este usuario no se puede dar de alta. Ya existe");
                    }
                    bdactevalu.CerrarConexion();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bntEliminar_Click(object sender, EventArgs e)//No funciona, preguntar a salva
        {
            try
            {
                int resultado;

                if (dtgUsuarios.SelectedRows.Count == 1)
                {
                    string nif = (string)dtgUsuarios.CurrentRow.Cells[0].Value;

                    DialogResult eliminacion = MessageBox.Show("¿Estas seguro que quieres borrar?",
                                                "Eliminación", MessageBoxButtons.YesNo);

                    if (eliminacion == DialogResult.Yes)
                    {
                        if (bdactevalu.AbrirConexion())
                        {
                            resultado = Usuario.EliminaUsuario(bdactevalu.Conexion,nif);
                        }
                        else
                        {
                            MessageBox.Show("No se puede abrir la Base de Datos");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                bdactevalu.CerrarConexion();
            }
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {
            ListaUsuarios();
            ListaFichaje();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void ListaUsuarios()
        {
            string consulta = "Select * from empleados;";
            List<Usuario> usuarios;
            if (bdactevalu.AbrirConexion())
            {
                usuarios = Usuario.BuscaUsuario(bdactevalu.Conexion, consulta);
                dtgUsuarios.DataSource = usuarios;
            }
            else
            {
                MessageBox.Show("No se puede abrir la Base de Datos");
            }
            bdactevalu.CerrarConexion();
        }

        private void ListaFichaje()
        {
            string consulta = "Select * from fichajes;";
            List<Usuario> usuarios;
            if (bdactevalu.AbrirConexion())
            {
                usuarios = Usuario.BuscaUsuario(bdactevalu.Conexion, consulta);
                dtgFichajes.DataSource = usuarios;
            }
            else
            {
                MessageBox.Show("No se puede abrir la Base de Datos");
            }
            bdactevalu.CerrarConexion();
        }

        private void MostrarTexto (Usuario usu)
        {
            dtgUsuarios.Rows.Add(usu.Nif, usu.Nombre, usu.Apellidos, usu.Administrador,usu.ContrasenyaAdministrador);
            dtgFichajes.Rows.Add(usu.Nif);

        }

    }
}
