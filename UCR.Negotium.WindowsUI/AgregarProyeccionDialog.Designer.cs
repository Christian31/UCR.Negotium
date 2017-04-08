namespace UCR.Negotium.WindowsUI
{
    partial class AgregarProyeccionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgregarProyeccionDialog));
            this.btnAgregarProyeccion = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblNombreProyeccion = new System.Windows.Forms.Label();
            this.lblUnidadMedida = new System.Windows.Forms.Label();
            this.tbxNombreArticuloProyeccion = new System.Windows.Forms.TextBox();
            this.cbxUnidadProyeccion = new System.Windows.Forms.ComboBox();
            this.dgvProyecciones = new System.Windows.Forms.DataGridView();
            this.lblNombreProyeccionError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProyecciones)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAgregarProyeccion
            // 
            this.btnAgregarProyeccion.Image = ((System.Drawing.Image)(resources.GetObject("btnAgregarProyeccion.Image")));
            this.btnAgregarProyeccion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregarProyeccion.Location = new System.Drawing.Point(96, 412);
            this.btnAgregarProyeccion.Name = "btnAgregarProyeccion";
            this.btnAgregarProyeccion.Size = new System.Drawing.Size(126, 25);
            this.btnAgregarProyeccion.TabIndex = 0;
            this.btnAgregarProyeccion.Text = "Agregar proyeccion";
            this.btnAgregarProyeccion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAgregarProyeccion.UseVisualStyleBackColor = true;
            this.btnAgregarProyeccion.Click += new System.EventHandler(this.btnAgregarProyeccion_Click);
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(367, 412);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 25);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancelar";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblNombreProyeccion
            // 
            this.lblNombreProyeccion.AutoSize = true;
            this.lblNombreProyeccion.Location = new System.Drawing.Point(35, 36);
            this.lblNombreProyeccion.Name = "lblNombreProyeccion";
            this.lblNombreProyeccion.Size = new System.Drawing.Size(98, 13);
            this.lblNombreProyeccion.TabIndex = 2;
            this.lblNombreProyeccion.Text = "Nombre del articulo";
            // 
            // lblUnidadMedida
            // 
            this.lblUnidadMedida.AutoSize = true;
            this.lblUnidadMedida.Location = new System.Drawing.Point(294, 36);
            this.lblUnidadMedida.Name = "lblUnidadMedida";
            this.lblUnidadMedida.Size = new System.Drawing.Size(93, 13);
            this.lblUnidadMedida.TabIndex = 3;
            this.lblUnidadMedida.Text = "Unidad de medida";
            // 
            // tbxNombreArticuloProyeccion
            // 
            this.tbxNombreArticuloProyeccion.Location = new System.Drawing.Point(131, 33);
            this.tbxNombreArticuloProyeccion.Name = "tbxNombreArticuloProyeccion";
            this.tbxNombreArticuloProyeccion.Size = new System.Drawing.Size(142, 20);
            this.tbxNombreArticuloProyeccion.TabIndex = 4;
            // 
            // cbxUnidadProyeccion
            // 
            this.cbxUnidadProyeccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUnidadProyeccion.FormattingEnabled = true;
            this.cbxUnidadProyeccion.Location = new System.Drawing.Point(386, 33);
            this.cbxUnidadProyeccion.Name = "cbxUnidadProyeccion";
            this.cbxUnidadProyeccion.Size = new System.Drawing.Size(121, 21);
            this.cbxUnidadProyeccion.TabIndex = 6;
            // 
            // dgvProyecciones
            // 
            this.dgvProyecciones.AllowUserToAddRows = false;
            this.dgvProyecciones.AllowUserToDeleteRows = false;
            this.dgvProyecciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProyecciones.Location = new System.Drawing.Point(96, 85);
            this.dgvProyecciones.Name = "dgvProyecciones";
            this.dgvProyecciones.Size = new System.Drawing.Size(346, 307);
            this.dgvProyecciones.TabIndex = 7;
            this.dgvProyecciones.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProyecciones_CellValueChanged);
            // 
            // lblNombreProyeccionError
            // 
            this.lblNombreProyeccionError.AutoSize = true;
            this.lblNombreProyeccionError.ForeColor = System.Drawing.Color.Red;
            this.lblNombreProyeccionError.Location = new System.Drawing.Point(274, 37);
            this.lblNombreProyeccionError.Name = "lblNombreProyeccionError";
            this.lblNombreProyeccionError.Size = new System.Drawing.Size(11, 13);
            this.lblNombreProyeccionError.TabIndex = 17;
            this.lblNombreProyeccionError.Text = "*";
            // 
            // AgregarProyeccionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(552, 467);
            this.Controls.Add(this.lblNombreProyeccionError);
            this.Controls.Add(this.dgvProyecciones);
            this.Controls.Add(this.cbxUnidadProyeccion);
            this.Controls.Add(this.tbxNombreArticuloProyeccion);
            this.Controls.Add(this.lblUnidadMedida);
            this.Controls.Add(this.lblNombreProyeccion);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnAgregarProyeccion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AgregarProyeccionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AgregarProyeccion";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProyecciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAgregarProyeccion;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblNombreProyeccion;
        private System.Windows.Forms.Label lblUnidadMedida;
        private System.Windows.Forms.TextBox tbxNombreArticuloProyeccion;
        private System.Windows.Forms.ComboBox cbxUnidadProyeccion;
        private System.Windows.Forms.DataGridView dgvProyecciones;
        private System.Windows.Forms.Label lblNombreProyeccionError;
    }
}