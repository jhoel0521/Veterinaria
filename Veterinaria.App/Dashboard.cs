using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.App.Navigation;
using Veterinaria.App.Views.Cliente;
using Veterinaria.App.Views.Mascota;

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
                        ManejarNavegacionMascota(viewType, data);
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
                    
                    // Configurar eventos usando una lambda que mantenga la referencia
                    indexView.NuevoCliente += () => 
                    {
                        var createView = new ClienteFormView();
                        createView.ConfigurarParaNuevo();
                        
                        // Configurar eventos del formulario
                        createView.ClienteGuardado += () =>
                        {
                            // Crear nueva instancia del índice y navegar
                            var newIndexView = new ClienteIndexView();
                            ConfigurarEventosClienteIndex(newIndexView);
                            _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                        };
                        
                        createView.CancelarOperacion += () =>
                        {
                            // Crear nueva instancia del índice y navegar
                            var newIndexView = new ClienteIndexView();
                            ConfigurarEventosClienteIndex(newIndexView);
                            _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                        };
                        
                        _navigationManager?.NavigateTo("Cliente", ViewType.Create, createView, null);
                    };
                    
                    indexView.EditarCliente += (id) => 
                    {
                        var cliente = ClienteController.GetById(id);
                        if (cliente != null)
                        {
                            var editView = new ClienteFormView();
                            editView.ConfigurarParaEdicion(cliente);
                            
                            // Configurar eventos del formulario
                            editView.ClienteGuardado += () =>
                            {
                                // Crear nueva instancia del índice y navegar
                                var newIndexView = new ClienteIndexView();
                                ConfigurarEventosClienteIndex(newIndexView);
                                _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                            };
                            
                            editView.CancelarOperacion += () =>
                            {
                                // Crear nueva instancia del índice y navegar
                                var newIndexView = new ClienteIndexView();
                                ConfigurarEventosClienteIndex(newIndexView);
                                _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                            };
                            
                            _navigationManager?.NavigateTo("Cliente", ViewType.Edit, editView, cliente);
                        }
                    };
                    
                    indexView.EliminarCliente += (id) => ConfirmarEliminarCliente(id);
                    
                    vista = indexView;
                    break;

                case ViewType.Create:
                    var createFormView = new ClienteFormView();
                    createFormView.ConfigurarParaNuevo();
                    
                    // Configurar eventos
                    createFormView.ClienteGuardado += () =>
                    {
                        var newIndexView = new ClienteIndexView();
                        ConfigurarEventosClienteIndex(newIndexView);
                        _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                    };
                    
                    createFormView.CancelarOperacion += () =>
                    {
                        var newIndexView = new ClienteIndexView();
                        ConfigurarEventosClienteIndex(newIndexView);
                        _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                    };
                    
                    vista = createFormView;
                    break;

                case ViewType.Edit:
                    if (data is ModelLayer.Cliente cliente)
                    {
                        var editFormView = new ClienteFormView();
                        editFormView.ConfigurarParaEdicion(cliente);
                        
                        // Configurar eventos
                        editFormView.ClienteGuardado += () =>
                        {
                            var newIndexView = new ClienteIndexView();
                            ConfigurarEventosClienteIndex(newIndexView);
                            _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                        };
                        
                        editFormView.CancelarOperacion += () =>
                        {
                            var newIndexView = new ClienteIndexView();
                            ConfigurarEventosClienteIndex(newIndexView);
                            _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                        };
                        
                        vista = editFormView;
                    }
                    break;
            }

            if (vista != null && _navigationManager != null)
            {
                _navigationManager.NavigateTo("Cliente", viewType, vista, data);
            }
        }

        /// <summary>
        /// Configura los eventos comunes para las vistas de índice de cliente
        /// </summary>
        private void ConfigurarEventosClienteIndex(ClienteIndexView indexView)
        {
            indexView.NuevoCliente += () =>
            {
                var createView = new ClienteFormView();
                createView.ConfigurarParaNuevo();
                
                createView.ClienteGuardado += () =>
                {
                    var newIndexView = new ClienteIndexView();
                    ConfigurarEventosClienteIndex(newIndexView);
                    _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                };
                
                createView.CancelarOperacion += () =>
                {
                    var newIndexView = new ClienteIndexView();
                    ConfigurarEventosClienteIndex(newIndexView);
                    _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                };
                
                _navigationManager?.NavigateTo("Cliente", ViewType.Create, createView, null);
            };
            
            indexView.EditarCliente += (id) =>
            {
                var cliente = ClienteController.GetById(id);
                if (cliente != null)
                {
                    var editView = new ClienteFormView();
                    editView.ConfigurarParaEdicion(cliente);
                    
                    editView.ClienteGuardado += () =>
                    {
                        var newIndexView = new ClienteIndexView();
                        ConfigurarEventosClienteIndex(newIndexView);
                        _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                    };
                    
                    editView.CancelarOperacion += () =>
                    {
                        var newIndexView = new ClienteIndexView();
                        ConfigurarEventosClienteIndex(newIndexView);
                        _navigationManager?.NavigateTo("Cliente", ViewType.Index, newIndexView, null);
                    };
                    
                    _navigationManager?.NavigateTo("Cliente", ViewType.Edit, editView, cliente);
                }
            };
            
            indexView.EliminarCliente += (id) => ConfirmarEliminarCliente(id);
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
                ConfigurarEventosClienteIndex(clienteIndexView);
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
            try
            {
                // Navegar al módulo de mascotas
                var mascotaIndexView = new MascotaIndexView();
                ConfigurarEventosMascotaIndex(mascotaIndexView);
                _navigationManager?.NavigateTo("Mascota", ViewType.Index, mascotaIndexView, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir módulo de mascotas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                // Hacer logout pero NO crear nuevo formulario de login
                AuthController.Logout();
                
                // Cerrar el dashboard - esto hará que se regrese al login automáticamente
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Solo preguntar si el usuario está cerrando manualmente (no cuando se hace logout)
            if (e.CloseReason == CloseReason.UserClosing && AuthController.EstaAutenticado)
            {
                var result = MessageBox.Show("¿Está seguro que desea salir del sistema?", 
                    "Confirmar Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Limpiar recursos de navegación
            _navigationManager?.Dispose();
            
            // Si aún está autenticado, hacer logout (por si cerró directamente sin usar logout)
            if (AuthController.EstaAutenticado)
            {
                AuthController.Logout();
            }
            
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Maneja la navegación específica del módulo de Mascotas
        /// </summary>
        private void ManejarNavegacionMascota(ViewType viewType, object? data)
        {
            UserControl? vista = null;

            switch (viewType)
            {
                case ViewType.Index:
                    var indexView = new MascotaIndexView();
                    
                    // Configurar eventos usando una lambda que mantenga la referencia
                    indexView.NuevaMascota += () => 
                    {
                        var createView = new MascotaFormView();
                        createView.ConfigurarParaNuevo();
                        
                        // Configurar eventos del formulario
                        createView.MascotaGuardada += () =>
                        {
                            // Crear nueva instancia del índice y navegar
                            var newIndexView = new MascotaIndexView();
                            ConfigurarEventosMascotaIndex(newIndexView);
                            _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                        };
                        
                        createView.CancelarOperacion += () =>
                        {
                            // Crear nueva instancia del índice y navegar
                            var newIndexView = new MascotaIndexView();
                            ConfigurarEventosMascotaIndex(newIndexView);
                            _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                        };
                        
                        _navigationManager?.NavigateTo("Mascota", ViewType.Create, createView, null);
                    };
                    
                    indexView.EditarMascota += (id) => 
                    {
                        var mascota = MascotaController.GetById(id);
                        if (mascota != null)
                        {
                            var editView = new MascotaFormView();
                            editView.ConfigurarParaEdicion(mascota);
                            
                            // Configurar eventos del formulario
                            editView.MascotaGuardada += () =>
                            {
                                // Crear nueva instancia del índice y navegar
                                var newIndexView = new MascotaIndexView();
                                ConfigurarEventosMascotaIndex(newIndexView);
                                _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                            };
                            
                            editView.CancelarOperacion += () =>
                            {
                                // Crear nueva instancia del índice y navegar
                                var newIndexView = new MascotaIndexView();
                                ConfigurarEventosMascotaIndex(newIndexView);
                                _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                            };
                            
                            _navigationManager?.NavigateTo("Mascota", ViewType.Edit, editView, mascota);
                        }
                    };
                    
                    indexView.EliminarMascota += (id) => ConfirmarEliminarMascota(id);
                    
                    vista = indexView;
                    break;

                case ViewType.Create:
                    var createFormView = new MascotaFormView();
                    createFormView.ConfigurarParaNuevo();
                    
                    // Configurar eventos
                    createFormView.MascotaGuardada += () =>
                    {
                        var newIndexView = new MascotaIndexView();
                        ConfigurarEventosMascotaIndex(newIndexView);
                        _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                    };
                    
                    createFormView.CancelarOperacion += () =>
                    {
                        var newIndexView = new MascotaIndexView();
                        ConfigurarEventosMascotaIndex(newIndexView);
                        _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                    };
                    
                    vista = createFormView;
                    break;

                case ViewType.Edit:
                    if (data is ModelLayer.Mascota mascota)
                    {
                        var editFormView = new MascotaFormView();
                        editFormView.ConfigurarParaEdicion(mascota);
                        
                        // Configurar eventos
                        editFormView.MascotaGuardada += () =>
                        {
                            var newIndexView = new MascotaIndexView();
                            ConfigurarEventosMascotaIndex(newIndexView);
                            _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                        };
                        
                        editFormView.CancelarOperacion += () =>
                        {
                            var newIndexView = new MascotaIndexView();
                            ConfigurarEventosMascotaIndex(newIndexView);
                            _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                        };
                        
                        vista = editFormView;
                    }
                    break;
            }

            if (vista != null && _navigationManager != null)
            {
                _navigationManager.NavigateTo("Mascota", viewType, vista, data);
            }
        }

        /// <summary>
        /// Configura los eventos comunes para las vistas de índice de mascota
        /// </summary>
        private void ConfigurarEventosMascotaIndex(MascotaIndexView indexView)
        {
            indexView.NuevaMascota += () =>
            {
                var createView = new MascotaFormView();
                createView.ConfigurarParaNuevo();
                
                createView.MascotaGuardada += () =>
                {
                    var newIndexView = new MascotaIndexView();
                    ConfigurarEventosMascotaIndex(newIndexView);
                    _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                };
                
                createView.CancelarOperacion += () =>
                {
                    var newIndexView = new MascotaIndexView();
                    ConfigurarEventosMascotaIndex(newIndexView);
                    _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                };
                
                _navigationManager?.NavigateTo("Mascota", ViewType.Create, createView, null);
            };
            
            indexView.EditarMascota += (id) =>
            {
                var mascota = MascotaController.GetById(id);
                if (mascota != null)
                {
                    var editView = new MascotaFormView();
                    editView.ConfigurarParaEdicion(mascota);
                    
                    editView.MascotaGuardada += () =>
                    {
                        var newIndexView = new MascotaIndexView();
                        ConfigurarEventosMascotaIndex(newIndexView);
                        _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                    };
                    
                    editView.CancelarOperacion += () =>
                    {
                        var newIndexView = new MascotaIndexView();
                        ConfigurarEventosMascotaIndex(newIndexView);
                        _navigationManager?.NavigateTo("Mascota", ViewType.Index, newIndexView, null);
                    };
                    
                    _navigationManager?.NavigateTo("Mascota", ViewType.Edit, editView, mascota);
                }
            };
            
            indexView.EliminarMascota += (id) => ConfirmarEliminarMascota(id);
        }

        /// <summary>
        /// Confirma y ejecuta la eliminación de una mascota
        /// </summary>
        private void ConfirmarEliminarMascota(int mascotaId)
        {
            try
            {
                var mascota = MascotaController.GetById(mascotaId);
                if (mascota == null)
                {
                    MessageBox.Show("Mascota no encontrada", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"¿Está seguro que desea eliminar a la mascota '{mascota.Nombre}'?\n\n" +
                    "Esta acción no se puede deshacer.",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var (success, message) = MascotaController.Delete(mascotaId);
                    
                    if (success)
                    {
                        MessageBox.Show(message, "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Refrescar la vista actual si es el índice de mascotas
                        var currentView = panelContent.Controls.OfType<MascotaIndexView>().FirstOrDefault();
                        currentView?.RefrescarVista();
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
                MessageBox.Show($"Error al eliminar mascota: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
