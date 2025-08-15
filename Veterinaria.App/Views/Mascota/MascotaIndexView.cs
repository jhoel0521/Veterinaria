using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.ModelLayer;

namespace Veterinaria.App.Views.Mascota
{
    /// <summary>
    /// Vista principal (Index) para gestión de mascotas
    /// Muestra lista de mascotas con opciones de búsqueda, crear, editar y eliminar
    /// Incluye filtros por nombre, especie, raza y dueño
    /// </summary>
    public partial class MascotaIndexView : UserControl
    {
        // Eventos para comunicación con el Dashboard
        public event Action<int>? EditarMascota;
        public event Action? NuevaMascota;
        public event Action<int>? VerMascota;
        public event Action<int>? EliminarMascota;

        private List<ModelLayer.Mascota> _mascotas = new List<ModelLayer.Mascota>();
        private List<ModelLayer.Cliente> _clientes = new List<ModelLayer.Cliente>();
        private bool _isLoading = false;

        public MascotaIndexView()
        {
            InitializeComponent();
            ConfigurarEventos();
            ConfigurarDataGridView();
            ConfigurarFiltros();
            CargarMascotas();
        }

        /// <summary>
        /// Configura los eventos de los controles
        /// </summary>
        private void ConfigurarEventos()
        {
            // Eventos de botones
            btnNuevaMascota.Click += BtnNuevaMascota_Click;
            btnBuscar.Click += BtnBuscar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnActualizar.Click += BtnActualizar_Click;

            // Eventos del TextBox de búsqueda
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
            txtBuscar.TextChanged += (sender, e) =>
            {
                // Iniciar búsqueda automática al cambiar el texto
                if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    BuscarMascotas();
                }
                else if (cboCliente.SelectedIndex <= 0 && cboEspecie.SelectedIndex <= 0)
                {
                    CargarMascotas(); // Si está vacío y sin filtros, recargar todas
                }
            };

            // Eventos de los ComboBox de filtros
            cboEspecie.SelectedIndexChanged += (sender, e) => AplicarFiltros();
            cboCliente.SelectedIndexChanged += (sender, e) => AplicarFiltros();

            // Eventos del DataGridView
            dgvMascotas.CellDoubleClick += DgvMascotas_CellDoubleClick;
            dgvMascotas.CellFormatting += DgvMascotas_CellFormatting;
            
            // Agregar evento MouseEnter para efecto hover en botones
            dgvMascotas.CellMouseEnter += DgvMascotas_CellMouseEnter;
            dgvMascotas.CellMouseLeave += DgvMascotas_CellMouseLeave;
        }

