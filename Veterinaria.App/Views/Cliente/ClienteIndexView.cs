using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.ModelLayer;

namespace Veterinaria.App.Views.Cliente
{
    /// <summary>
    /// Vista principal (Index) para gestión de clientes
    /// Muestra lista de clientes con opciones de búsqueda, crear, editar y eliminar
    /// </summary>
    public partial class ClienteIndexView : UserControl
    {
        // Eventos para comunicación con el Dashboard
        public event Action<int>? EditarCliente;
        public event Action? NuevoCliente;
        public event Action<int>? VerCliente;
        public event Action<int>? EliminarCliente;

        private List<ModelLayer.Cliente> _clientes = new List<ModelLayer.Cliente>();
        private bool _isLoading = false;

        public ClienteIndexView()
        {
            InitializeComponent();
            ConfigurarEventos();
            ConfigurarDataGridView();
            CargarClientes();
        }

        /// <summary>
        /// Configura los eventos de los controles
        /// </summary>
        private void ConfigurarEventos()
        {
            // Eventos de botones
            btnNuevoCliente.Click += BtnNuevoCliente_Click;
            btnBuscar.Click += BtnBuscar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnActualizar.Click += BtnActualizar_Click;

            // Eventos del TextBox de búsqueda
            txtBuscar.KeyDown += TxtBuscar_KeyDown;

            // Eventos del DataGridView
            dgvClientes.CellDoubleClick += DgvClientes_CellDoubleClick;
            dgvClientes.CellFormatting += DgvClientes_CellFormatting;
        }

        /// <summary>
        /// Configura las columnas del DataGridView
        /// </summary>
        private void ConfigurarDataGridView()
        {
            dgvClientes.AutoGenerateColumns = false;

            // Columna ID (oculta)
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Visible = false
            });

