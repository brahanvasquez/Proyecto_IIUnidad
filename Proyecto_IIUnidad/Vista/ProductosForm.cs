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
    public partial class ProductosForm : Form
    {
        public ProductosForm()
        {
            InitializeComponent();
        }

        ProductoDatos proDatos = new ProductoDatos();
        Producto producto = new Producto();
        string tipoOperacion = string.Empty;

        private void ProductosForm_Load(object sender, EventArgs e)
        {
            LlenarProductos();
        }

        private async void LlenarProductos()
        {
            dgvProductos.DataSource = await proDatos.DevolverListaAsync();
        }

        private void HabilitarControles()
        {
            txtCodigo.Enabled = true;
            txtDescripcion.Enabled = true;
            txtPrecio.Enabled = true;
            txtExistencia.Enabled = true;
            dtpFecha.Enabled = true;
            dgvProductos.Enabled = true;
            btnAdjuntar.Enabled = true; 
        }

        private void DeshabilitarControles()
        {
            txtCodigo.Enabled = false;
            txtDescripcion.Enabled = false;
            txtPrecio.Enabled = false;
            txtExistencia.Enabled = false;
            dtpFecha.Enabled = false;
            dgvProductos.Enabled = false;
            btnAdjuntar.Enabled = false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tipoOperacion = "Nuevo";
            HabilitarControles();
        }
    }
}
