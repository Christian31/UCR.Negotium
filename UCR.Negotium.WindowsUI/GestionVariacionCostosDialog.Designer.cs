namespace UCR.Negotium.WindowsUI
{
    partial class GestionVariacionCostosDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GestionVariacionCostosDialog));
            this.dgvVariacionCostos = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGuardarVariacion = new System.Windows.Forms.Button();
            this.btnVolverVariacionAnual = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariacionCostos)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvVariacionCostos
            // 
            this.dgvVariacionCostos.AllowUserToAddRows = false;
            this.dgvVariacionCostos.AllowUserToDeleteRows = false;
            this.dgvVariacionCostos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariacionCostos.Location = new System.Drawing.Point(41, 56);
            this.dgvVariacionCostos.Name = "dgvVariacionCostos";
            this.dgvVariacionCostos.Size = new System.Drawing.Size(294, 312);
            this.dgvVariacionCostos.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Variación Anual de Costos";
            // 
            // btnGuardarVariacion
            // 
            this.btnGuardarVariacion.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardarVariacion.Image")));
            this.btnGuardarVariacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardarVariacion.Location = new System.Drawing.Point(41, 386);
            this.btnGuardarVariacion.Name = "btnGuardarVariacion";
            this.btnGuardarVariacion.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarVariacion.TabIndex = 2;
            this.btnGuardarVariacion.Text = "Guardar";
            this.btnGuardarVariacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardarVariacion.UseVisualStyleBackColor = true;
            this.btnGuardarVariacion.Click += new System.EventHandler(this.btnGuardarVariacion_Click);
            // 
            // btnVolverVariacionAnual
            // 
            this.btnVolverVariacionAnual.Image = ((System.Drawing.Image)(resources.GetObject("btnVolverVariacionAnual.Image")));
            this.btnVolverVariacionAnual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVolverVariacionAnual.Location = new System.Drawing.Point(268, 386);
            this.btnVolverVariacionAnual.Name = "btnVolverVariacionAnual";
            this.btnVolverVariacionAnual.Size = new System.Drawing.Size(67, 23);
            this.btnVolverVariacionAnual.TabIndex = 3;
            this.btnVolverVariacionAnual.Text = "Volver";
            this.btnVolverVariacionAnual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVolverVariacionAnual.UseVisualStyleBackColor = true;
            this.btnVolverVariacionAnual.Click += new System.EventHandler(this.btnVolverVariacionAnual_Click);
            // 
            // GestionVariacionCostos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(380, 452);
            this.Controls.Add(this.btnVolverVariacionAnual);
            this.Controls.Add(this.btnGuardarVariacion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvVariacionCostos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GestionVariacionCostos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion de Variación Anual de Costos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariacionCostos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVariacionCostos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGuardarVariacion;
        private System.Windows.Forms.Button btnVolverVariacionAnual;
    }
}