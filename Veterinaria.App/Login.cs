using Veterinaria.BusinessLayer.Controllers;

namespace Veterinaria.App
{
    /// <summary>
    /// Formulario de Login - Autenticación de usuarios
    /// </summary>
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            ConfigurarLogin();
        }

        private void ConfigurarLogin()
        {
            // Configurar eventos
            btnLogin.Click += BtnLogin_Click;
            btnCancelar.Click += BtnCancelar_Click;
            
            // Configurar tecla Enter para login
            this.KeyPreview = true;
            this.KeyDown += Login_KeyDown;
            
            // Configurar focus inicial
            txtUsuario.Focus();
            
            // Configurar placeholder text (opcional)
            if (string.IsNullOrEmpty(txtUsuario.Text))
                txtUsuario.Text = "Usuario o Email";
            
            txtUsuario.Enter += TxtUsuario_Enter;
            txtUsuario.Leave += TxtUsuario_Leave;
        }

        private void Login_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnLogin_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnCancelar_Click(sender, e);
            }
        }

        private void TxtUsuario_Enter(object? sender, EventArgs e)
        {
            if (txtUsuario.Text == "Usuario o Email")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
            }
        }

        private void TxtUsuario_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "Usuario o Email";
                txtUsuario.ForeColor = Color.Gray;
            }
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            await RealizarLogin();
        }

        private async Task RealizarLogin()
        {
            try
            {
                // Deshabilitar controles durante el login
                btnLogin.Enabled = false;
                btnLogin.Text = "Ingresando...";
                
                // Obtener credenciales
                var usuario = txtUsuario.Text == "Usuario o Email" ? "" : txtUsuario.Text.Trim();
                var contrasena = txtContrasena.Text;

                // Validar campos
                if (string.IsNullOrWhiteSpace(usuario))
                {
                    MessageBox.Show("Por favor ingrese su usuario o email.", "Campo Requerido", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsuario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(contrasena))
                {
                    MessageBox.Show("Por favor ingrese su contraseña.", "Campo Requerido", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContrasena.Focus();
                    return;
                }

                // Realizar autenticación
                var resultado = await AuthController.LoginAsync(usuario, contrasena);

                if (resultado.Exitoso)
                {
                    // Login exitoso - mostrar dashboard
                    var dashboard = new Dashboard();
                    dashboard.Show();
                    
                    // Ocultar formulario de login
                    this.Hide();
                }
                else
                {
                    // Mostrar error
                    MessageBox.Show(resultado.Mensaje, "Error de Autenticación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    // Limpiar contraseña y enfocar usuario
                    txtContrasena.Text = "";
                    txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Rehabilitar controles
                btnLogin.Enabled = true;
                btnLogin.Text = "Ingresar";
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea salir de la aplicación?", 
                "Confirmar Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void LblCrearCuenta_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar formulario de registro
            MessageBox.Show("Funcionalidad de registro - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LblOlvideContrasena_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar recuperación de contraseña
            MessageBox.Show("Recuperación de contraseña - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("¿Está seguro que desea salir de la aplicación?", 
                    "Confirmar Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            
            base.OnFormClosing(e);
        }
    }
}
