using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class UsuariosForm : Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }

        UsuarioDatos userDatos = new UsuarioDatos();
        string tipoOperacion = string.Empty;
        //solo se declara para guardar
        Usuario user;

        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            LlenarDataGrid();
        }

        private async void LlenarDataGrid()
        {
            dgvUsuarios.DataSource = await userDatos.DevolverListaAsync();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            HabilitarControles();
            tipoOperacion = "Nuevo";
        }

        private void HabilitarControles()
        {
            txtCodigo.Enabled = true;
            txtNombre.Enabled = true;
            txtClave.Enabled = true;
            txtCorreo.Enabled = true;
            cbbRol.Enabled = true;
            cbActivo.Enabled = true;
        }

        private void DeshabilitarControles()
        {
            txtCodigo.Enabled = false;
            txtNombre.Enabled = false;
            txtClave.Enabled = false;
            txtCorreo.Enabled = false;
            cbbRol.Enabled = false;
            cbActivo.Enabled = false;
        }

        private void LimpiarControles()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtClave.Clear();
            txtCorreo.Clear();
            cbbRol.Text=String.Empty;
            cbActivo.Checked=false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            tipoOperacion = "Modificar";
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                txtCodigo.Text = dgvUsuarios.CurrentRow.Cells["Codigo"].Value.ToString();
                txtNombre.Text = dgvUsuarios.CurrentRow.Cells["Nombre"].Value.ToString();
                txtClave.Text = dgvUsuarios.CurrentRow.Cells["Clave"].Value.ToString();
                txtCorreo.Text = dgvUsuarios.CurrentRow.Cells["Correo"].Value.ToString();
                cbbRol.Text = dgvUsuarios.CurrentRow.Cells["Rol"].Value.ToString();
                cbActivo.Checked = Convert.ToBoolean(dgvUsuarios.CurrentRow.Cells["EstaActivo"].Value);
                HabilitarControles();
                txtCodigo.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            user = new Usuario();

            if (tipoOperacion == "Nuevo")
            {
                if (txtCodigo.Text=="")
                {
                    errorProvider1.SetError(txtCodigo, "Ingrese un código");
                    txtCodigo.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtNombre.Text))
                {
                    errorProvider1.SetError(txtNombre, "Ingrese un nombre");
                    txtNombre.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtClave.Text))
                {
                    errorProvider1.SetError(txtClave, "Ingrese una clave");
                    txtClave.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(cbbRol.Text))
                {
                    errorProvider1.SetError(cbbRol, "Seleccione un rol");
                    cbbRol.Focus();
                    return;
                }

                user.Codigo = txtCodigo.Text;
                user.Nombre = txtNombre.Text;
                user.Clave = txtClave.Text;
                user.Correo = txtCorreo.Text;
                user.Rol = cbbRol.Text;
                user.EstaActivo = cbActivo.Checked;

                bool inserto = await userDatos.InsertarAsync(user);

                if (inserto)
                {
                    LlenarDataGrid();
                    LimpiarControles();
                    DeshabilitarControles();
                    MessageBox.Show("Usuario guardado","" ,MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (tipoOperacion=="Modificar")
            {
                if (txtCodigo.Text == "")
                {
                    errorProvider1.SetError(txtCodigo, "Ingrese un código");
                    txtCodigo.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtNombre.Text))
                {
                    errorProvider1.SetError(txtNombre, "Ingrese un nombre");
                    txtNombre.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtClave.Text))
                {
                    errorProvider1.SetError(txtClave, "Ingrese una clave");
                    txtClave.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(cbbRol.Text))
                {
                    errorProvider1.SetError(cbbRol, "Seleccione un rol");
                    cbbRol.Focus();
                    return;
                }

                user.Codigo = txtCodigo.Text;
                user.Nombre = txtNombre.Text;
                user.Clave = txtClave.Text;
                user.Correo = txtCorreo.Text;
                user.Rol = cbbRol.Text;
                user.EstaActivo = cbActivo.Checked;

                bool modifico = await userDatos.ActualizarAsync(user);

                if (modifico)
                {
                    LlenarDataGrid();
                    LimpiarControles();
                    DeshabilitarControles();
                    MessageBox.Show("Usuario guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count>0)
            {
                bool elimino = await userDatos.EliminarAsync(dgvUsuarios.CurrentRow.Cells["Codigo"].Value.ToString());

                if (elimino)
                {
                    LlenarDataGrid();
          
                    MessageBox.Show("Usuario eliminado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo eliminar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
