namespace Veterinaria.App.Views.Producto
{
    partial class ProductoFormView
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
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
            this.lblCodigo = new Label();
            this.txtCodigo = new TextBox();
            this.lblNombre = new Label();
            this.txtNombre = new TextBox();
            this.lblDescripcion = new Label();
            this.txtDescripcion = new TextBox();
            this.lblPrecio = new Label();
            this.numPrecio = new NumericUpDown();
            this.lblStock = new Label();
            this.numStock = new NumericUpDown();
            this.lblStockMinimo = new Label();
            this.numStockMinimo = new NumericUpDown();
            this.lblCategoria = new Label();
            this.txtCategoria = new TextBox();
            this.lblProveedor = new Label();
            this.txtProveedor = new TextBox();
            this.chkActivo = new CheckBox();
            this.panelActions = new Panel();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();
            this.lblInfo = new Label();
            this.panelHeader.SuspendLayout();
            this.panelForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStockMinimo)).BeginInit();
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
            this.lblTitle.Size = new Size(175, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Nuevo Producto";
            // 
            // panelForm
            // 
            this.panelForm.BackColor = Color.White;
            this.panelForm.BorderStyle = BorderStyle.FixedSingle;
            this.panelForm.Controls.Add(this.lblCodigo);
            this.panelForm.Controls.Add(this.txtCodigo);
            this.panelForm.Controls.Add(this.lblNombre);
            this.panelForm.Controls.Add(this.txtNombre);
            this.panelForm.Controls.Add(this.lblDescripcion);
            this.panelForm.Controls.Add(this.txtDescripcion);
            this.panelForm.Controls.Add(this.lblPrecio);
            this.panelForm.Controls.Add(this.numPrecio);
            this.panelForm.Controls.Add(this.lblStock);
            this.panelForm.Controls.Add(this.numStock);
            this.panelForm.Controls.Add(this.lblStockMinimo);
            this.panelForm.Controls.Add(this.numStockMinimo);
            this.panelForm.Controls.Add(this.lblCategoria);
            this.panelForm.Controls.Add(this.txtCategoria);
            this.panelForm.Controls.Add(this.lblProveedor);
            this.panelForm.Controls.Add(this.txtProveedor);
            this.panelForm.Controls.Add(this.chkActivo);
            this.panelForm.Location = new Point(50, 80);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new Size(900, 480);
            this.panelForm.TabIndex = 1;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCodigo.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblCodigo.Location = new Point(30, 30);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new Size(65, 20);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código *";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtCodigo.Location = new Point(30, 55);
            this.txtCodigo.MaxLength = 50;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new Size(200, 27);
            this.txtCodigo.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblNombre.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblNombre.Location = new Point(260, 30);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new Size(68, 20);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre *";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtNombre.Location = new Point(260, 55);
            this.txtNombre.MaxLength = 200;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new Size(600, 27);
            this.txtNombre.TabIndex = 3;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblDescripcion.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblDescripcion.Location = new Point(30, 100);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new Size(87, 20);
            this.lblDescripcion.TabIndex = 4;
            this.lblDescripcion.Text = "Descripción";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtDescripcion.Location = new Point(30, 125);
            this.txtDescripcion.MaxLength = 500;
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new Size(830, 80);
            this.txtDescripcion.TabIndex = 5;
            // 
            // lblPrecio
            // 
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblPrecio.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblPrecio.Location = new Point(30, 225);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new Size(58, 20);
            this.lblPrecio.TabIndex = 6;
            this.lblPrecio.Text = "Precio *";
            // 
            // numPrecio
            // 
            this.numPrecio.DecimalPlaces = 2;
            this.numPrecio.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.numPrecio.Location = new Point(30, 250);
            this.numPrecio.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.numPrecio.Name = "numPrecio";
            this.numPrecio.Size = new Size(150, 27);
            this.numPrecio.TabIndex = 7;
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblStock.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblStock.Location = new Point(210, 225);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new Size(44, 20);
            this.lblStock.TabIndex = 8;
            this.lblStock.Text = "Stock";
            // 
            // numStock
            // 
            this.numStock.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.numStock.Location = new Point(210, 250);
            this.numStock.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.numStock.Name = "numStock";
            this.numStock.Size = new Size(120, 27);
            this.numStock.TabIndex = 9;
            // 
            // lblStockMinimo
            // 
            this.lblStockMinimo.AutoSize = true;
            this.lblStockMinimo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblStockMinimo.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblStockMinimo.Location = new Point(360, 225);
            this.lblStockMinimo.Name = "lblStockMinimo";
            this.lblStockMinimo.Size = new Size(100, 20);
            this.lblStockMinimo.TabIndex = 10;
            this.lblStockMinimo.Text = "Stock Mínimo";
            // 
            // numStockMinimo
            // 
            this.numStockMinimo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.numStockMinimo.Location = new Point(360, 250);
            this.numStockMinimo.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.numStockMinimo.Name = "numStockMinimo";
            this.numStockMinimo.Size = new Size(120, 27);
            this.numStockMinimo.TabIndex = 11;
            // 
            // lblCategoria
            // 
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCategoria.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblCategoria.Location = new Point(30, 295);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new Size(72, 20);
            this.lblCategoria.TabIndex = 12;
            this.lblCategoria.Text = "Categoría";
            // 
            // txtCategoria
            // 
            this.txtCategoria.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtCategoria.Location = new Point(30, 320);
            this.txtCategoria.MaxLength = 100;
            this.txtCategoria.Name = "txtCategoria";
            this.txtCategoria.Size = new Size(400, 27);
            this.txtCategoria.TabIndex = 13;
            // 
            // lblProveedor
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblProveedor.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblProveedor.Location = new Point(460, 295);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new Size(75, 20);
            this.lblProveedor.TabIndex = 14;
            this.lblProveedor.Text = "Proveedor";
            // 
            // txtProveedor
            // 
            this.txtProveedor.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtProveedor.Location = new Point(460, 320);
            this.txtProveedor.MaxLength = 200;
            this.txtProveedor.Name = "txtProveedor";
            this.txtProveedor.Size = new Size(400, 27);
            this.txtProveedor.TabIndex = 15;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Checked = true;
            this.chkActivo.CheckState = CheckState.Checked;
            this.chkActivo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.chkActivo.ForeColor = Color.FromArgb(52, 73, 94);
            this.chkActivo.Location = new Point(30, 375);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new Size(133, 24);
            this.chkActivo.TabIndex = 16;
            this.chkActivo.Text = "Producto activo";
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
            // ProductoFormView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelHeader);
            this.Name = "ProductoFormView";
            this.Size = new Size(1000, 640);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStockMinimo)).EndInit();
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelForm;
        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblPrecio;
        private NumericUpDown numPrecio;
        private Label lblStock;
        private NumericUpDown numStock;
        private Label lblStockMinimo;
        private NumericUpDown numStockMinimo;
        private Label lblCategoria;
        private TextBox txtCategoria;
        private Label lblProveedor;
        private TextBox txtProveedor;
        private CheckBox chkActivo;
        private Panel panelActions;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblInfo;
    }
}