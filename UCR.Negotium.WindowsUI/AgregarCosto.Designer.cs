namespace UCR.Negotium.WindowsUI
{
    partial class AgregarCosto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgregarCosto));
            this.dgvCosto = new System.Windows.Forms.DataGridView();
            this.btnAgregarCosto = new System.Windows.Forms.Button();
            this.btnCancelarCosto = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxNombreCosto = new System.Windows.Forms.TextBox();
            this.cbxUnidadCosto = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCosto)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCosto
            // 
            this.dgvCosto.AllowDrop = true;
            this.dgvCosto.AllowUserToAddRows = false;
            this.dgvCosto.AllowUserToDeleteRows = false;
            this.dgvCosto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCosto.Location = new System.Drawing.Point(96, 85);
            this.dgvCosto.Name = "dgvCosto";
            this.dgvCosto.Size = new System.Drawing.Size(346, 307);
            this.dgvCosto.TabIndex = 0;
            // 
            // btnAgregarCosto
            // 
            this.btnAgregarCosto.Image = ((System.Drawing.Image)(resources.GetObject("btnAgregarCosto.Image")));
            this.btnAgregarCosto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregarCosto.Location = new System.Drawing.Point(131, 412);
            this.btnAgregarCosto.Name = "btnAgregarCosto";
            this.btnAgregarCosto.Size = new System.Drawing.Size(98, 23);
            this.btnAgregarCosto.TabIndex = 1;
            this.btnAgregarCosto.Text = "Agregar Costo";
            this.btnAgregarCosto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAgregarCosto.UseVisualStyleBackColor = true;
            this.btnAgregarCosto.Click += new System.EventHandler(this.btnAgregarCosto_Click);
            // 
            // btnCancelarCosto
            // 
            this.btnCancelarCosto.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelarCosto.Image")));
            this.btnCancelarCosto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelarCosto.Location = new System.Drawing.Point(326, 412);
            this.btnCancelarCosto.Name = "btnCancelarCosto";
            this.btnCancelarCosto.Size = new System.Drawing.Size(75, 23);
            this.btnCancelarCosto.TabIndex = 2;
            this.btnCancelarCosto.Text = "Cancelar";
            this.btnCancelarCosto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelarCosto.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nombre del Costo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Unidad de medida";
            // 
            // tbxNombreCosto
            // 
            this.tbxNombreCosto.Location = new System.Drawing.Point(131, 33);
            this.tbxNombreCosto.Name = "tbxNombreCosto";
            this.tbxNombreCosto.Size = new System.Drawing.Size(142, 20);
            this.tbxNombreCosto.TabIndex = 5;
            // 
            // cbxUnidadCosto
            // 
            this.cbxUnidadCosto.FormattingEnabled = true;
            this.cbxUnidadCosto.Location = new System.Drawing.Point(386, 33);
            this.cbxUnidadCosto.Name = "cbxUnidadCosto";
            this.cbxUnidadCosto.Size = new System.Drawing.Size(121, 21);
            this.cbxUnidadCosto.TabIndex = 6;
            // 
            // AgregarCosto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(552, 467);
            this.Controls.Add(this.cbxUnidadCosto);
            this.Controls.Add(this.tbxNombreCosto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancelarCosto);
            this.Controls.Add(this.btnAgregarCosto);
            this.Controls.Add(this.dgvCosto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AgregarCosto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AgregarCosto";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCosto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCosto;
        private System.Windows.Forms.Button btnAgregarCosto;
        private System.Windows.Forms.Button btnCancelarCosto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxNombreCosto;
        private System.Windows.Forms.ComboBox cbxUnidadCosto;
    }
}