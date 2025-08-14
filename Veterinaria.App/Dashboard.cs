using Veterinaria.BusinessLayer.Controllers;

namespace Veterinaria.App
{
    /// <summary>
    /// Dashboard principal del sistema - Pantalla principal después del login
    /// </summary>
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            ConfigurarDashboard();
        }

        private void ConfigurarDashboard()
        {
            // Configurar información del usuario
            if (AuthController.EstaAutenticado)
            {
                var userInfo = AuthController.GetUserInfo();
                if (userInfo != null)
                {
                    lblUsuario.Text = $"Bienvenido, {userInfo.NombreCompleto}";
                    lblEmail.Text = userInfo.Email;
                }
            }

            // Configurar eventos de botones
            btnClientes.Click += BtnClientes_Click;
            btnMascotas.Click += BtnMascotas_Click;
            btnVentas.Click += BtnVentas_Click;
            btnProductos.Click += BtnProductos_Click;
            btnReportes.Click += BtnReportes_Click;
            btnConfiguracion.Click += BtnConfiguracion_Click;
            btnLogout.Click += BtnLogout_Click;
        }

        private void BtnClientes_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar formulario de clientes
            MessageBox.Show("Módulo de Clientes - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnMascotas_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar formulario de mascotas
            MessageBox.Show("Módulo de Mascotas - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnVentas_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar formulario de ventas
            MessageBox.Show("Módulo de Ventas - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnProductos_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar formulario de productos
            MessageBox.Show("Módulo de Productos - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnReportes_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar formulario de reportes
            MessageBox.Show("Módulo de Reportes - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnConfiguracion_Click(object? sender, EventArgs e)
        {
            // TODO: Implementar formulario de configuración
            MessageBox.Show("Módulo de Configuración - Próximamente", "Información", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", 
                "Confirmar Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                AuthController.Logout();
                
                // Crear y mostrar formulario de login
                var loginForm = new Login();
                loginForm.Show();
                
                // Cerrar dashboard
                this.Hide();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea salir del sistema?", 
                "Confirmar Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            AuthController.Logout();
            base.OnFormClosing(e);
        }
    }
}
