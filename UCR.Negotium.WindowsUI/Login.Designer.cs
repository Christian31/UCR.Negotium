namespace UCR.Negotium.WindowsUI
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.tbxCorreo = new System.Windows.Forms.TextBox();
            this.tbxPass = new System.Windows.Forms.TextBox();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.llbOlvidoContraseña = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(84, 97);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(59, 18);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Correo:";
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPass.Location = new System.Drawing.Point(54, 150);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(89, 18);
            this.lblPass.TabIndex = 1;
            this.lblPass.Text = "Contraseña:";
            // 
            // tbxCorreo
            // 
            this.tbxCorreo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxCorreo.Location = new System.Drawing.Point(173, 95);
            this.tbxCorreo.Name = "tbxCorreo";
            this.tbxCorreo.Size = new System.Drawing.Size(235, 24);
            this.tbxCorreo.TabIndex = 2;
            // 
            // tbxPass
            // 
            this.tbxPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxPass.Location = new System.Drawing.Point(173, 148);
            this.tbxPass.Name = "tbxPass";
            this.tbxPass.Size = new System.Drawing.Size(235, 24);
            this.tbxPass.TabIndex = 3;
            this.tbxPass.UseSystemPasswordChar = true;
            // 
            // btnIngresar
            // 
            this.btnIngresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIngresar.Location = new System.Drawing.Point(173, 231);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(111, 28);
            this.btnIngresar.TabIndex = 4;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = true;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(299, 231);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "Registrarse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // llbOlvidoContraseña
            // 
            this.llbOlvidoContraseña.AutoSize = true;
            this.llbOlvidoContraseña.BackColor = System.Drawing.SystemColors.Menu;
            this.llbOlvidoContraseña.Location = new System.Drawing.Point(173, 191);
            this.llbOlvidoContraseña.Name = "llbOlvidoContraseña";
            this.llbOlvidoContraseña.Size = new System.Drawing.Size(119, 13);
            this.llbOlvidoContraseña.TabIndex = 6;
            this.llbOlvidoContraseña.TabStop = true;
            this.llbOlvidoContraseña.Text = "¿Olvidó su contraseña?";
            this.llbOlvidoContraseña.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbOlvidoContraseña_LinkClicked);
            // 
            // Login
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(766, 372);
            this.Controls.Add(this.llbOlvidoContraseña);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnIngresar);
            this.Controls.Add(this.tbxPass);
            this.Controls.Add(this.tbxCorreo);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.lblEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(782, 411);
            this.MinimumSize = new System.Drawing.Size(782, 411);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.Login_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox tbxCorreo;
        private System.Windows.Forms.TextBox tbxPass;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel llbOlvidoContraseña;
    }
}