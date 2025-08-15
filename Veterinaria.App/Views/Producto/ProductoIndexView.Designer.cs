namespace Veterinaria.App.Views.Producto
{
    partial class ProductoIndexView
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
            this.btnNuevoProducto = new Button();
            this.panelFilters = new Panel();
            this.txtBuscar = new TextBox();
            this.lblBuscar = new Label();
            this.btnBuscar = new Button();
            this.btnLimpiar = new Button();
            this.lblCategoria = new Label();
            this.cboCategoria = new ComboBox();
            this.lblEstadoStock = new Label();
            this.cboEstadoStock = new ComboBox();
            this.panelActions = new Panel();
            this.btnActualizar = new Button();
            this.lblResultados = new Label();
            this.dgvProductos = new DataGridView();
            this.panelHeader.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.panelActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = Color.White;
            this.panelHeader.BorderStyle = BorderStyle.FixedSingle;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.btnNuevoProducto);
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
            this.lblTitle.Size = new Size(192, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gestión Productos";
            // 
            // btnNuevoProducto
            // 
            this.btnNuevoProducto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnNuevoProducto.BackColor = Color.FromArgb(46, 204, 113);
            this.btnNuevoProducto.FlatAppearance.BorderSize = 0;
            this.btnNuevoProducto.FlatStyle = FlatStyle.Flat;
            this.btnNuevoProducto.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnNuevoProducto.ForeColor = Color.White;
            this.btnNuevoProducto.Location = new Point(850, 15);
            this.btnNuevoProducto.Name = "btnNuevoProducto";
            this.btnNuevoProducto.Size = new Size(130, 35);
            this.btnNuevoProducto.TabIndex = 1;
            this.btnNuevoProducto.Text = "Nuevo Producto";
            this.btnNuevoProducto.UseVisualStyleBackColor = false;
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = Color.FromArgb(249, 249, 249);
            this.panelFilters.BorderStyle = BorderStyle.FixedSingle;
            this.panelFilters.Controls.Add(this.txtBuscar);
            this.panelFilters.Controls.Add(this.lblBuscar);
            this.panelFilters.Controls.Add(this.btnBuscar);
            this.panelFilters.Controls.Add(this.btnLimpiar);
            this.panelFilters.Controls.Add(this.lblCategoria);
            this.panelFilters.Controls.Add(this.cboCategoria);
            this.panelFilters.Controls.Add(this.lblEstadoStock);
            this.panelFilters.Controls.Add(this.cboEstadoStock);
            this.panelFilters.Dock = DockStyle.Top;
            this.panelFilters.Location = new Point(0, 60);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new Size(1000, 80);
            this.panelFilters.TabIndex = 1;
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblBuscar.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblBuscar.Location = new Point(15, 15);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new Size(48, 19);
            this.lblBuscar.TabIndex = 0;
            this.lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtBuscar.Location = new Point(70, 12);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.PlaceholderText = "Código, nombre, descripción o categoría...";
            this.txtBuscar.Size = new Size(300, 25);
            this.txtBuscar.TabIndex = 1;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = Color.FromArgb(52, 152, 219);
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatStyle = FlatStyle.Flat;
            this.btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnBuscar.ForeColor = Color.White;
            this.btnBuscar.Location = new Point(380, 12);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new Size(70, 25);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = Color.FromArgb(149, 165, 166);
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = FlatStyle.Flat;
            this.btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnLimpiar.ForeColor = Color.White;
            this.btnLimpiar.Location = new Point(460, 12);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new Size(70, 25);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // lblCategoria
            // 
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCategoria.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblCategoria.Location = new Point(15, 50);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new Size(68, 19);
            this.lblCategoria.TabIndex = 4;
            this.lblCategoria.Text = "Categoría:";
            // 
            // cboCategoria
            // 
            this.cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.cboCategoria.FormattingEnabled = true;
            this.cboCategoria.Location = new Point(90, 48);
            this.cboCategoria.Name = "cboCategoria";
            this.cboCategoria.Size = new Size(180, 23);
            this.cboCategoria.TabIndex = 5;
            // 
            // lblEstadoStock
            // 
            this.lblEstadoStock.AutoSize = true;
            this.lblEstadoStock.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblEstadoStock.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblEstadoStock.Location = new Point(290, 50);
            this.lblEstadoStock.Name = "lblEstadoStock";
            this.lblEstadoStock.Size = new Size(88, 19);
            this.lblEstadoStock.TabIndex = 6;
            this.lblEstadoStock.Text = "Estado Stock:";
            // 
            // cboEstadoStock
            // 
            this.cboEstadoStock.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboEstadoStock.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.cboEstadoStock.FormattingEnabled = true;
            this.cboEstadoStock.Location = new Point(385, 48);
            this.cboEstadoStock.Name = "cboEstadoStock";
            this.cboEstadoStock.Size = new Size(150, 23);
            this.cboEstadoStock.TabIndex = 7;
            // 
            // panelActions
            // 
            this.panelActions.BackColor = Color.White;
            this.panelActions.Controls.Add(this.btnActualizar);
            this.panelActions.Controls.Add(this.lblResultados);
            this.panelActions.Dock = DockStyle.Top;
            this.panelActions.Location = new Point(0, 140);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new Size(1000, 40);
            this.panelActions.TabIndex = 2;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnActualizar.BackColor = Color.FromArgb(230, 126, 34);
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatStyle = FlatStyle.Flat;
            this.btnActualizar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnActualizar.ForeColor = Color.White;
            this.btnActualizar.Location = new Point(900, 8);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new Size(90, 25);
            this.btnActualizar.TabIndex = 1;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = false;
            // 
            // lblResultados
            // 
            this.lblResultados.AutoSize = true;
            this.lblResultados.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblResultados.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblResultados.Location = new Point(15, 10);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new Size(150, 19);
            this.lblResultados.TabIndex = 0;
            this.lblResultados.Text = "Mostrando 0 resultados";
            // 
            // dgvProductos
            // 
            this.dgvProductos.AllowUserToAddRows = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            this.dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductos.BackgroundColor = Color.White;
            this.dgvProductos.BorderStyle = BorderStyle.None;
            this.dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            this.dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvProductos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 73, 94);
            this.dgvProductos.ColumnHeadersHeight = 35;
            this.dgvProductos.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            this.dgvProductos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvProductos.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvProductos.Dock = DockStyle.Fill;
            this.dgvProductos.EnableHeadersVisualStyles = false;
            this.dgvProductos.GridColor = Color.FromArgb(189, 195, 199);
            this.dgvProductos.Location = new Point(0, 180);
            this.dgvProductos.MultiSelect = false;
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.ReadOnly = true;
            this.dgvProductos.RowHeadersVisible = false;
            this.dgvProductos.RowTemplate.Height = 30;
            this.dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductos.Size = new Size(1000, 460);
            this.dgvProductos.TabIndex = 3;
            // 
            // ProductoIndexView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.panelHeader);
            this.Name = "ProductoIndexView";
            this.Size = new Size(1000, 640);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private Button btnNuevoProducto;
        private Panel panelFilters;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private Button btnBuscar;
        private Button btnLimpiar;
        private Label lblCategoria;
        private ComboBox cboCategoria;
        private Label lblEstadoStock;
        private ComboBox cboEstadoStock;
        private Panel panelActions;
        private Button btnActualizar;
        private Label lblResultados;
        private DataGridView dgvProductos;
    }
}