using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.ModelLayer;

namespace Veterinaria.App.Views.Producto
{
    /// <summary>
    /// Vista de formulario para crear/editar productos
    /// Maneja tanto creación como edición según el modo configurado
    /// </summary>
    public partial class ProductoFormView : UserControl
    {
        // Eventos para comunicación con el Dashboard
        public event Action? ProductoGuardado;
        public event Action? CancelarOperacion;

        private ModelLayer.Producto? _productoActual;
        private bool _modoEdicion = false;
        private bool _guardando = false;

        public ProductoFormView()
        {
            InitializeComponent();
            ConfigurarEventos();
            ConfigurarValidacion();
        }

        /// <summary>
        /// Configura la vista para crear un nuevo producto
        /// </summary>
        public void ConfigurarParaNuevo()
        {
            _modoEdicion = false;
            _productoActual = null;
            lblTitle.Text = "Nuevo Producto";
            LimpiarFormulario();
            txtCodigo.Focus();
        }

        /// <summary>
        /// Configura la vista para editar un producto existente
        /// </summary>
        public void ConfigurarParaEdicion(ModelLayer.Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            _modoEdicion = true;
            _productoActual = producto;
            lblTitle.Text = $"Editando Producto - {producto.Nombre}";
            CargarDatosProducto(producto);
            txtCodigo.Focus();
        }

        /// <summary>
        /// Configura los eventos de los controles
        /// </summary>
        private void ConfigurarEventos()
        {
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            // Validación en tiempo real
            txtCodigo.TextChanged += ValidarFormulario;
            txtNombre.TextChanged += ValidarFormulario;
            numPrecio.ValueChanged += ValidarFormulario;
            numStock.ValueChanged += ValidarFormulario;
            numStockMinimo.ValueChanged += ValidarFormulario;

            // Eventos de teclado
            this.KeyDown += ProductoFormView_KeyDown;
            txtCodigo.KeyDown += Campo_KeyDown;
            txtNombre.KeyDown += Campo_KeyDown;
            txtDescripcion.KeyDown += Campo_KeyDown;
            txtCategoria.KeyDown += Campo_KeyDown;
            txtProveedor.KeyDown += Campo_KeyDown;
        }

        /// <summary>
        /// Configura la validación visual de los campos
        /// </summary>
        private void ConfigurarValidacion()
        {
            // Configurar placeholder text para campos opcionales
            txtDescripcion.PlaceholderText = "Descripción del producto (opcional)";
            txtCategoria.PlaceholderText = "Categoría del producto (opcional)";
            txtProveedor.PlaceholderText = "Proveedor del producto (opcional)";

            // Configurar controles numéricos
            numPrecio.Minimum = 0;
            numPrecio.Maximum = 999999;
            numPrecio.DecimalPlaces = 2;
            numPrecio.Value = 0;

            numStock.Minimum = 0;
            numStock.Maximum = 999999;
            numStock.DecimalPlaces = 0;
            numStock.Value = 0;

            numStockMinimo.Minimum = 0;
            numStockMinimo.Maximum = 999999;
            numStockMinimo.DecimalPlaces = 0;
            numStockMinimo.Value = 0;

            // Validar inicialmente
            ValidarFormulario(null, EventArgs.Empty);
        }

        /// <summary>
        /// Carga los datos de un producto existente en el formulario
        /// </summary>
        private void CargarDatosProducto(ModelLayer.Producto producto)
        {
            try
            {
                txtCodigo.Text = producto.Codigo;
                txtNombre.Text = producto.Nombre;
                txtDescripcion.Text = producto.Descripcion ?? "";
                txtCategoria.Text = producto.Categoria ?? "";
                txtProveedor.Text = producto.Proveedor ?? "";
                
                numPrecio.Value = producto.Precio;
                numStock.Value = producto.Stock;
                numStockMinimo.Value = producto.StockMinimo;
                
                chkActivo.Checked = producto.Activo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del producto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtCategoria.Clear();
            txtProveedor.Clear();
            
            numPrecio.Value = 0;
            numStock.Value = 0;
            numStockMinimo.Value = 0;
            
            chkActivo.Checked = true;

            LimpiarEstilosValidacion();
        }

        /// <summary>
        /// Valida el formulario completo
        /// </summary>
        private void ValidarFormulario(object? sender, EventArgs e)
        {
            if (_guardando) return;

            bool esValido = true;

            // Validar código
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MarcarCampoInvalido(txtCodigo);
                esValido = false;
            }
            else
            {
                MarcarCampoValido(txtCodigo);
            }

            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MarcarCampoInvalido(txtNombre);
                esValido = false;
            }
            else
            {
                MarcarCampoValido(txtNombre);
            }

