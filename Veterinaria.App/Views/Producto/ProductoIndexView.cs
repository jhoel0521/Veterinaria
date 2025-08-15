using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.ModelLayer;

namespace Veterinaria.App.Views.Producto
{
    /// <summary>
    /// Vista principal (Index) para gestión de productos
    /// Muestra lista de productos con opciones de búsqueda, crear, editar y eliminar
    /// Incluye filtros por nombre, código, categoría y estado de stock
    /// </summary>
    public partial class ProductoIndexView : UserControl
    {
        // Eventos para comunicación con el Dashboard
        public event Action<int>? EditarProducto;
        public event Action? NuevoProducto;
        public event Action<int>? VerProducto;
        public event Action<int>? EliminarProducto;

        private List<ModelLayer.Producto> _productos = new List<ModelLayer.Producto>();
        private bool _isLoading = false;

        public ProductoIndexView()
        {
            InitializeComponent();
            ConfigurarEventos();
            ConfigurarDataGridView();
            ConfigurarFiltros();
            CargarProductos();
        }

        /// <summary>
        /// Configura los eventos de los controles
        /// </summary>
        private void ConfigurarEventos()
        {
            // Eventos de botones
            btnNuevoProducto.Click += BtnNuevoProducto_Click;
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
                    BuscarProductos();
                }
                else if (cboCategoria.SelectedIndex <= 0 && cboEstadoStock.SelectedIndex <= 0)
                {
                    CargarProductos(); // Si está vacío y sin filtros, recargar todos
                }
            };

            // Eventos de los ComboBox de filtros
            cboCategoria.SelectedIndexChanged += (sender, e) => AplicarFiltros();
            cboEstadoStock.SelectedIndexChanged += (sender, e) => AplicarFiltros();

            // Eventos del DataGridView
            dgvProductos.CellDoubleClick += DgvProductos_CellDoubleClick;
            dgvProductos.CellFormatting += DgvProductos_CellFormatting;

