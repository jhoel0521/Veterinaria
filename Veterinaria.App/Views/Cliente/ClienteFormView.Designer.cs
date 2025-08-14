namespace Veterinaria.App.Views.Cliente
{
    partial class ClienteFormView
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se están usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new Panel();
            this.lblTitle = new Label();
            this.panelForm = new Panel();
            this.lblNombre = new Label();
            this.txtNombre = new TextBox();
            this.lblApellido = new Label();
            this.txtApellido = new TextBox();
            this.lblTelefono = new Label();
            this.txtTelefono = new TextBox();
            this.lblEmail = new Label();
            this.txtEmail = new TextBox();
            this.lblDireccion = new Label();
            this.txtDireccion = new TextBox();
            this.chkActivo = new CheckBox();
            this.panelActions = new Panel();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();
            this.lblInfo = new Label();
            this.panelHeader.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = Color.White;
            this.panelHeader.BorderStyle = BorderStyle.FixedSingle;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(1000, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblTitle.Location = new Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(155, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Nuevo Cliente";
            // 
            // panelForm
            // 
            this.panelForm.BackColor = Color.White;
            this.panelForm.BorderStyle = BorderStyle.FixedSingle;
            this.panelForm.Controls.Add(this.lblNombre);
            this.panelForm.Controls.Add(this.txtNombre);
            this.panelForm.Controls.Add(this.lblApellido);
            this.panelForm.Controls.Add(this.txtApellido);
            this.panelForm.Controls.Add(this.lblTelefono);
            this.panelForm.Controls.Add(this.txtTelefono);
            this.panelForm.Controls.Add(this.lblEmail);
            this.panelForm.Controls.Add(this.txtEmail);
            this.panelForm.Controls.Add(this.lblDireccion);
            this.panelForm.Controls.Add(this.txtDireccion);
            this.panelForm.Controls.Add(this.chkActivo);
            this.panelForm.Location = new Point(50, 80);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new Size(900, 400);
            this.panelForm.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblNombre.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblNombre.Location = new Point(30, 30);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new Size(68, 20);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre *";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtNombre.Location = new Point(30, 55);
            this.txtNombre.MaxLength = 100;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new Size(400, 27);
            this.txtNombre.TabIndex = 1;
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblApellido.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblApellido.Location = new Point(460, 30);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new Size(71, 20);
            this.lblApellido.TabIndex = 2;
            this.lblApellido.Text = "Apellido *";
            // 
            // txtApellido
            // 
            this.txtApellido.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtApellido.Location = new Point(460, 55);
            this.txtApellido.MaxLength = 100;
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new Size(400, 27);
            this.txtApellido.TabIndex = 3;
            // 
            // lblTelefono
            // 
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblTelefono.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblTelefono.Location = new Point(30, 100);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new Size(64, 20);
            this.lblTelefono.TabIndex = 4;
            this.lblTelefono.Text = "Teléfono";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtTelefono.Location = new Point(30, 125);
            this.txtTelefono.MaxLength = 20;
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new Size(400, 27);
            this.txtTelefono.TabIndex = 5;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblEmail.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblEmail.Location = new Point(460, 100);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new Size(46, 20);
            this.lblEmail.TabIndex = 6;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtEmail.Location = new Point(460, 125);
            this.txtEmail.MaxLength = 200;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(400, 27);
            this.txtEmail.TabIndex = 7;
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblDireccion.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblDireccion.Location = new Point(30, 170);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new Size(72, 20);
            this.lblDireccion.TabIndex = 8;
            this.lblDireccion.Text = "Dirección";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtDireccion.Location = new Point(30, 195);
            this.txtDireccion.MaxLength = 500;
            this.txtDireccion.Multiline = true;
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new Size(830, 80);
            this.txtDireccion.TabIndex = 9;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Checked = true;
            this.chkActivo.CheckState = CheckState.Checked;
            this.chkActivo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.chkActivo.ForeColor = Color.FromArgb(52, 73, 94);
            this.chkActivo.Location = new Point(30, 295);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new Size(123, 24);
            this.chkActivo.TabIndex = 10;
            this.chkActivo.Text = "Cliente activo";
            this.chkActivo.UseVisualStyleBackColor = true;
            // 
            // panelActions
            // 
            this.panelActions.BackColor = Color.FromArgb(249, 249, 249);
            this.panelActions.BorderStyle = BorderStyle.FixedSingle;
            this.panelActions.Controls.Add(this.btnGuardar);
            this.panelActions.Controls.Add(this.btnCancelar);
            this.panelActions.Controls.Add(this.lblInfo);
            this.panelActions.Dock = DockStyle.Bottom;
            this.panelActions.Location = new Point(0, 570);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new Size(1000, 70);
            this.panelActions.TabIndex = 2;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnGuardar.BackColor = Color.FromArgb(46, 204, 113);
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = FlatStyle.Flat;
            this.btnGuardar.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnGuardar.ForeColor = Color.White;
            this.btnGuardar.Location = new Point(750, 15);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new Size(120, 40);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancelar.BackColor = Color.FromArgb(149, 165, 166);
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnCancelar.ForeColor = Color.White;
            this.btnCancelar.Location = new Point(880, 15);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new Size(100, 40);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            this.lblInfo.ForeColor = Color.FromArgb(127, 140, 141);
            this.lblInfo.Location = new Point(15, 15);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(200, 15);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Los campos marcados con * son obligatorios";
            // 
            // ClienteFormView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelHeader);
            this.Name = "ClienteFormView";
            this.Size = new Size(1000, 640);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelForm;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private CheckBox chkActivo;
        private Panel panelActions;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblInfo;
    }
}