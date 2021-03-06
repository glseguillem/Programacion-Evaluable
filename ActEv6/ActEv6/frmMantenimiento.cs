﻿using System;
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
            ListaFichaje();
            txtClave.Enabled = chkAdmin.Checked;
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

            return ok;
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)//falta comprobar errores
        {
            try
            {
                if (bdactevalu.AbrirConexion() && ValidarDatos())
                {
                    bool admi;
                    string consulta = string.Format("SELECT * FROM empleados WHERE NIF LIKE ('{0}');"
                        , txtNIF.Text);
                  
                    if (Usuario.BuscaUsuario(bdactevalu.Conexion, consulta).Count==0 && Usuario.ComprobarLetraNif(txtNIF.Text))
                    {
                        if (chkAdmin.Checked)
                        {
                            admi = true;
                        }
                        else
                        {
                            admi = false;
                        }
                        Usuario usu = new Usuario(txtNIF.Text,txtNombre.Text,txtApellido.Text,admi,txtClave.Text);
                        Usuario.AñadirUsuario(bdactevalu.Conexion, usu);
                        ListaUsuarios();
                    }
                    else
                    {
                        MessageBox.Show("Este usuario no se puede dar de alta, ss posible que ya exista o el nif sea incorrecto ");
                    }
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
                bdactevalu.CerrarConexion();
            
        }

        private void bntEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int resultado;
                int resFichajes;

                if (dtgUsuarios.SelectedRows.Count == 1)
                {
                    string nif = (string)dtgUsuarios.CurrentRow.Cells[0].Value;

                    DialogResult eliminacion = MessageBox.Show("¿Estas seguro que quieres borrar? también se eliminará toda la información de los fichajes de este usuario",
                                                "Eliminación", MessageBoxButtons.YesNo);

                    if (eliminacion == DialogResult.Yes)
                    {
                        if (bdactevalu.AbrirConexion())
                        {
                            resFichajes = Fichaje.EliminarFichaje(bdactevalu.Conexion, nif);
                            resultado = Usuario.EliminaUsuario(bdactevalu.Conexion,nif);
                            MessageBox.Show("Registros afectados: " + resultado);
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
            
                bdactevalu.CerrarConexion();
            
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {
            ListaUsuarios();
            ListaFichaje();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            
            DialogResult respuesta=MessageBox.Show("Cerrar aplicacion?", "Cerrar", MessageBoxButtons.YesNo);
            if (respuesta==DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Close();
                this.Dispose();
            }
            
            
        }
        /// <summary>
        /// Añade toda la información de los usuarios al datagreed
        /// </summary>
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

        /// <summary>
        /// Añade toda la información de los fichajes al datagreed
        /// </summary>
        private void ListaFichaje()
        {
            string consulta = "Select * from fichajes;";
            List<Fichaje> fichajes;
            if (bdactevalu.AbrirConexion())
            {
                fichajes = Fichaje.BuscaFichajes(bdactevalu.Conexion, consulta);
                dtgFichajes.DataSource = fichajes;
            }
            else
            {
                MessageBox.Show("No se puede abrir la Base de Datos");
            }
            bdactevalu.CerrarConexion();
        }

        private void txtNIF_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void chkAdmin_CheckedChanged(object sender, EventArgs e)
        {
            txtClave.Enabled = chkAdmin.Checked;
            if (!chkAdmin.Checked)
            {
                txtClave.Clear();
            }
        }

        private void frmMantenimiento_Load(object sender, EventArgs e)
        {

        }
    }
}
