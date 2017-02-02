namespace UCR.Negotium.WindowsUI
{
    partial class InteresFinanciamientoIVDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InteresFinanciamientoIVDialog));
            this.dgvInteresesFinanciamiento = new System.Windows.Forms.DataGridView();
            this.btnGuardarInteres = new System.Windows.Forms.Button();
            this.btnCancelarInteres = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInteresesFinanciamiento)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInteresesFinanciamiento
            // 
            this.dgvInteresesFinanciamiento.AllowUserToAddRows = false;
            this.dgvInteresesFinanciamiento.AllowUserToDeleteRows = false;
            this.dgvInteresesFinanciamiento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInteresesFinanciamiento.Location = new System.Drawing.Point(41, 56);
            this.dgvInteresesFinanciamiento.Name = "dgvInteresesFinanciamiento";
            this.dgvInteresesFinanciamiento.Size = new System.Drawing.Size(294, 312);
            this.dgvInteresesFinanciamiento.TabIndex = 0;
            // 
            // btnGuardarInteres
            // 
            this.btnGuardarInteres.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardarInteres.Image")));
            this.btnGuardarInteres.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardarInteres.Location = new System.Drawing.Point(41, 386);
            this.btnGuardarInteres.Name = "btnGuardarInteres";
            this.btnGuardarInteres.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarInteres.TabIndex = 1;
            this.btnGuardarInteres.Text = "Guardar";
            this.btnGuardarInteres.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardarInteres.UseVisualStyleBackColor = true;
            this.btnGuardarInteres.Click += new System.EventHandler(this.btnGuardarInteres_Click);
            // 
            // btnCancelarInteres
            // 
            this.btnCancelarInteres.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelarInteres.Image")));
            this.btnCancelarInteres.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelarInteres.Location = new System.Drawing.Point(268, 386);
            this.btnCancelarInteres.Name = "btnCancelarInteres";
            this.btnCancelarInteres.Size = new System.Drawing.Size(67, 23);
            this.btnCancelarInteres.TabIndex = 2;
            this.btnCancelarInteres.Text = "Volver";
            this.btnCancelarInteres.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelarInteres.UseVisualStyleBackColor = true;
            this.btnCancelarInteres.Click += new System.EventHandler(this.btnCancelarInteres_Click);
            // 
            // InteresFinanciamientoUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(380, 452);
            this.Controls.Add(this.btnCancelarInteres);
            this.Controls.Add(this.btnGuardarInteres);
            this.Controls.Add(this.dgvInteresesFinanciamiento);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InteresFinanciamientoUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Intereses sobre el Financiamiento";
            ((System.ComponentModel.ISupportInitialize)(this.dgvInteresesFinanciamiento)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInteresesFinanciamiento;
        private System.Windows.Forms.Button btnGuardarInteres;
        private System.Windows.Forms.Button btnCancelarInteres;
    }
}