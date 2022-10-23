using Datos;

namespace Vista
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == String.Empty)
            {
                errorProvider1.SetError(txtUsuario, "Ingrese un usuario válido");
                txtUsuario.Focus();
                return;
            }
            errorProvider1.Clear();

            if (txtClave.Text == String.Empty)
            {
                errorProvider1.SetError(txtClave, "Ingrese una clave");
                txtClave.Focus();
                return;
            }
            errorProvider1.Clear();

            UsuarioDatos userDatos = new UsuarioDatos();

            bool valido = await userDatos.LoginAsync(txtUsuario.Text, txtClave.Text);

            if (valido)
            {
                Menu formulario = new Menu();
                Hide();
                formulario.Show();
            }
            else
            {
                MessageBox.Show("Datos del usuario incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}