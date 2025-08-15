namespace Veterinaria.App.Views.Mascota
{
    partial class MascotaFormView
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
            this.lblCliente = new Label();
            this.cboCliente = new ComboBox();
            this.lblNombre = new Label();
            this.txtNombre = new TextBox();
            this.lblEspecie = new Label();
            this.txtEspecie = new TextBox();
            this.lblRaza = new Label();
            this.txtRaza = new TextBox();
            this.lblEdad = new Label();
            this.numEdad = new NumericUpDown();
            this.lblPeso = new Label();
            this.numPeso = new NumericUpDown();
            this.lblColor = new Label();
            this.txtColor = new TextBox();
            this.lblObservaciones = new Label();
            this.txtObservaciones = new TextBox();
            this.chkActivo = new CheckBox();
            this.panelActions = new Panel();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();
            this.lblInfo = new Label();
            this.panelHeader.SuspendLayout();
            this.panelForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeso)).BeginInit();
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
            this.lblTitle.Size = new Size(165, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Nueva Mascota";
            // 
            // panelForm
            // 
            this.panelForm.BackColor = Color.White;
            this.panelForm.BorderStyle = BorderStyle.FixedSingle;
            this.panelForm.Controls.Add(this.lblCliente);
            this.panelForm.Controls.Add(this.cboCliente);
            this.panelForm.Controls.Add(this.lblNombre);
            this.panelForm.Controls.Add(this.txtNombre);
            this.panelForm.Controls.Add(this.lblEspecie);
            this.panelForm.Controls.Add(this.txtEspecie);
            this.panelForm.Controls.Add(this.lblRaza);
            this.panelForm.Controls.Add(this.txtRaza);
            this.panelForm.Controls.Add(this.lblEdad);
            this.panelForm.Controls.Add(this.numEdad);
            this.panelForm.Controls.Add(this.lblPeso);
            this.panelForm.Controls.Add(this.numPeso);
            this.panelForm.Controls.Add(this.lblColor);
            this.panelForm.Controls.Add(this.txtColor);
            this.panelForm.Controls.Add(this.lblObservaciones);
            this.panelForm.Controls.Add(this.txtObservaciones);
            this.panelForm.Controls.Add(this.chkActivo);
            this.panelForm.Location = new Point(50, 80);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new Size(900, 480);
            this.panelForm.TabIndex = 1;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCliente.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblCliente.Location = new Point(30, 30);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new Size(60, 20);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Dueño *";
            // 
            // cboCliente
            // 
            this.cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboCliente.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.cboCliente.FormattingEnabled = true;
            this.cboCliente.Location = new Point(30, 55);
            this.cboCliente.Name = "cboCliente";
            this.cboCliente.Size = new Size(830, 28);
            this.cboCliente.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblNombre.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblNombre.Location = new Point(30, 100);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new Size(68, 20);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre *";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtNombre.Location = new Point(30, 125);
            this.txtNombre.MaxLength = 100;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new Size(400, 27);
            this.txtNombre.TabIndex = 3;
            // 
            // lblEspecie
            // 
            this.lblEspecie.AutoSize = true;
            this.lblEspecie.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblEspecie.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblEspecie.Location = new Point(460, 100);
            this.lblEspecie.Name = "lblEspecie";
            this.lblEspecie.Size = new Size(66, 20);
            this.lblEspecie.TabIndex = 4;
            this.lblEspecie.Text = "Especie *";
            // 
            // txtEspecie
            // 
            this.txtEspecie.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtEspecie.Location = new Point(460, 125);
            this.txtEspecie.MaxLength = 100;
            this.txtEspecie.Name = "txtEspecie";
            this.txtEspecie.Size = new Size(400, 27);
            this.txtEspecie.TabIndex = 5;
            // 
            // lblRaza
            // 
            this.lblRaza.AutoSize = true;
            this.lblRaza.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblRaza.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblRaza.Location = new Point(30, 170);
            this.lblRaza.Name = "lblRaza";
            this.lblRaza.Size = new Size(37, 20);
            this.lblRaza.TabIndex = 6;
            this.lblRaza.Text = "Raza";
            // 
            // txtRaza
            // 
            this.txtRaza.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtRaza.Location = new Point(30, 195);
            this.txtRaza.MaxLength = 100;
            this.txtRaza.Name = "txtRaza";
            this.txtRaza.Size = new Size(400, 27);
            this.txtRaza.TabIndex = 7;
            // 
            // lblEdad
            // 
            this.lblEdad.AutoSize = true;
            this.lblEdad.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblEdad.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblEdad.Location = new Point(460, 170);
            this.lblEdad.Name = "lblEdad";
            this.lblEdad.Size = new Size(79, 20);
            this.lblEdad.TabIndex = 8;
            this.lblEdad.Text = "Edad (años)";
            // 
            // numEdad
            // 
            this.numEdad.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.numEdad.Location = new Point(460, 195);
            this.numEdad.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            this.numEdad.Name = "numEdad";
            this.numEdad.Size = new Size(180, 27);
            this.numEdad.TabIndex = 9;
            // 
            // lblPeso
            // 
            this.lblPeso.AutoSize = true;
            this.lblPeso.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblPeso.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblPeso.Location = new Point(680, 170);
            this.lblPeso.Name = "lblPeso";
            this.lblPeso.Size = new Size(66, 20);
            this.lblPeso.TabIndex = 10;
            this.lblPeso.Text = "Peso (kg)";
            // 
            // numPeso
            // 
            this.numPeso.DecimalPlaces = 2;
            this.numPeso.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.numPeso.Location = new Point(680, 195);
            this.numPeso.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            this.numPeso.Name = "numPeso";
            this.numPeso.Size = new Size(180, 27);
            this.numPeso.TabIndex = 11;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblColor.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblColor.Location = new Point(30, 240);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new Size(44, 20);
            this.lblColor.TabIndex = 12;
            this.lblColor.Text = "Color";
            // 
            // txtColor
            // 
            this.txtColor.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtColor.Location = new Point(30, 265);
            this.txtColor.MaxLength = 50;
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new Size(400, 27);
            this.txtColor.TabIndex = 13;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblObservaciones.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblObservaciones.Location = new Point(30, 310);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new Size(104, 20);
            this.lblObservaciones.TabIndex = 14;
            this.lblObservaciones.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtObservaciones.Location = new Point(30, 335);
            this.txtObservaciones.MaxLength = 500;
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new Size(830, 80);
            this.txtObservaciones.TabIndex = 15;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Checked = true;
            this.chkActivo.CheckState = CheckState.Checked;
            this.chkActivo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.chkActivo.ForeColor = Color.FromArgb(52, 73, 94);
            this.chkActivo.Location = new Point(30, 435);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new Size(130, 24);
            this.chkActivo.TabIndex = 16;
            this.chkActivo.Text = "Mascota activa";
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
            this.lblInfo.Size = new Size(212, 15);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Los campos marcados con * son obligatorios";
            // 
            // MascotaFormView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelHeader);
            this.Name = "MascotaFormView";
            this.Size = new Size(1000, 640);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeso)).EndInit();
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelForm;
        private Label lblCliente;
        private ComboBox cboCliente;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblEspecie;
        private TextBox txtEspecie;
        private Label lblRaza;
        private TextBox txtRaza;
        private Label lblEdad;
        private NumericUpDown numEdad;
        private Label lblPeso;
        private NumericUpDown numPeso;
        private Label lblColor;
        private TextBox txtColor;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private CheckBox chkActivo;
        private Panel panelActions;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblInfo;
    }
}