using Datos;
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

        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            LlenasDataGrid();
        }

        private async void LlenasDataGrid()
        {
            dgvUsuarios.DataSource = await userDatos.DevolverListaAsync();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            HabilitarControles();
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
    }
}
