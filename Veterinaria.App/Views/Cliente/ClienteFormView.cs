using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.ModelLayer;

namespace Veterinaria.App.Views.Cliente
{
    /// <summary>
    /// Vista de formulario para crear/editar clientes
    /// Maneja tanto creación como edición según el modo configurado
    /// </summary>
    public partial class ClienteFormView : UserControl
    {
        // Eventos para comunicación con el Dashboard
        public event Action? ClienteGuardado;
        public event Action? CancelarOperacion;

        private ModelLayer.Cliente? _clienteActual;
        private bool _modoEdicion = false;
        private bool _guardando = false;

        public ClienteFormView()
        {
            InitializeComponent();
            ConfigurarEventos();
            ConfigurarValidacion();
        }

        /// <summary>
        /// Configura la vista para crear un nuevo cliente
        /// </summary>
        public void ConfigurarParaNuevo()
        {
            _modoEdicion = false;
            _clienteActual = null;
            lblTitle.Text = "Nuevo Cliente";
            LimpiarFormulario();
            txtNombre.Focus();
        }

        /// <summary>
        /// Configura la vista para editar un cliente existente
        /// </summary>
        public void ConfigurarParaEdicion(ModelLayer.Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            _modoEdicion = true;
            _clienteActual = cliente;
            lblTitle.Text = $"Editando Cliente - {cliente.NombreCompleto()}";
            CargarDatosCliente(cliente);
            txtNombre.Focus();
        }

        /// <summary>
        /// Configura los eventos de los controles
        /// </summary>
        private void ConfigurarEventos()
        {
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            // Validación en tiempo real
            txtNombre.TextChanged += ValidarFormulario;
            txtApellido.TextChanged += ValidarFormulario;
            txtEmail.TextChanged += ValidarFormulario;
            txtEmail.Leave += TxtEmail_Leave;

            // Eventos de teclado
            this.KeyDown += ClienteFormView_KeyDown;
            txtNombre.KeyDown += Campo_KeyDown;
            txtApellido.KeyDown += Campo_KeyDown;
            txtTelefono.KeyDown += Campo_KeyDown;
            txtEmail.KeyDown += Campo_KeyDown;
        }

        /// <summary>
        /// Configura la validación visual de los campos
        /// </summary>
        private void ConfigurarValidacion()
        {
            // Configurar placeholder text para campos opcionales
            txtTelefono.PlaceholderText = "Ej: +56 9 1234 5678";
            txtEmail.PlaceholderText = "cliente@email.com";
            txtDireccion.PlaceholderText = "Dirección completa del cliente...";

            // Validar inicialmente
            ValidarFormulario(null, EventArgs.Empty);
        }

        /// <summary>
        /// Carga los datos del cliente en el formulario
        /// </summary>
        private void CargarDatosCliente(ModelLayer.Cliente cliente)
        {
            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtTelefono.Text = cliente.Telefono ?? "";
            txtEmail.Text = cliente.Email ?? "";
            txtDireccion.Text = cliente.Direccion ?? "";
            chkActivo.Checked = cliente.Activo;
        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            chkActivo.Checked = true;

            // Limpiar estilos de error
            LimpiarErrores();
        }

        /// <summary>
        /// Valida el formulario y habilita/deshabilita el botón guardar
        /// </summary>
        private void ValidarFormulario(object? sender, EventArgs e)
        {
            bool formularioValido = true;
            
            // Limpiar errores previos
            LimpiarErrores();

            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MarcarCampoError(txtNombre);
                formularioValido = false;
            }

            // Validar apellido
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MarcarCampoError(txtApellido);
                formularioValido = false;
            }