            // Columna Nombre Completo
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreCompleto",
                HeaderText = "Nombre Completo",
                DataPropertyName = "NombreCompleto",
                Width = 200,
                ReadOnly = true
            });

            // Columna Email
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 200,
                ReadOnly = true
            });

            // Columna Teléfono
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Telefono",
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                Width = 120,
                ReadOnly = true
            });

            // Columna Dirección
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Direccion",
                HeaderText = "Dirección",
                DataPropertyName = "Direccion",
                Width = 250,
                ReadOnly = true
            });

            // Columna Estado
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = "Estado",
                DataPropertyName = "EstadoTexto",
                Width = 80,
                ReadOnly = true
            });

            // Columna de Acciones
            var columnAcciones = new DataGridViewButtonColumn
            {
                Name = "Acciones",
                HeaderText = "Acciones",
                Width = 100,
                UseColumnTextForButtonValue = false
            };
            dgvClientes.Columns.Add(columnAcciones);

            // Evento para manejar clicks en botones de acción
            dgvClientes.CellContentClick += DgvClientes_CellContentClick;
        }

        /// <summary>
        /// Carga la lista de clientes desde el controlador
        /// </summary>
        private async void CargarClientes()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                btnActualizar.Enabled = false;
                btnActualizar.Text = "?? Cargando...";

                await Task.Run(() =>
                {
                    _clientes = ClienteController.GetAll();
                });

                // Crear lista con propiedades calculadas para el DataGridView
                var clientesDisplay = _clientes.Select(c => new
                {
                    Id = c.Id,
                    NombreCompleto = c.NombreCompleto(),
                    Email = c.Email ?? "Sin email",
                    Telefono = c.Telefono ?? "Sin teléfono",
                    Direccion = c.Direccion ?? "Sin dirección",
                    EstadoTexto = c.Activo ? "Activo" : "Inactivo",
                    Activo = c.Activo
                }).ToList();

                dgvClientes.DataSource = clientesDisplay;
                ActualizarContador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
                btnActualizar.Enabled = true;
                btnActualizar.Text = "?? Actualizar";
            }
        }

        /// <summary>
        /// Busca clientes según el texto ingresado
        /// </summary>
        private async void BuscarClientes()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                btnBuscar.Enabled = false;
                btnBuscar.Text = "?? Buscando...";

                var textoBusqueda = txtBuscar.Text.Trim();

                await Task.Run(() =>
                {
                    _clientes = ClienteController.Search(textoBusqueda);
                });

                var clientesDisplay = _clientes.Select(c => new
                {
                    Id = c.Id,
                    NombreCompleto = c.NombreCompleto(),
                    Email = c.Email ?? "Sin email",
                    Telefono = c.Telefono ?? "Sin teléfono",
                    Direccion = c.Direccion ?? "Sin dirección",
                    EstadoTexto = c.Activo ? "Activo" : "Inactivo",
                    Activo = c.Activo
                }).ToList();

                dgvClientes.DataSource = clientesDisplay;
                ActualizarContador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
                btnBuscar.Enabled = true;
                btnBuscar.Text = "?? Buscar";
            }
        }

        /// <summary>
        /// Actualiza el contador de resultados
        /// </summary>
        private void ActualizarContador()
        {
            var total = _clientes.Count;
            var activos = _clientes.Count(c => c.Activo);
            var inactivos = total - activos;

            lblResultados.Text = $"Mostrando {total} cliente(s) - {activos} activo(s), {inactivos} inactivo(s)";
        }

        #region Event Handlers

        private void BtnNuevoCliente_Click(object? sender, EventArgs e)
        {
            NuevoCliente?.Invoke();
        }

        private void BtnBuscar_Click(object? sender, EventArgs e)
        {
            BuscarClientes();
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarClientes();
        }

        private void BtnActualizar_Click(object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarClientes();
            }
        }

        private void DgvClientes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var clienteId = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["Id"].Value);
                VerCliente?.Invoke(clienteId);
            }
        }

        private void DgvClientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvClientes.Columns[e.ColumnIndex].Name == "Estado")
            {
                if (e.Value?.ToString() == "Inactivo")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(46, 204, 113);
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
            }
        }

        private void DgvClientes_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvClientes.Columns[e.ColumnIndex].Name;

                if (columnName == "Acciones")
                {
                    var clienteId = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["Id"].Value);
                    
                    // Mostrar menú contextual con opciones
                    MostrarMenuAcciones(clienteId);
                }
            }
        }

        #endregion

        /// <summary>
        /// Muestra el menú de acciones para un cliente
        /// </summary>
        private void MostrarMenuAcciones(int clienteId)
        {
            var menu = new ContextMenuStrip();

            // Ver detalles
            var verItem = new ToolStripMenuItem("??? Ver detalles");
            verItem.Click += (s, e) => VerCliente?.Invoke(clienteId);
            menu.Items.Add(verItem);

            // Editar
            var editarItem = new ToolStripMenuItem("?? Editar");
            editarItem.Click += (s, e) => EditarCliente?.Invoke(clienteId);
            menu.Items.Add(editarItem);

            menu.Items.Add(new ToolStripSeparator());

            // Eliminar
            var eliminarItem = new ToolStripMenuItem("??? Eliminar");
            eliminarItem.Click += (s, e) => EliminarCliente?.Invoke(clienteId);
            eliminarItem.ForeColor = Color.FromArgb(231, 76, 60);
            menu.Items.Add(eliminarItem);

            // Mostrar el menú en la posición del cursor
            menu.Show(Cursor.Position);
        }

        /// <summary>
        /// Método público para refrescar la vista (llamado desde el Dashboard)
        /// </summary>
        public void RefrescarDatos()
        {
            CargarClientes();
        }

        /// <summary>
        /// Obtiene el cliente seleccionado actualmente
        /// </summary>
        public int? GetClienteSeleccionado()
        {
            if (dgvClientes.CurrentRow != null)
            {
                return Convert.ToInt32(dgvClientes.CurrentRow.Cells["Id"].Value);
            }
            return null;
        }
    }
}