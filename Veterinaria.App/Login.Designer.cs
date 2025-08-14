namespace Veterinaria.App
{
    partial class Login
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
            this.panelMain = new Panel();
            this.panelLogin = new Panel();
            this.lblTitulo = new Label();
            this.lblSubtitulo = new Label();
            this.txtUsuario = new TextBox();
            this.txtContrasena = new TextBox();
            this.btnLogin = new Button();
            this.btnCancelar = new Button();
            this.lblOlvideContrasena = new Label();
            this.lblCrearCuenta = new Label();
            this.panelLogo = new Panel();
            this.lblLogo = new Label();
            this.panelMain.SuspendLayout();
            this.panelLogin.SuspendLayout();
            this.panelLogo.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.panelMain.Controls.Add(this.panelLogin);
            this.panelMain.Controls.Add(this.panelLogo);
            this.panelMain.Dock = DockStyle.Fill;
            this.panelMain.Location = new Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(800, 500);
            this.panelMain.TabIndex = 0;
            
            // 
            // panelLogo
            // 
            this.panelLogo.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.panelLogo.Controls.Add(this.lblLogo);
            this.panelLogo.Dock = DockStyle.Left;
            this.panelLogo.Location = new Point(0, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new Size(400, 500);
            this.panelLogo.TabIndex = 0;
            
            // 
            // lblLogo
            // 
            this.lblLogo.Anchor = AnchorStyles.None;
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new Font("Segoe UI", 32F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblLogo.ForeColor = Color.White;
            this.lblLogo.Location = new Point(50, 200);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new Size(300, 59);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "🐾 Veterinaria";
            this.lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            
            // 
            // panelLogin
            // 
            this.panelLogin.BackColor = Color.White;
            this.panelLogin.Controls.Add(this.lblTitulo);
            this.panelLogin.Controls.Add(this.lblSubtitulo);
            this.panelLogin.Controls.Add(this.txtUsuario);
            this.panelLogin.Controls.Add(this.txtContrasena);
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.Controls.Add(this.btnCancelar);
            this.panelLogin.Controls.Add(this.lblOlvideContrasena);
            this.panelLogin.Controls.Add(this.lblCrearCuenta);
            this.panelLogin.Dock = DockStyle.Fill;
            this.panelLogin.Location = new Point(400, 0);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Padding = new Padding(40);
            this.panelLogin.Size = new Size(400, 500);
            this.panelLogin.TabIndex = 1;
            
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.lblTitulo.Location = new Point(50, 80);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(174, 45);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Bienvenido";
            
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.lblSubtitulo.Location = new Point(50, 130);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new Size(200, 21);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Ingrese sus credenciales";
            
            // 
            // txtUsuario
            // 
            this.txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            this.txtUsuario.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtUsuario.ForeColor = Color.Gray;
            this.txtUsuario.Location = new Point(50, 180);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new Size(300, 29);
            this.txtUsuario.TabIndex = 2;
            this.txtUsuario.Text = "Usuario o Email";
            
            // 
            // txtContrasena
            // 
            this.txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            this.txtContrasena.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtContrasena.Location = new Point(50, 230);
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.PasswordChar = '*';
            this.txtContrasena.PlaceholderText = "Contraseña";
            this.txtContrasena.Size = new Size(300, 29);
            this.txtContrasena.TabIndex = 3;
            
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.Location = new Point(50, 290);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(140, 40);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Ingresar";
            this.btnLogin.UseVisualStyleBackColor = false;
            
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnCancelar.ForeColor = Color.White;
            this.btnCancelar.Location = new Point(210, 290);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new Size(140, 40);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            
            // 
            // lblOlvideContrasena
            // 
            this.lblOlvideContrasena.AutoSize = true;
            this.lblOlvideContrasena.Cursor = Cursors.Hand;
            this.lblOlvideContrasena.Font = new Font("Segoe UI", 10F, FontStyle.Underline, GraphicsUnit.Point);
            this.lblOlvideContrasena.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lblOlvideContrasena.Location = new Point(50, 360);
            this.lblOlvideContrasena.Name = "lblOlvideContrasena";
            this.lblOlvideContrasena.Size = new Size(138, 19);
            this.lblOlvideContrasena.TabIndex = 6;
            this.lblOlvideContrasena.Text = "Olvidé mi contraseña";
            
            // 
            // lblCrearCuenta
            // 
            this.lblCrearCuenta.AutoSize = true;
            this.lblCrearCuenta.Cursor = Cursors.Hand;
            this.lblCrearCuenta.Font = new Font("Segoe UI", 10F, FontStyle.Underline, GraphicsUnit.Point);
            this.lblCrearCuenta.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lblCrearCuenta.Location = new Point(50, 390);
            this.lblCrearCuenta.Name = "lblCrearCuenta";
            this.lblCrearCuenta.Size = new Size(88, 19);
            this.lblCrearCuenta.TabIndex = 7;
            this.lblCrearCuenta.Text = "Crear cuenta";
            
            // 
            // Login
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Veterinaria - Login";
            this.panelMain.ResumeLayout(false);
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.panelLogo.ResumeLayout(false);
            this.panelLogo.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel panelLogo;
        private Label lblLogo;
        private Panel panelLogin;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private TextBox txtUsuario;
        private TextBox txtContrasena;
        private Button btnLogin;
        private Button btnCancelar;
        private Label lblOlvideContrasena;
        private Label lblCrearCuenta;
    }
}