            // Validar precio
            if (numPrecio.Value <= 0)
            {
                MarcarCampoInvalido(numPrecio);
                esValido = false;
            }
            else
            {
                MarcarCampoValido(numPrecio);
            }

            // Los demás campos numéricos pueden ser 0, así que no necesitan validación adicional

            btnGuardar.Enabled = esValido;
        }

        /// <summary>
        /// Marca un campo como inválido visualmente
        /// </summary>
        private void MarcarCampoInvalido(Control campo)
        {
            campo.BackColor = Color.FromArgb(255, 235, 238);
        }

        /// <summary>
        /// Marca un campo como válido visualmente
        /// </summary>
        private void MarcarCampoValido(Control campo)
        {
            campo.BackColor = Color.White;
        }

        /// <summary>
        /// Limpia los estilos de validación de todos los campos
        /// </summary>
        private void LimpiarEstilosValidacion()
        {
            txtCodigo.BackColor = Color.White;
            txtNombre.BackColor = Color.White;
            txtDescripcion.BackColor = Color.White;
            txtCategoria.BackColor = Color.White;
            txtProveedor.BackColor = Color.White;
            numPrecio.BackColor = Color.White;
            numStock.BackColor = Color.White;
            numStockMinimo.BackColor = Color.White;
        }

        /// <summary>
        /// Guarda el producto (crear o actualizar)
        /// </summary>
        private void GuardarProducto()
        {
            try
            {
                _guardando = true;
                btnGuardar.Enabled = false;
                btnCancelar.Enabled = false;

                // Obtener datos del formulario
                var codigo = txtCodigo.Text.Trim();
                var nombre = txtNombre.Text.Trim();
                var descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim();
                var categoria = string.IsNullOrWhiteSpace(txtCategoria.Text) ? null : txtCategoria.Text.Trim();
                var proveedor = string.IsNullOrWhiteSpace(txtProveedor.Text) ? null : txtProveedor.Text.Trim();
                
                var precio = numPrecio.Value;
                var stock = (int)numStock.Value;
                var stockMinimo = (int)numStockMinimo.Value;

                // Guardar según el modo
                (bool success, string message, ModelLayer.Producto? producto) resultado;

                if (_modoEdicion && _productoActual != null)
                {
                    resultado = ProductoController.Update(
                        _productoActual.Id,
                        codigo,
                        nombre,
                        descripcion,
                        precio,
                        stock,
                        stockMinimo,
                        categoria,
                        proveedor);
                }
                else
                {
                    resultado = ProductoController.Create(
                        codigo,
                        nombre,
                        descripcion,
                        precio,
                        stock,
                        stockMinimo,
                        categoria,
                        proveedor);
                }

                if (resultado.success)
                {
                    MessageBox.Show(resultado.message, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ProductoGuardado?.Invoke();
                }
                else
                {
                    MessageBox.Show(resultado.message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al guardar producto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _guardando = false;
                btnGuardar.Enabled = true;
                btnCancelar.Enabled = true;
            }
        }

        #region Eventos de Botones
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarFormularioCompleto())
            {
                GuardarProducto();
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "¿Está seguro de que desea cancelar? Se perderán los cambios no guardados.",
                "Confirmar cancelación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                CancelarOperacion?.Invoke();
            }
        }
        #endregion

        #region Eventos de Teclado
        private void ProductoFormView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                BtnCancelar_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (btnGuardar.Enabled)
                {
                    BtnGuardar_Click(sender, e);
                }
                e.Handled = true;
            }
        }

        private void Campo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                e.Handled = true;
            }
        }
        #endregion

        /// <summary>
        /// Validación completa antes de guardar
        /// </summary>
        private bool ValidarFormularioCompleto()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                errores.Add("El código es requerido");

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                errores.Add("El nombre es requerido");

            if (numPrecio.Value <= 0)
                errores.Add("El precio debe ser mayor que cero");

            if (numStock.Value < 0)
                errores.Add("El stock no puede ser negativo");

            if (numStockMinimo.Value < 0)
                errores.Add("El stock mínimo no puede ser negativo");

            if (errores.Any())
            {
                MessageBox.Show(
                    "Por favor corrija los siguientes errores:\n\n" + string.Join("\n", errores),
                    "Errores de validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}