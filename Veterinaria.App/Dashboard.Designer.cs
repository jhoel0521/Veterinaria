namespace Veterinaria.App
{
    partial class Dashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new Panel();
            this.lblUsuario = new Label();
            this.lblEmail = new Label();
            this.btnLogout = new Button();
            this.panelSidebar = new Panel();
            this.btnClientes = new Button();
            this.btnMascotas = new Button();
            this.btnVentas = new Button();
            this.btnProductos = new Button();
            this.btnReportes = new Button();
            this.btnConfiguracion = new Button();
            this.panelContent = new Panel();
            this.lblWelcome = new Label();
            this.panelTop.SuspendLayout();
            this.panelSidebar.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.panelTop.Controls.Add(this.lblUsuario);
            this.panelTop.Controls.Add(this.lblEmail);
            this.panelTop.Controls.Add(this.btnLogout);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(1200, 60);
            this.panelTop.TabIndex = 0;
            
            // 
            // lblUsuario
            // 
            this.lblUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblUsuario.ForeColor = Color.White;
            this.lblUsuario.Location = new Point(20, 10);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new Size(200, 21);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Bienvenido, Usuario";
            
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblEmail.ForeColor = Color.LightGray;
            this.lblEmail.Location = new Point(20, 35);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new Size(100, 15);
            this.lblEmail.TabIndex = 1;
            this.lblEmail.Text = "usuario@email.com";
            
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = FlatStyle.Flat;
            this.btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnLogout.ForeColor = Color.White;
            this.btnLogout.Location = new Point(1100, 15);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new Size(90, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Cerrar Sesión";
            this.btnLogout.UseVisualStyleBackColor = false;
            
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.panelSidebar.Controls.Add(this.btnClientes);
            this.panelSidebar.Controls.Add(this.btnMascotas);
            this.panelSidebar.Controls.Add(this.btnVentas);
            this.panelSidebar.Controls.Add(this.btnProductos);
            this.panelSidebar.Controls.Add(this.btnReportes);
            this.panelSidebar.Controls.Add(this.btnConfiguracion);
            this.panelSidebar.Dock = DockStyle.Left;
            this.panelSidebar.Location = new Point(0, 60);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new Size(200, 640);
            this.panelSidebar.TabIndex = 1;
            
            // 
            // btnClientes
            // 
            this.btnClientes.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnClientes.FlatAppearance.BorderSize = 0;
            this.btnClientes.FlatStyle = FlatStyle.Flat;
            this.btnClientes.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnClientes.ForeColor = Color.White;
            this.btnClientes.Location = new Point(0, 20);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new Size(200, 50);
            this.btnClientes.TabIndex = 0;
            this.btnClientes.Text = "🧑‍🤝‍🧑 Clientes";
            this.btnClientes.TextAlign = ContentAlignment.MiddleLeft;
            this.btnClientes.UseVisualStyleBackColor = false;
            
            // 
            // btnMascotas
            // 
            this.btnMascotas.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnMascotas.FlatAppearance.BorderSize = 0;
            this.btnMascotas.FlatStyle = FlatStyle.Flat;
            this.btnMascotas.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnMascotas.ForeColor = Color.White;
            this.btnMascotas.Location = new Point(0, 80);
            this.btnMascotas.Name = "btnMascotas";
            this.btnMascotas.Size = new Size(200, 50);
            this.btnMascotas.TabIndex = 1;
            this.btnMascotas.Text = "🐾 Mascotas";
            this.btnMascotas.TextAlign = ContentAlignment.MiddleLeft;
            this.btnMascotas.UseVisualStyleBackColor = false;
            
            // 
            // btnVentas
            // 
            this.btnVentas.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnVentas.FlatAppearance.BorderSize = 0;
            this.btnVentas.FlatStyle = FlatStyle.Flat;
            this.btnVentas.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnVentas.ForeColor = Color.White;
            this.btnVentas.Location = new Point(0, 140);
            this.btnVentas.Name = "btnVentas";
            this.btnVentas.Size = new Size(200, 50);
            this.btnVentas.TabIndex = 2;
            this.btnVentas.Text = "🛒 Ventas";
            this.btnVentas.TextAlign = ContentAlignment.MiddleLeft;
            this.btnVentas.UseVisualStyleBackColor = false;
            
            // 
            // btnProductos
            // 
            this.btnProductos.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnProductos.FlatAppearance.BorderSize = 0;
            this.btnProductos.FlatStyle = FlatStyle.Flat;
            this.btnProductos.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnProductos.ForeColor = Color.White;
            this.btnProductos.Location = new Point(0, 200);
            this.btnProductos.Name = "btnProductos";
            this.btnProductos.Size = new Size(200, 50);
            this.btnProductos.TabIndex = 3;
            this.btnProductos.Text = "📦 Productos";
            this.btnProductos.TextAlign = ContentAlignment.MiddleLeft;
            this.btnProductos.UseVisualStyleBackColor = false;
            
            // 
            // btnReportes
            // 
            this.btnReportes.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnReportes.FlatAppearance.BorderSize = 0;
            this.btnReportes.FlatStyle = FlatStyle.Flat;
            this.btnReportes.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnReportes.ForeColor = Color.White;
            this.btnReportes.Location = new Point(0, 260);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new Size(200, 50);
            this.btnReportes.TabIndex = 4;
            this.btnReportes.Text = "📊 Reportes";
            this.btnReportes.TextAlign = ContentAlignment.MiddleLeft;
            this.btnReportes.UseVisualStyleBackColor = false;
            
            // 
            // btnConfiguracion
            // 
            this.btnConfiguracion.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnConfiguracion.FlatAppearance.BorderSize = 0;
            this.btnConfiguracion.FlatStyle = FlatStyle.Flat;
            this.btnConfiguracion.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnConfiguracion.ForeColor = Color.White;
            this.btnConfiguracion.Location = new Point(0, 320);
            this.btnConfiguracion.Name = "btnConfiguracion";
            this.btnConfiguracion.Size = new Size(200, 50);
            this.btnConfiguracion.TabIndex = 5;
            this.btnConfiguracion.Text = "⚙️ Configuración";
            this.btnConfiguracion.TextAlign = ContentAlignment.MiddleLeft;
            this.btnConfiguracion.UseVisualStyleBackColor = false;
            
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.panelContent.Controls.Add(this.lblWelcome);
            this.panelContent.Dock = DockStyle.Fill;
            this.panelContent.Location = new Point(200, 60);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new Size(1000, 640);
            this.panelContent.TabIndex = 2;
            
            // 
            // lblWelcome
            // 
            this.lblWelcome.Anchor = AnchorStyles.None;
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.lblWelcome.Location = new Point(300, 280);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new Size(400, 45);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Sistema de Veterinaria";
            this.lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.panelTop);
            this.Name = "Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Veterinaria - Dashboard";
            this.WindowState = FormWindowState.Maximized;
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSidebar.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label lblUsuario;
        private Label lblEmail;
        private Button btnLogout;
        private Panel panelSidebar;
        private Button btnClientes;
        private Button btnMascotas;
        private Button btnVentas;
        private Button btnProductos;
        private Button btnReportes;
        private Button btnConfiguracion;
        private Panel panelContent;
        private Label lblWelcome;
    }
}
