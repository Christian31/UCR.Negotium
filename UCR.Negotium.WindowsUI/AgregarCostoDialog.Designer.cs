namespace UCR.Negotium.WindowsUI
{
    partial class AgregarCostoDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgregarCostoDialog));
            this.dgvCosto = new System.Windows.Forms.DataGridView();
            this.btnAgregarCosto = new System.Windows.Forms.Button();
            this.btnCancelarCosto = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxNombreCosto = new System.Windows.Forms.TextBox();
            this.cbxUnidadCosto = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxCategoriasCosto = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxAnoInicialCosto = new System.Windows.Forms.ComboBox();
            this.lblNombreCostoError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCosto)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCosto
            // 
            this.dgvCosto.AllowDrop = true;
            this.dgvCosto.AllowUserToAddRows = false;
            this.dgvCosto.AllowUserToDeleteRows = false;
            this.dgvCosto.AllowUserToResizeColumns = false;
            this.dgvCosto.AllowUserToResizeRows = false;
            this.dgvCosto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCosto.Location = new System.Drawing.Point(101, 122);
            this.dgvCosto.Name = "dgvCosto";
            this.dgvCosto.Size = new System.Drawing.Size(346, 307);
            this.dgvCosto.TabIndex = 0;
            this.dgvCosto.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCosto_CellValueChanged);
            // 
            // btnAgregarCosto
            // 
            this.btnAgregarCosto.Image = ((System.Drawing.Image)(resources.GetObject("btnAgregarCosto.Image")));
            this.btnAgregarCosto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregarCosto.Location = new System.Drawing.Point(101, 452);
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
            this.btnCancelarCosto.Location = new System.Drawing.Point(372, 452);
            this.btnCancelarCosto.Name = "btnCancelarCosto";
            this.btnCancelarCosto.Size = new System.Drawing.Size(75, 23);
            this.btnCancelarCosto.TabIndex = 2;
            this.btnCancelarCosto.Text = "Cancelar";
            this.btnCancelarCosto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelarCosto.UseVisualStyleBackColor = true;
            this.btnCancelarCosto.Click += new System.EventHandler(this.btnCancelarCosto_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nombre del Costo:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Unidad de medida:";
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
            this.cbxUnidadCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUnidadCosto.FormattingEnabled = true;
            this.cbxUnidadCosto.Location = new System.Drawing.Point(392, 33);
            this.cbxUnidadCosto.Name = "cbxUnidadCosto";
            this.cbxUnidadCosto.Size = new System.Drawing.Size(121, 21);
            this.cbxUnidadCosto.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(70, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Año inicial:";
            // 
            // cbxCategoriasCosto
            // 
            this.cbxCategoriasCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCategoriasCosto.FormattingEnabled = true;
            this.cbxCategoriasCosto.Items.AddRange(new object[] {
            "Operativos",
            "Administrativos"});
            this.cbxCategoriasCosto.Location = new System.Drawing.Point(392, 76);
            this.cbxCategoriasCosto.Name = "cbxCategoriasCosto";
            this.cbxCategoriasCosto.Size = new System.Drawing.Size(121, 21);
            this.cbxCategoriasCosto.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(335, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Categoria:";
            // 
            // cbxAnoInicialCosto
            // 
            this.cbxAnoInicialCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAnoInicialCosto.FormattingEnabled = true;
            this.cbxAnoInicialCosto.Location = new System.Drawing.Point(131, 77);
            this.cbxAnoInicialCosto.Name = "cbxAnoInicialCosto";
            this.cbxAnoInicialCosto.Size = new System.Drawing.Size(142, 21);
            this.cbxAnoInicialCosto.TabIndex = 15;
            // 
            // lblNombreCostoError
            // 
            this.lblNombreCostoError.AutoSize = true;
            this.lblNombreCostoError.ForeColor = System.Drawing.Color.Red;
            this.lblNombreCostoError.Location = new System.Drawing.Point(275, 38);
            this.lblNombreCostoError.Name = "lblNombreCostoError";
            this.lblNombreCostoError.Size = new System.Drawing.Size(11, 13);
            this.lblNombreCostoError.TabIndex = 16;
            this.lblNombreCostoError.Text = "*";
            // 
            // AgregarCostoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(552, 505);
            this.Controls.Add(this.lblNombreCostoError);
            this.Controls.Add(this.cbxAnoInicialCosto);
            this.Controls.Add(this.cbxCategoriasCosto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxUnidadCosto);
            this.Controls.Add(this.tbxNombreCosto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancelarCosto);
            this.Controls.Add(this.btnAgregarCosto);
            this.Controls.Add(this.dgvCosto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AgregarCostoDialog";
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxCategoriasCosto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxAnoInicialCosto;
        private System.Windows.Forms.Label lblNombreCostoError;
    }
}