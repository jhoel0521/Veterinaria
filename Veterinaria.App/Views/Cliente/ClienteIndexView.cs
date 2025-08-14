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
            
            // Agregar evento MouseEnter para efecto hover en botones
            dgvClientes.CellMouseEnter += DgvClientes_CellMouseEnter;
            dgvClientes.CellMouseLeave += DgvClientes_CellMouseLeave;
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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            // Columna Email
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 180,
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

            // Columna Estado
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = "Estado",
                DataPropertyName = "EstadoTexto",
                Width = 80,
                ReadOnly = true
            });

            // Botón Editar
            var btnEditarColumn = new DataGridViewButtonColumn
            {
                Name = "BtnEditar",
                HeaderText = "",
                Text = "✏️ Editar",
                UseColumnTextForButtonValue = true,
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                FlatStyle = FlatStyle.Flat
            };
            dgvClientes.Columns.Add(btnEditarColumn);

            // Botón Eliminar
            var btnEliminarColumn = new DataGridViewButtonColumn
            {
                Name = "BtnEliminar",
                HeaderText = "",
                Text = "🗑️ Eliminar",
                UseColumnTextForButtonValue = true,
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(231, 76, 60),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                FlatStyle = FlatStyle.Flat
            };
            dgvClientes.Columns.Add(btnEliminarColumn);

            // Evento para manejar clicks en botones de acción
            dgvClientes.CellContentClick += DgvClientes_CellContentClick;
            
            // Configurar estilo de botones cuando se pinta la celda
            dgvClientes.CellPainting += DgvClientes_CellPainting;
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
                btnActualizar.Text = "Cargando...";

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
                btnActualizar.Text = "Actualizar";
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
                btnBuscar.Text = "Buscando...";

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
                btnBuscar.Text = "Buscar";
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
                var clienteId = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["Id"].Value);

                switch (columnName)
                {
                    case "BtnEditar":
                        EditarCliente?.Invoke(clienteId);
                        break;

                    case "BtnEliminar":
                        EliminarCliente?.Invoke(clienteId);
                        break;
                }
            }
        }

        private void DgvClientes_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            // Solo aplicar estilo personalizado a las columnas de botones
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var columnName = dgvClientes.Columns[e.ColumnIndex].Name;
                
                if (columnName == "BtnEditar" || columnName == "BtnEliminar")
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);
                    
                    // Determinar color del botón
                    Color backColor = columnName == "BtnEditar" ? 
                        Color.FromArgb(52, 152, 219) : Color.FromArgb(231, 76, 60);
                    
                    // Crear un rectángulo con margen para el botón
                    var buttonRect = new Rectangle(
                        e.CellBounds.X + 2,
                        e.CellBounds.Y + 2,
                        e.CellBounds.Width - 4,
                        e.CellBounds.Height - 4
                    );

                    // Pintar el fondo del botón con bordes redondeados
                    using (var brush = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(brush, buttonRect);
                    }

                    // Dibujar el texto del botón
                    string buttonText = columnName == "BtnEditar" ? "✏️ Editar" : "🗑️ Eliminar";
                    using (var textBrush = new SolidBrush(Color.White))
                    {
                        var stringFormat = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        
                        e.Graphics.DrawString(buttonText, 
                            new Font("Segoe UI", 8F, FontStyle.Bold), 
                            textBrush, buttonRect, stringFormat);
                    }

                    e.Handled = true;
                }
            }
        }

        private void DgvClientes_CellMouseEnter(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvClientes.Columns[e.ColumnIndex].Name;
                if (columnName == "BtnEditar" || columnName == "BtnEliminar")
                {
                    dgvClientes.Cursor = Cursors.Hand;
                }
            }
        }

        private void DgvClientes_CellMouseLeave(object? sender, DataGridViewCellEventArgs e)
        {
            dgvClientes.Cursor = Cursors.Default;
        }

        #endregion

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