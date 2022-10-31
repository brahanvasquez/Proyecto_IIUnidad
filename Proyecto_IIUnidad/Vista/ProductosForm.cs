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
        Producto producto;
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
            picbImagen.Enabled = true;
            btnAdjuntar.Enabled = true; 
        }

        private void LimpiarControles()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtExistencia.Clear();
            dtpFecha.Value=DateTime.Now;
            picbImagen.Image = null;
            
        }

        private void DeshabilitarControles()
        {
            txtCodigo.Enabled = false;
            txtDescripcion.Enabled = false;
            txtPrecio.Enabled = false;
            txtExistencia.Enabled = false;
            dtpFecha.Enabled = false;
            picbImagen.Enabled = false;
            btnAdjuntar.Enabled = false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tipoOperacion = "Nuevo";
            HabilitarControles();
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            

            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                errorProvider1.SetError(txtCodigo, "Ingrese el código");
                txtCodigo.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                errorProvider1.SetError(txtDescripcion, "Ingrese una descripción");
                txtDescripcion.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                errorProvider1.SetError(txtPrecio, "Ingrese el precio");
                txtPrecio.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtExistencia.Text))
            {
                errorProvider1.SetError(txtExistencia, "Ingrese una existencia");
                txtExistencia.Focus();
                return;
            }

            producto = new Producto();

            if (picbImagen.Image != null)
            {
                //SYSTEM.IO
                MemoryStream ms = new MemoryStream();

                picbImagen.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                producto.Imagen = ms.GetBuffer();
            }
            else
            {
                producto.Imagen = null;
            }

            producto.Codigo =Convert.ToInt32(txtCodigo.Text);
            producto.Descripcion=txtDescripcion.Text;
            producto.Existencia = Convert.ToInt32(txtExistencia.Text);
            producto.Precio = Convert.ToDecimal(txtPrecio.Text);
            producto.FechaCreacion = dtpFecha.Value;

            if (tipoOperacion=="Nuevo")
            {
                bool inserto = await proDatos.InsertarAsync(producto);
                if (inserto)
                {
                    LlenarProductos();
                    LimpiarControles();
                    DeshabilitarControles();
                    MessageBox.Show("Producto guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Producto no se pudo guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (tipoOperacion=="Modificar")
            {
                bool modifico = await proDatos.ActualizarAsync(producto);
                if (modifico)
                {
                    LlenarProductos();
                    LimpiarControles();
                    DeshabilitarControles();
                    MessageBox.Show("Producto guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Producto no se pudo guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdjuntar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                picbImagen.Image = Image.FromFile(dialog.FileName);
            }
        }

        private async void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                tipoOperacion = "Modificar";
                HabilitarControles();
                txtCodigo.ReadOnly = true;
                txtCodigo.Text = dgvProductos.CurrentRow.Cells["Codigo"].Value.ToString();
                txtDescripcion.Text = dgvProductos.CurrentRow.Cells["Descripcion"].Value.ToString();
                txtExistencia.Text = dgvProductos.CurrentRow.Cells["Existencia"].Value.ToString();
                txtPrecio.Text = dgvProductos.CurrentRow.Cells["Precio"].Value.ToString();
                dtpFecha.Value = Convert.ToDateTime(dgvProductos.CurrentRow.Cells["FechaCreacion"].Value);

                byte[] imagenDeBaseDatos = await proDatos.SeleccionarImagen(dgvProductos.CurrentRow.Cells["Codigo"].Value.ToString());

                if (imagenDeBaseDatos.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(imagenDeBaseDatos);

                    picbImagen.Image = System.Drawing.Bitmap.FromStream(ms);
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }



            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                bool elimino = await proDatos.EliminarAsync(dgvProductos.CurrentRow.Cells["Codigo"].Value.ToString());
                if (elimino)
                {
                    LlenarProductos();
                
                    MessageBox.Show("Producto eliminado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Producto no se pudo eliminar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtExistencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar !='.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.')>-1))
            {
                e.Handled = true;
            }
        }
    }
}
