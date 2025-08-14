namespace Veterinaria.App.Views.Cliente
{
    partial class ClienteIndexView
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
            this.btnNuevoCliente = new Button();
            this.panelFilters = new Panel();
            this.txtBuscar = new TextBox();
            this.lblBuscar = new Label();
            this.btnBuscar = new Button();
            this.btnLimpiar = new Button();
            this.panelActions = new Panel();
            this.btnActualizar = new Button();
            this.lblResultados = new Label();
            this.dgvClientes = new DataGridView();
            this.panelHeader.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.panelActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = Color.White;
            this.panelHeader.BorderStyle = BorderStyle.FixedSingle;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.btnNuevoCliente);
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
            this.lblTitle.Size = new Size(186, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gestión Clientes";
            // 
            // btnNuevoCliente
            // 
            this.btnNuevoCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnNuevoCliente.BackColor = Color.FromArgb(46, 204, 113);
            this.btnNuevoCliente.FlatAppearance.BorderSize = 0;
            this.btnNuevoCliente.FlatStyle = FlatStyle.Flat;
            this.btnNuevoCliente.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnNuevoCliente.ForeColor = Color.White;
            this.btnNuevoCliente.Location = new Point(850, 15);
            this.btnNuevoCliente.Name = "btnNuevoCliente";
            this.btnNuevoCliente.Size = new Size(130, 35);
            this.btnNuevoCliente.TabIndex = 1;
            this.btnNuevoCliente.Text = "Nuevo Cliente";
            this.btnNuevoCliente.UseVisualStyleBackColor = false;
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = Color.FromArgb(249, 249, 249);
            this.panelFilters.BorderStyle = BorderStyle.FixedSingle;
            this.panelFilters.Controls.Add(this.txtBuscar);
            this.panelFilters.Controls.Add(this.lblBuscar);
            this.panelFilters.Controls.Add(this.btnBuscar);
            this.panelFilters.Controls.Add(this.btnLimpiar);
            this.panelFilters.Dock = DockStyle.Top;
            this.panelFilters.Location = new Point(0, 60);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new Size(1000, 50);
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
            this.txtBuscar.PlaceholderText = "Nombre, apellido o email...";
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
            // panelActions
            // 
            this.panelActions.BackColor = Color.White;
            this.panelActions.Controls.Add(this.btnActualizar);
            this.panelActions.Controls.Add(this.lblResultados);
            this.panelActions.Dock = DockStyle.Top;
            this.panelActions.Location = new Point(0, 110);
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
            // dgvClientes
            // 
            this.dgvClientes.AllowUserToAddRows = false;
            this.dgvClientes.AllowUserToDeleteRows = false;
            this.dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClientes.BackgroundColor = Color.White;
            this.dgvClientes.BorderStyle = BorderStyle.None;
            this.dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            this.dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvClientes.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 73, 94);
            this.dgvClientes.ColumnHeadersHeight = 35;
            this.dgvClientes.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            this.dgvClientes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvClientes.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvClientes.Dock = DockStyle.Fill;
            this.dgvClientes.EnableHeadersVisualStyles = false;
            this.dgvClientes.GridColor = Color.FromArgb(189, 195, 199);
            this.dgvClientes.Location = new Point(0, 150);
            this.dgvClientes.MultiSelect = false;
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.ReadOnly = true;
            this.dgvClientes.RowHeadersVisible = false;
            this.dgvClientes.RowTemplate.Height = 30;
            this.dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientes.Size = new Size(1000, 490);
            this.dgvClientes.TabIndex = 3;
            // 
            // ClienteIndexView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Controls.Add(this.dgvClientes);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.panelHeader);
            this.Name = "ClienteIndexView";
            this.Size = new Size(1000, 640);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private Button btnNuevoCliente;
        private Panel panelFilters;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private Button btnBuscar;
        private Button btnLimpiar;
        private Panel panelActions;
        private Button btnActualizar;
        private Label lblResultados;
        private DataGridView dgvClientes;
    }
}