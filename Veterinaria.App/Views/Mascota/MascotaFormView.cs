using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Veterinaria.BusinessLayer.Controllers;
using Veterinaria.ModelLayer;

namespace Veterinaria.App.Views.Mascota
{
    /// <summary>
    /// Vista de formulario para crear/editar mascotas
    /// Maneja tanto creación como edición según el modo configurado
    /// </summary>
    public partial class MascotaFormView : UserControl
    {
        // Eventos para comunicación con el Dashboard
        public event Action? MascotaGuardada;
        public event Action? CancelarOperacion;

        private ModelLayer.Mascota? _mascotaActual;
        private bool _modoEdicion = false;
        private bool _guardando = false;

        public MascotaFormView()
        {
            InitializeComponent();
            ConfigurarEventos();
            ConfigurarValidacion();
            CargarClientes();
        }

        /// <summary>
        /// Configura la vista para crear una nueva mascota
        /// </summary>
        public void ConfigurarParaNuevo()
        {
            _modoEdicion = false;
            _mascotaActual = null;
            lblTitle.Text = "Nueva Mascota";
            LimpiarFormulario();
            cboCliente.Focus();
        }

        /// <summary>
        /// Configura la vista para editar una mascota existente
        /// </summary>
        public void ConfigurarParaEdicion(ModelLayer.Mascota mascota)
        {
            if (mascota == null)
                throw new ArgumentNullException(nameof(mascota));

            _modoEdicion = true;
            _mascotaActual = mascota;
            lblTitle.Text = $"Editando Mascota - {mascota.Nombre}";
            CargarDatosMascota(mascota);
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
            txtEspecie.TextChanged += ValidarFormulario;
            cboCliente.SelectedIndexChanged += ValidarFormulario;
            numEdad.ValueChanged += ValidarFormulario;
            numPeso.ValueChanged += ValidarFormulario;

            // Eventos de teclado
            this.KeyDown += MascotaFormView_KeyDown;
            txtNombre.KeyDown += Campo_KeyDown;
            txtEspecie.KeyDown += Campo_KeyDown;
            txtRaza.KeyDown += Campo_KeyDown;
            txtColor.KeyDown += Campo_KeyDown;
        }

        /// <summary>
        /// Configura la validación visual de los campos
        /// </summary>
        private void ConfigurarValidacion()
        {
            // Configurar placeholder text para campos opcionales
            txtRaza.PlaceholderText = "Raza de la mascota (opcional)";
            txtColor.PlaceholderText = "Color de la mascota (opcional)";
            txtObservaciones.PlaceholderText = "Observaciones adicionales sobre la mascota...";

            // Configurar controles numéricos
            numEdad.Minimum = 0;
            numEdad.Maximum = 30; // Edad máxima realista para mascotas
            numEdad.DecimalPlaces = 0;
            numEdad.Value = 0;

            numPeso.Minimum = 0;
            numPeso.Maximum = 999;
            numPeso.DecimalPlaces = 2;
            numPeso.Value = 0;

            // Validar inicialmente
            ValidarFormulario(null, EventArgs.Empty);
        }