            // Agregar evento MouseEnter para efecto hover en botones
            dgvProductos.CellMouseEnter += DgvProductos_CellMouseEnter;
            dgvProductos.CellMouseLeave += DgvProductos_CellMouseLeave;
            dgvProductos.CellClick += DgvProductos_CellClick;
        }

        /// <summary>
        /// Configura las columnas del DataGridView
        /// </summary>
        private void ConfigurarDataGridView()
        {
            dgvProductos.AutoGenerateColumns = false;

            // Columna ID (oculta)
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Visible = false
            });

            // Columna Código
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Codigo",
                HeaderText = "Código",
                DataPropertyName = "Codigo",
                Width = 100,
                ReadOnly = true
            });

            // Columna Nombre
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 200,
                ReadOnly = true
            });

            // Columna Categoría
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Categoria",
                HeaderText = "Categoría",
                DataPropertyName = "Categoria",
                Width = 120,
                ReadOnly = true
            });

            // Columna Precio
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Precio",
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                Width = 80,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "C2"
                }
            });

            // Columna Stock
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Stock",
                HeaderText = "Stock",
                DataPropertyName = "Stock",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Columna Stock Mínimo
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockMinimo",
                HeaderText = "Stock Mín.",
                DataPropertyName = "StockMinimo",
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Columna Estado Stock
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EstadoStock",
                HeaderText = "Estado Stock",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Columna Estado
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
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
            dgvProductos.Columns.Add(btnEditarColumn);

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
            dgvProductos.Columns.Add(btnEliminarColumn);
        }

        /// <summary>
        /// Configura los filtros de búsqueda
        /// </summary>
        private void ConfigurarFiltros()
        {
            // Configurar ComboBox de categorías
            cboCategoria.Items.Add("Todas las categorías");
            cboCategoria.SelectedIndex = 0;

            // Configurar ComboBox de estado de stock
            cboEstadoStock.Items.Add("Todos los estados");
            cboEstadoStock.Items.Add("Stock normal");
            cboEstadoStock.Items.Add("Stock bajo");
            cboEstadoStock.Items.Add("Sin stock");
            cboEstadoStock.SelectedIndex = 0;

            // Cargar datos para los filtros
            CargarCategorias();
        }

        /// <summary>
        /// Carga las categorias disponibles en el ComboBox
        /// </summary>
        private void CargarCategorias()
        {
            try
            {
                var categorias = ProductoController.GetCategorias();
                foreach (var categoria in categorias)
                {
                    cboCategoria.Items.Add(categoria);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Carga todos los productos en el DataGridView
        /// </summary>
        private void CargarProductos()
        {
            try
            {
                _isLoading = true;
                dgvProductos.DataSource = null;

                _productos = ProductoController.GetAll();
                MostrarProductos(_productos);

                ActualizarContadorResultados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Busca productos según el texto ingresado
        /// </summary>
        private void BuscarProductos()
        {
            try
            {
                if (_isLoading) return;

                _isLoading = true;
                var searchText = txtBuscar.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    _productos = ProductoController.GetAll();
                }
                else
                {
                    _productos = ProductoController.Search(searchText);
                }

                // Aplicar filtros adicionales
                AplicarFiltrosAdicionales();
                MostrarProductos(_productos);
                ActualizarContadorResultados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Aplica los filtros de categoría y estado de stock
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
                    _productos = ProductoController.Search(txtBuscar.Text.Trim());
                }
                else
                {
                    _productos = ProductoController.GetAll();
                }

                AplicarFiltrosAdicionales();
                MostrarProductos(_productos);
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
        /// Aplica filtros adicionales de categoría y estado de stock
        /// </summary>
        private void AplicarFiltrosAdicionales()
        {
            // Filtrar por categoría
            if (cboCategoria.SelectedIndex > 0)
            {
                var categoriaSeleccionada = cboCategoria.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(categoriaSeleccionada))
                {
                    _productos = _productos.Where(p =>
                        p.Categoria?.Equals(categoriaSeleccionada, StringComparison.OrdinalIgnoreCase) == true)
                        .ToList();
                }
            }

            // Filtrar por estado de stock
            if (cboEstadoStock.SelectedIndex > 0)
            {
                var estadoSeleccionado = cboEstadoStock.SelectedItem?.ToString();
                switch (estadoSeleccionado)
                {
                    case "Stock normal":
                        _productos = _productos.Where(p => p.Stock > p.StockMinimo).ToList();
                        break;
                    case "Stock bajo":
                        _productos = _productos.Where(p => p.Stock <= p.StockMinimo && p.Stock > 0).ToList();
                        break;
                    case "Sin stock":
                        _productos = _productos.Where(p => p.Stock == 0).ToList();
                        break;
                }
            }
        }

        /// <summary>
        /// Muestra los productos en el DataGridView
        /// </summary>
        private void MostrarProductos(List<ModelLayer.Producto> productos)
        {
            try
            {
                dgvProductos.Rows.Clear();

                foreach (var producto in productos)
                {
                    string estadoStock;
                    if (producto.Stock == 0)
                        estadoStock = "Sin stock";
                    else if (producto.Stock <= producto.StockMinimo)
                        estadoStock = "Stock bajo";
                    else
                        estadoStock = "Normal";

                    var estado = producto.Activo ? "Activo" : "Inactivo";

                    dgvProductos.Rows.Add(
                        producto.Id,
                        producto.Codigo,
                        producto.Nombre,
                        producto.Categoria ?? "Sin categoría",
                        producto.Precio,
                        producto.Stock,
                        producto.StockMinimo,
                        estadoStock,
                        estado,
                        "Editar",
                        "Eliminar"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar productos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza el contador de resultados
        /// </summary>
        private void ActualizarContadorResultados()
        {
            var count = dgvProductos.Rows.Count;
            lblResultados.Text = $"Mostrando {count} producto{(count != 1 ? "s" : "")}";
        }

        /// <summary>
        /// Limpia todos los filtros y recarga los productos
        /// </summary>
        private void LimpiarFiltros()
        {
            txtBuscar.Clear();
            cboCategoria.SelectedIndex = 0;
            cboEstadoStock.SelectedIndex = 0;
            CargarProductos();
        }

        #region Eventos de Botones
        private void BtnNuevoProducto_Click(object sender, EventArgs e)
        {
            NuevoProducto?.Invoke();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            BuscarProductos();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFiltros();
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }
        #endregion

        #region Eventos del DataGridView
        private void DgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var productoId = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["Id"].Value);
                VerProducto?.Invoke(productoId);
            }
        }

        private void DgvProductos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.CellStyle == null) return;

            var columnName = dgvProductos.Columns[e.ColumnIndex].Name;

            if (columnName == "Estado")
            {
                if (e.Value?.ToString() == "Inactivo")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                    e.CellStyle.Font = new Font(dgvProductos.Font, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(46, 204, 113);
                    e.CellStyle.Font = new Font(dgvProductos.Font, FontStyle.Bold);
                }
            }
            else if (columnName == "EstadoStock")
            {
                var estadoStock = e.Value?.ToString();
                switch (estadoStock)
                {
                    case "Sin stock":
                        e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                        e.CellStyle.Font = new Font(dgvProductos.Font, FontStyle.Bold);
                        break;
                    case "Stock bajo":
                        e.CellStyle.ForeColor = Color.FromArgb(230, 126, 34);
                        e.CellStyle.Font = new Font(dgvProductos.Font, FontStyle.Bold);
                        break;
                    case "Normal":
                        e.CellStyle.ForeColor = Color.FromArgb(46, 204, 113);
                        e.CellStyle.Font = new Font(dgvProductos.Font, FontStyle.Bold);
                        break;
                }
            }

            // Manejar clics en botones
            if (e.ColumnIndex >= 0 && dgvProductos.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                var buttonColumnName = dgvProductos.Columns[e.ColumnIndex].Name;
                if (buttonColumnName == "BtnEditar" || buttonColumnName == "BtnEliminar")
                {
                    e.CellStyle.Font = new Font(dgvProductos.Font, FontStyle.Bold);
                }
            }
        }

        private void DgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvProductos.Columns[e.ColumnIndex].Name;
                var productoId = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["Id"].Value);

                switch (columnName)
                {
                    case "BtnEditar":
                        EditarProducto?.Invoke(productoId);
                        break;
                    case "BtnEliminar":
                        var productoNombre = dgvProductos.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                        var result = MessageBox.Show(
                            $"¿Está seguro de que desea eliminar el producto '{productoNombre}'?",
                            "Confirmar eliminación",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            EliminarProducto?.Invoke(productoId);
                            CargarProductos(); // Recargar despues de eliminar
                        }
                        break;
                }
            }
        }

        private void DgvProductos_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvProductos.Columns[e.ColumnIndex].Name;
                if (columnName == "BtnEditar" || columnName == "BtnEliminar")
                {
                    dgvProductos.Cursor = Cursors.Hand;
                }
            }
        }

        private void DgvProductos_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvProductos.Cursor = Cursors.Default;
        }
        #endregion

        #region Eventos de Teclado
        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarProductos();
                e.Handled = true;
            }
        }
        #endregion

        /// <summary>
        /// Método público para refrescar la vista desde el dashboard
        /// </summary>
        public void RefrescarVista()
        {
            CargarProductos();
        }
    }
}