        /// <summary>
        /// Configura las columnas del DataGridView
        /// </summary>
        private void ConfigurarDataGridView()
        {
            dgvMascotas.AutoGenerateColumns = false;

            // Columna ID (oculta)
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Visible = false
            });

            // Columna Nombre
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 150,
                ReadOnly = true
            });

            // Columna Especie
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Especie",
                HeaderText = "Especie",
                DataPropertyName = "Especie",
                Width = 100,
                ReadOnly = true
            });

            // Columna Raza
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Raza",
                HeaderText = "Raza",
                DataPropertyName = "Raza",
                Width = 120,
                ReadOnly = true
            });

            // Columna Edad
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Edad",
                HeaderText = "Edad",
                DataPropertyName = "Edad",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Columna Dueño
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Dueno",
                HeaderText = "Dueño",
                Width = 200,
                ReadOnly = true
            });

            // Columna Color
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Color",
                HeaderText = "Color",
                DataPropertyName = "Color",
                Width = 100,
                ReadOnly = true
            });

            // Columna Estado
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = "Estado",
                Width = 80,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Botón Editar
            var btnEditarColumn = new DataGridViewButtonColumn
            {
                Name = "BtnEditar",
                HeaderText = "Editar",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                Width = 70,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(41, 128, 185),
                    SelectionForeColor = Color.White
                }
            };
            dgvMascotas.Columns.Add(btnEditarColumn);

            // Botón Eliminar
            var btnEliminarColumn = new DataGridViewButtonColumn
            {
                Name = "BtnEliminar",
                HeaderText = "Eliminar",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(231, 76, 60),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(192, 57, 43),
                    SelectionForeColor = Color.White
                }
            };
            dgvMascotas.Columns.Add(btnEliminarColumn);
        }

        /// <summary>
        /// Configura los filtros de búsqueda
        /// </summary>
        private void ConfigurarFiltros()
        {
            // Configurar ComboBox de especies
            cboEspecie.Items.Add("Todas las especies");
            cboEspecie.SelectedIndex = 0;

            // Configurar ComboBox de clientes
            cboCliente.Items.Add("Todos los dueños");
            cboCliente.SelectedIndex = 0;

            // Cargar datos para los filtros
            CargarEspecies();
            CargarClientes();
        }

        /// <summary>
        /// Carga las especies disponibles en el ComboBox
        /// </summary>
        private void CargarEspecies()
        {
            try
            {
                var especies = MascotaController.GetEspecies();
                foreach (var especie in especies)
                {
                    cboEspecie.Items.Add(especie);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especies: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Carga los clientes disponibles en el ComboBox
        /// </summary>
        private void CargarClientes()
        {
            try
            {
                _clientes = ClienteController.GetAll();
                foreach (var cliente in _clientes.OrderBy(c => c.NombreCompleto()))
                {
                    cboCliente.Items.Add($"{cliente.NombreCompleto()} (ID: {cliente.Id})");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Carga todas las mascotas en el DataGridView
        /// </summary>
        private void CargarMascotas()
        {
            try
            {
                _isLoading = true;
                dgvMascotas.DataSource = null;
                
                _mascotas = MascotaController.GetAll();
                MostrarMascotas(_mascotas);
                
                ActualizarContadorResultados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar mascotas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Busca mascotas según el texto ingresado
        /// </summary>
        private void BuscarMascotas()
        {
            try
            {
                if (_isLoading) return;

                _isLoading = true;
                var searchText = txtBuscar.Text.Trim();
                
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    _mascotas = MascotaController.GetAll();
                }
                else
                {
                    _mascotas = MascotaController.Search(searchText);
                }

                // Aplicar filtros adicionales
                AplicarFiltrosAdicionales();
                MostrarMascotas(_mascotas);
                ActualizarContadorResultados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar mascotas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Aplica los filtros de especie y cliente
        /// </summary>
        private void AplicarFiltros()
        {
            try
            {
                if (_isLoading) return;

                _isLoading = true;
                
                // Comenzar con búsqueda por texto si hay
                if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    _mascotas = MascotaController.Search(txtBuscar.Text.Trim());
                }
                else
                {
                    _mascotas = MascotaController.GetAll();
                }

                AplicarFiltrosAdicionales();
                MostrarMascotas(_mascotas);
                ActualizarContadorResultados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar filtros: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Aplica filtros adicionales de especie y cliente
        /// </summary>
        private void AplicarFiltrosAdicionales()
        {
            // Filtrar por especie
            if (cboEspecie.SelectedIndex > 0)
            {
                var especieSeleccionada = cboEspecie.SelectedItem;
                if (especieSeleccionada == null) return;
                _mascotas = _mascotas.Where(m => m.Especie.Equals(especieSeleccionada.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtrar por cliente
            if (cboCliente.SelectedIndex > 0)
            {
                var clienteSeleccionado = cboCliente.SelectedItem;
                if( clienteSeleccionado == null) return;
                var clienteStr = clienteSeleccionado.ToString();
                var idStr = clienteStr?.Split("ID: ")[1].TrimEnd(')');
                if (int.TryParse(idStr, out int clienteId))
                {
                    _mascotas = _mascotas.Where(m => m.ClienteId == clienteId).ToList();
                }
            }
        }

        /// <summary>
        /// Muestra las mascotas en el DataGridView
        /// </summary>
        private void MostrarMascotas(List<ModelLayer.Mascota> mascotas)
        {
            try
            {
                dgvMascotas.Rows.Clear();

                foreach (var mascota in mascotas)
                {
                    // Debug: verificar que el ClienteId se está cargando correctamente
                    if (mascota.ClienteId == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"ADVERTENCIA: Mascota {mascota.Nombre} tiene ClienteId = 0");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"INFO: Mascota {mascota.Nombre} tiene ClienteId = {mascota.ClienteId}");
                    }

                    var cliente = mascota.GetCliente();
                    var nombreCliente = cliente?.NombreCompleto() ?? $"Cliente no encontrado (ID: {mascota.ClienteId})";
                    
                    var edadTexto = mascota.Edad.HasValue ? 
                        (mascota.EsCachorro() ? $"{mascota.Edad} año(s) - Cachorro" : $"{mascota.Edad} año(s)") :
                        "No especificada";

                    var estado = mascota.Activo ? "Activa" : "Inactiva";

                    dgvMascotas.Rows.Add(
                        mascota.Id,
                        mascota.Nombre,
                        mascota.Especie,
                        mascota.Raza ?? "No especificada",
                        edadTexto,
                        nombreCliente,
                        mascota.Color ?? "No especificado",
                        estado,
                        "Editar",
                        "Eliminar"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar mascotas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza el contador de resultados
        /// </summary>
        private void ActualizarContadorResultados()
        {
            var count = dgvMascotas.Rows.Count;
            lblResultados.Text = $"Mostrando {count} mascota{(count != 1 ? "s" : "")}";
        }

        /// <summary>
        /// Limpia todos los filtros y recarga las mascotas
        /// </summary>
        private void LimpiarFiltros()
        {
            txtBuscar.Clear();
            cboEspecie.SelectedIndex = 0;
            cboCliente.SelectedIndex = 0;
            CargarMascotas();
        }

        #region Eventos de Botones
        private void BtnNuevaMascota_Click(object sender, EventArgs e)
        {
            NuevaMascota?.Invoke();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            BuscarMascotas();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFiltros();
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            CargarMascotas();
        }
        #endregion

        #region Eventos del DataGridView
        private void DgvMascotas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var mascotaId = Convert.ToInt32(dgvMascotas.Rows[e.RowIndex].Cells["Id"].Value);
                VerMascota?.Invoke(mascotaId);
            }
        }

        private void DgvMascotas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.CellStyle == null) return;
            if (dgvMascotas.Columns[e.ColumnIndex].Name == "Estado")
            {
                if (e.Value?.ToString() == "Inactiva")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                    e.CellStyle.Font = new Font(dgvMascotas.Font, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(46, 204, 113);
                    e.CellStyle.Font = new Font(dgvMascotas.Font, FontStyle.Bold);
                }
            }

            // Manejar clics en botones
            if (e.ColumnIndex >= 0 && dgvMascotas.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                var columnName = dgvMascotas.Columns[e.ColumnIndex].Name;
                if (columnName == "BtnEditar" || columnName == "BtnEliminar")
                {
                    e.CellStyle.Font = new Font(dgvMascotas.Font, FontStyle.Bold);
                }
            }
        }

        private void DgvMascotas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvMascotas.Columns[e.ColumnIndex].Name;
                var mascotaId = Convert.ToInt32(dgvMascotas.Rows[e.RowIndex].Cells["Id"].Value);

                switch (columnName)
                {
                    case "BtnEditar":
                        EditarMascota?.Invoke(mascotaId);
                        break;
                    case "BtnEliminar":
                        var mascotaNombre = dgvMascotas.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                        var result = MessageBox.Show(
                            $"¿Está seguro de que desea eliminar a {mascotaNombre}?",
                            "Confirmar eliminación",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        
                        if (result == DialogResult.Yes)
                        {
                            EliminarMascota?.Invoke(mascotaId);
                            CargarMascotas(); // Recargar después de eliminar
                        }
                        break;
                }
            }
        }

        private void DgvMascotas_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvMascotas.Columns[e.ColumnIndex].Name;
                if (columnName == "BtnEditar" || columnName == "BtnEliminar")
                {
                    dgvMascotas.Cursor = Cursors.Hand;
                }
            }
        }

        private void DgvMascotas_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvMascotas.Cursor = Cursors.Default;
        }
        #endregion

        #region Eventos de Teclado
        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarMascotas();
                e.Handled = true;
            }
        }
        #endregion

        /// <summary>
        /// Método público para refrescar la vista desde el dashboard
        /// </summary>
        public void RefrescarVista()
        {
            CargarMascotas();
        }
    }
}