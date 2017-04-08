namespace UCR.Negotium.WindowsUI
{
    partial class GestionCrecimientosOfertaDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GestionCrecimientosOfertaDialog));
            this.dgvCrecimientosOferta = new System.Windows.Forms.DataGridView();
            this.btnGuardarCrecimientos = new System.Windows.Forms.Button();
            this.btnCancelarCrecimientos = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCrecimientosOferta)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCrecimientosOferta
            // 
            this.dgvCrecimientosOferta.AllowUserToAddRows = false;
            this.dgvCrecimientosOferta.AllowUserToDeleteRows = false;
            this.dgvCrecimientosOferta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCrecimientosOferta.Location = new System.Drawing.Point(41, 56);
            this.dgvCrecimientosOferta.Name = "dgvCrecimientosOferta";
            this.dgvCrecimientosOferta.Size = new System.Drawing.Size(294, 312);
            this.dgvCrecimientosOferta.TabIndex = 0;
            this.dgvCrecimientosOferta.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCrecimientosOferta_CellValueChanged);
            // 
            // btnGuardarCrecimientos
            // 
            this.btnGuardarCrecimientos.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardarCrecimientos.Image")));
            this.btnGuardarCrecimientos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardarCrecimientos.Location = new System.Drawing.Point(41, 386);
            this.btnGuardarCrecimientos.Name = "btnGuardarCrecimientos";
            this.btnGuardarCrecimientos.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarCrecimientos.TabIndex = 1;
            this.btnGuardarCrecimientos.Text = "Guardar";
            this.btnGuardarCrecimientos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardarCrecimientos.UseVisualStyleBackColor = true;
            this.btnGuardarCrecimientos.Click += new System.EventHandler(this.btnGuardarCrecimientos_Click);
            // 
            // btnCancelarCrecimientos
            // 
            this.btnCancelarCrecimientos.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelarCrecimientos.Image")));
            this.btnCancelarCrecimientos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelarCrecimientos.Location = new System.Drawing.Point(268, 386);
            this.btnCancelarCrecimientos.Name = "btnCancelarCrecimientos";
            this.btnCancelarCrecimientos.Size = new System.Drawing.Size(67, 23);
            this.btnCancelarCrecimientos.TabIndex = 2;
            this.btnCancelarCrecimientos.Text = "Volver";
            this.btnCancelarCrecimientos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelarCrecimientos.UseVisualStyleBackColor = true;
            this.btnCancelarCrecimientos.Click += new System.EventHandler(this.btnCancelarCrecimientos_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Crecimiento Anual de Oferta";
            // 
            // GestionCrecimientosOfertaDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(380, 452);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancelarCrecimientos);
            this.Controls.Add(this.btnGuardarCrecimientos);
            this.Controls.Add(this.dgvCrecimientosOferta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GestionCrecimientosOfertaDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion de Crecimiento Anual de Oferta";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCrecimientosOferta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCrecimientosOferta;
        private System.Windows.Forms.Button btnGuardarCrecimientos;
        private System.Windows.Forms.Button btnCancelarCrecimientos;
        private System.Windows.Forms.Label label1;
    }
}