            // Validar email (si se proporciona)
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !EsEmailValido(txtEmail.Text))
            {
                MarcarCampoError(txtEmail);
                formularioValido = false;
            }

            btnGuardar.Enabled = formularioValido && !_guardando;
        }

        /// <summary>
        /// Marca un campo con error visual
        /// </summary>
        private void MarcarCampoError(TextBox campo)
        {
            campo.BackColor = Color.FromArgb(255, 235, 235);
            campo.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Limpia todos los errores visuales
        /// </summary>
        private void LimpiarErrores()
        {
            txtNombre.BackColor = Color.White;
            txtApellido.BackColor = Color.White;
            txtTelefono.BackColor = Color.White;
            txtEmail.BackColor = Color.White;
        }

        /// <summary>
        /// Valida si un email tiene formato correcto
        /// </summary>
        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #region Event Handlers

        private async void BtnGuardar_Click(object? sender, EventArgs e)
        {
            await GuardarCliente();
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            if (HaycambiosPendientes())
            {
                var result = MessageBox.Show(
                    "¿Está seguro que desea cancelar? Los cambios no guardados se perderán.",
                    "Confirmar Cancelación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            CancelarOperacion?.Invoke();
        }

        private void ClienteFormView_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                BtnCancelar_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                e.Handled = true;
                if (btnGuardar.Enabled)
                {
                    BtnGuardar_Click(sender, e);
                }
            }
        }

        private void Campo_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Mover al siguiente campo o guardar si es el último
                if (sender == txtNombre)
                    txtApellido.Focus();
                else if (sender == txtApellido)
                    txtTelefono.Focus();
                else if (sender == txtTelefono)
                    txtEmail.Focus();
                else if (sender == txtEmail)
                    txtDireccion.Focus();
                else if (btnGuardar.Enabled)
                    BtnGuardar_Click(sender, e);
            }
        }

        private void TxtEmail_Leave(object? sender, EventArgs e)
        {
            // Convertir email a minúsculas automáticamente
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = txtEmail.Text.Trim().ToLower();
            }
        }

        #endregion

        /// <summary>
        /// Guarda el cliente (crear nuevo o actualizar existente)
        /// </summary>
        private async Task GuardarCliente()
        {
            if (_guardando) return;

            try
            {
                _guardando = true;
                btnGuardar.Enabled = false;
                btnGuardar.Text = "Guardando...";

                var nombre = txtNombre.Text.Trim();
                var apellido = txtApellido.Text.Trim();
                var telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();
                var email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                var direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim();

                (bool success, string message, ModelLayer.Cliente? cliente) resultado;

                if (_modoEdicion && _clienteActual != null)
                {
                    // Actualizar cliente existente
                    resultado = await Task.Run(() =>
                        ClienteController.Update(_clienteActual.Id, nombre, apellido, telefono, email, direccion));
                }
                else
                {
                    // Crear nuevo cliente
                    resultado = await Task.Run(() =>
                        ClienteController.Create(nombre, apellido, telefono, email, direccion));
                }

                if (resultado.success)
                {
                    MessageBox.Show(resultado.message, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClienteGuardado?.Invoke();
                }
                else
                {
                    MessageBox.Show(resultado.message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _guardando = false;
                btnGuardar.Enabled = true;
                btnGuardar.Text = "Guardar";
                ValidarFormulario(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Verifica si hay cambios pendientes en el formulario
        /// </summary>
        private bool HaycambiosPendientes()
        {
            if (_modoEdicion && _clienteActual != null)
            {
                return txtNombre.Text.Trim() != _clienteActual.Nombre ||
                       txtApellido.Text.Trim() != _clienteActual.Apellido ||
                       txtTelefono.Text.Trim() != (_clienteActual.Telefono ?? "") ||
                       txtEmail.Text.Trim().ToLower() != (_clienteActual.Email ?? "").ToLower() ||
                       txtDireccion.Text.Trim() != (_clienteActual.Direccion ?? "") ||
                       chkActivo.Checked != _clienteActual.Activo;
            }
            else
            {
                return !string.IsNullOrWhiteSpace(txtNombre.Text) ||
                       !string.IsNullOrWhiteSpace(txtApellido.Text) ||
                       !string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                       !string.IsNullOrWhiteSpace(txtEmail.Text) ||
                       !string.IsNullOrWhiteSpace(txtDireccion.Text);
            }
        }

        /// <summary>
        /// Obtiene el cliente actual (para edición)
        /// </summary>
        public ModelLayer.Cliente? GetClienteActual()
        {
            return _clienteActual;
        }

        /// <summary>
        /// Indica si está en modo edición
        /// </summary>
        public bool EsModoEdicion => _modoEdicion;
    }
}