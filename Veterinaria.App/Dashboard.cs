using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.App.Navigation;
using Veterinaria.App.Views.Cliente;

namespace Veterinaria.App
{
    /// <summary>
    /// Dashboard principal del sistema - Pantalla principal después del login
    /// </summary>
    public partial class Dashboard : Form
    {
        private NavigationManager? _navigationManager;

        public Dashboard()
        {
            InitializeComponent();
            ConfigurarDashboard();
            InicializarNavegacion();
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

        /// <summary>
        /// Inicializa el sistema de navegación modular
        /// </summary>
        private void InicializarNavegacion()
        {
            try
            {
                // Limpiar el panel de contenido
                panelContent.Controls.Clear();

                // Crear el navegador
                _navigationManager = new NavigationManager(panelContent);
                _navigationManager.NavigationRequested += OnNavigationRequested;

                // Mostrar mensaje de bienvenida inicial
                MostrarPantallaBienvenida();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inicializando navegación: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Muestra la pantalla de bienvenida inicial
        /// </summary>
        private void MostrarPantallaBienvenida()
        {
            var welcomePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(236, 240, 241)
            };

            var welcomeLabel = new Label
            {
                Text = "Sistema de Veterinaria",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true
            };
            
            welcomeLabel.Location = new Point(
                (welcomePanel.Width - welcomeLabel.Width) / 2,
                (welcomePanel.Height - welcomeLabel.Height) / 2
            );

            var instructionLabel = new Label
            {
                Text = "Seleccione una opción del menú lateral para comenzar",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true
            };

            instructionLabel.Location = new Point(
                (welcomePanel.Width - instructionLabel.Width) / 2,
                welcomeLabel.Bottom + 20
            );

            welcomePanel.Controls.Add(welcomeLabel);
            welcomePanel.Controls.Add(instructionLabel);

            // Centrar labels cuando el panel se redimensione
            welcomePanel.Resize += (s, e) =>
            {
                welcomeLabel.Location = new Point(
                    (welcomePanel.Width - welcomeLabel.Width) / 2,
                    (welcomePanel.Height - welcomeLabel.Height) / 2 - 20
                );
                instructionLabel.Location = new Point(
                    (welcomePanel.Width - instructionLabel.Width) / 2,
                    welcomeLabel.Bottom + 10
                );
            };

            panelContent.Controls.Add(welcomePanel);
        }

        /// <summary>
        /// Maneja las solicitudes de navegación entre vistas
        /// </summary>
        private void OnNavigationRequested(string module, ViewType viewType, object? data)
        {
            try
            {
                switch (module.ToLower())
                {
                    case "cliente":
                        ManejarNavegacionCliente(viewType, data);
                        break;
                    
                    case "mascota":
                        // TODO: Implementar navegación de mascotas
                        MessageBox.Show("Módulo de Mascotas - Próximamente", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    
                    case "venta":
                        // TODO: Implementar navegación de ventas
                        MessageBox.Show("Módulo de Ventas - Próximamente", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    
                    default:
                        MessageBox.Show($"Módulo '{module}' no implementado", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en navegación: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Maneja la navegación específica del módulo de Clientes
        /// </summary>
        private void ManejarNavegacionCliente(ViewType viewType, object? data)
        {
            UserControl? vista = null;

            switch (viewType)
            {
                case ViewType.Index:
                    var indexView = new ClienteIndexView();
                    
                    // Configurar eventos
                    indexView.NuevoCliente += () => 
                        _navigationManager?.NavigateTo("Cliente", ViewType.Create, new ClienteFormView(), null);
                    
                    indexView.EditarCliente += (id) => 
                    {
                        var cliente = ClienteController.GetById(id);
                        if (cliente != null)
                        {
                            var formView = new ClienteFormView();
                            formView.ConfigurarParaEdicion(cliente);
                            _navigationManager?.NavigateTo("Cliente", ViewType.Edit, formView, cliente);
                        }
                    };
                    
                    indexView.EliminarCliente += (id) => ConfirmarEliminarCliente(id);
                    
                    vista = indexView;
                    break;

                case ViewType.Create:
                    var createView = new ClienteFormView();
                    createView.ConfigurarParaNuevo();
                    
                    // Configurar eventos
                    createView.ClienteGuardado += () =>
                        _navigationManager?.NavigateTo("Cliente", ViewType.Index, new ClienteIndexView(), null);
                    
                    createView.CancelarOperacion += () =>
                        _navigationManager?.NavigateTo("Cliente", ViewType.Index, new ClienteIndexView(), null);
                    
                    vista = createView;
                    break;

                case ViewType.Edit:
                    if (data is ModelLayer.Cliente cliente)
                    {
                        var editView = new ClienteFormView();
                        editView.ConfigurarParaEdicion(cliente);
                        
                        // Configurar eventos
                        editView.ClienteGuardado += () =>
                            _navigationManager?.NavigateTo("Cliente", ViewType.Index, new ClienteIndexView(), null);
                        
                        editView.CancelarOperacion += () =>
                            _navigationManager?.NavigateTo("Cliente", ViewType.Index, new ClienteIndexView(), null);
                        
                        vista = editView;
                    }
                    break;
            }

            if (vista != null && _navigationManager != null)
            {
                _navigationManager.NavigateTo("Cliente", viewType, vista, data);
            }
        }

        /// <summary>
        /// Confirma y ejecuta la eliminación de un cliente
        /// </summary>
        private void ConfirmarEliminarCliente(int clienteId)
        {
            try
            {
                var cliente = ClienteController.GetById(clienteId);
                if (cliente == null)
                {
                    MessageBox.Show("Cliente no encontrado", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"¿Está seguro que desea eliminar al cliente '{cliente.NombreCompleto()}'?\n\n" +
                    "Esta acción no se puede deshacer.",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var (success, message) = ClienteController.Delete(clienteId);
                    
                    if (success)
                    {
                        MessageBox.Show(message, "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Refrescar la vista actual si es el índice de clientes
                        var currentView = panelContent.Controls.OfType<ClienteIndexView>().FirstOrDefault();
                        currentView?.RefrescarDatos();
                    }
                    else
                    {
                        MessageBox.Show(message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cliente: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClientes_Click(object? sender, EventArgs e)
        {
            try
            {
                // Navegar al módulo de clientes
                var clienteIndexView = new ClienteIndexView();
                
                // Configurar eventos
                clienteIndexView.NuevoCliente += () =>
                    _navigationManager?.NavigateTo("Cliente", ViewType.Create, new ClienteFormView(), null);
                
                clienteIndexView.EditarCliente += (id) =>
                {
                    var cliente = ClienteController.GetById(id);
                    if (cliente != null)
                    {
                        var formView = new ClienteFormView();
                        formView.ConfigurarParaEdicion(cliente);
                        _navigationManager?.NavigateTo("Cliente", ViewType.Edit, formView, cliente);
                    }
                };
                
                clienteIndexView.EliminarCliente += (id) => ConfirmarEliminarCliente(id);

                _navigationManager?.NavigateTo("Cliente", ViewType.Index, clienteIndexView, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir módulo de clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            // Limpiar recursos de navegación
            _navigationManager?.Dispose();
            AuthController.Logout();
            base.OnFormClosing(e);
        }
    }
}