        /// <summary>
        /// Carga los clientes disponibles en el ComboBox
        /// </summary>
        private void CargarClientes()
        {
            try
            {
                cboCliente.Items.Clear();
                cboCliente.Items.Add("Seleccionar cliente...");

                var clientes = ClienteController.GetAll();
                foreach (var cliente in clientes.OrderBy(c => c.NombreCompleto()))
                {
                    var item = new ComboBoxItem
                    {
                        Text = $"{cliente.NombreCompleto()} - {cliente.Email ?? "Sin email"}",
                        Value = cliente.Id
                    };
                    cboCliente.Items.Add(item);
                }

                cboCliente.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Carga los datos de una mascota existente en el formulario
        /// </summary>
        private void CargarDatosMascota(ModelLayer.Mascota mascota)
        {
            try
            {
                txtNombre.Text = mascota.Nombre;
                txtEspecie.Text = mascota.Especie;
                txtRaza.Text = mascota.Raza ?? "";
                txtColor.Text = mascota.Color ?? "";
                txtObservaciones.Text = mascota.Observaciones ?? "";
                
                numEdad.Value = mascota.Edad ?? 0;
                numPeso.Value = mascota.Peso ?? 0;
                
                chkActivo.Checked = mascota.Activo;

                // Seleccionar cliente
                for (int i = 1; i < cboCliente.Items.Count; i++)
                {
                    if (cboCliente.Items[i] is ComboBoxItem item && item.Value == mascota.ClienteId)
                    {
                        cboCliente.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de la mascota: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtEspecie.Clear();
            txtRaza.Clear();
            txtColor.Clear();
            txtObservaciones.Clear();
            
            numEdad.Value = 0;
            numPeso.Value = 0;
            
            cboCliente.SelectedIndex = 0;
            chkActivo.Checked = true;

            // Limpiar estilos de validación
            LimpiarEstilosValidacion();
        }

        /// <summary>
        /// Valida el formulario completo
        /// </summary>
        private void ValidarFormulario(object? sender, EventArgs e)
        {
            if (_guardando) return;

            bool esValido = true;

            // Validar cliente
            if (cboCliente.SelectedIndex <= 0)
            {
                MarcarCampoInvalido(cboCliente);
                esValido = false;
            }
            else
            {
                MarcarCampoValido(cboCliente);
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

            // Validar especie
            if (string.IsNullOrWhiteSpace(txtEspecie.Text))
            {
                MarcarCampoInvalido(txtEspecie);
                esValido = false;
            }
            else
            {
                MarcarCampoValido(txtEspecie);
            }

            // Los campos numéricos se validan automáticamente por los controles NumericUpDown

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
            txtNombre.BackColor = Color.White;
            txtEspecie.BackColor = Color.White;
            txtRaza.BackColor = Color.White;
            txtColor.BackColor = Color.White;
            txtObservaciones.BackColor = Color.White;
            cboCliente.BackColor = Color.White;
        }

        /// <summary>
        /// Guarda la mascota (crear o actualizar)
        /// </summary>
        private void GuardarMascota()
        {
            try
            {
                _guardando = true;
                btnGuardar.Enabled = false;
                btnCancelar.Enabled = false;

                // Obtener datos del formulario
                var clienteSeleccionado = (ComboBoxItem)cboCliente.SelectedItem;
                var clienteId = clienteSeleccionado.Value;
                var nombre = txtNombre.Text.Trim();
                var especie = txtEspecie.Text.Trim();
                var raza = string.IsNullOrWhiteSpace(txtRaza.Text) ? null : txtRaza.Text.Trim();
                var color = string.IsNullOrWhiteSpace(txtColor.Text) ? null : txtColor.Text.Trim();
                var observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();
                
                int? edad = numEdad.Value > 0 ? (int)numEdad.Value : null;
                decimal? peso = numPeso.Value > 0 ? numPeso.Value : null;

                // Guardar según el modo
                (bool success, string message, ModelLayer.Mascota? mascota) resultado;

                if (_modoEdicion && _mascotaActual != null)
                {
                    resultado = MascotaController.Update(
                        _mascotaActual.Id,
                        clienteId,
                        nombre,
                        especie,
                        raza,
                        edad,
                        peso,
                        color,
                        observaciones);
                }
                else
                {
                    resultado = MascotaController.Create(
                        clienteId,
                        nombre,
                        especie,
                        raza,
                        edad,
                        peso,
                        color,
                        observaciones);
                }

                if (resultado.success)
                {
                    MessageBox.Show(resultado.message, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MascotaGuardada?.Invoke();
                }
                else
                {
                    MessageBox.Show(resultado.message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al guardar mascota: {ex.Message}", "Error",
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
                GuardarMascota();
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "¿Estás seguro de que desea cancelar? Se perderán los cambios no guardados.",
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
        private void MascotaFormView_KeyDown(object sender, KeyEventArgs e)
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

            if (cboCliente.SelectedIndex <= 0)
                errores.Add("Debe seleccionar un cliente");

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                errores.Add("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtEspecie.Text))
                errores.Add("La especie es requerida");

            if (numEdad.Value < 0)
                errores.Add("La edad no puede ser negativa");

            if (numPeso.Value < 0)
                errores.Add("El peso no puede ser negativo");

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

        /// <summary>
        /// Clase auxiliar para items del ComboBox
        /// </summary>
        private class ComboBoxItem
        {
            public string Text { get; set; } = "";
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}