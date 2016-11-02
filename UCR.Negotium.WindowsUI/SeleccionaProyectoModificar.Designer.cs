namespace UCR.Negotium.WindowsUI
{
    partial class SeleccionaProyectoModificar
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tbBuscarProyectos = new System.Windows.Forms.TextBox();
            this.dgvProyectos = new System.Windows.Forms.DataGridView();
            this.nombreProyecto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.proponente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.organizacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProyectos)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(102, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buscar:";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(132, 272);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(350, 272);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // tbBuscarProyectos
            // 
            this.tbBuscarProyectos.Location = new System.Drawing.Point(157, 36);
            this.tbBuscarProyectos.Name = "tbBuscarProyectos";
            this.tbBuscarProyectos.Size = new System.Drawing.Size(293, 20);
            this.tbBuscarProyectos.TabIndex = 4;
            this.tbBuscarProyectos.TextChanged += new System.EventHandler(this.tbBuscarProyectos_TextChanged);
            // 
            // dgvProyectos
            // 
            this.dgvProyectos.AllowUserToAddRows = false;
            this.dgvProyectos.AllowUserToDeleteRows = false;
            this.dgvProyectos.AllowUserToOrderColumns = true;
            this.dgvProyectos.AllowUserToResizeRows = false;
            this.dgvProyectos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProyectos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nombreProyecto,
            this.proponente,
            this.organizacion});
            this.dgvProyectos.Location = new System.Drawing.Point(38, 75);
            this.dgvProyectos.Name = "dgvProyectos";
            this.dgvProyectos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProyectos.Size = new System.Drawing.Size(457, 180);
            this.dgvProyectos.TabIndex = 5;
            this.dgvProyectos.SelectionChanged += new System.EventHandler(this.dgvProyectos_SelectionChanged);
            // 
            // nombreProyecto
            // 
            this.nombreProyecto.DataPropertyName = "nombreProyecto";
            this.nombreProyecto.HeaderText = "Nombre del Proyecto";
            this.nombreProyecto.Name = "nombreProyecto";
            this.nombreProyecto.Width = 150;
            // 
            // proponente
            // 
            this.proponente.DataPropertyName = "proponente";
            this.proponente.HeaderText = "Proponente";
            this.proponente.Name = "proponente";
            this.proponente.Width = 122;
            // 
            // organizacion
            // 
            this.organizacion.DataPropertyName = "organizacion";
            this.organizacion.HeaderText = "Organización";
            this.organizacion.Name = "organizacion";
            this.organizacion.Width = 142;
            // 
            // SeleccionaProyectoModificar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(540, 336);
            this.Controls.Add(this.dgvProyectos);
            this.Controls.Add(this.tbBuscarProyectos);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SeleccionaProyectoModificar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SeleccionaProyectoModificar";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProyectos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox tbBuscarProyectos;
        private System.Windows.Forms.DataGridView dgvProyectos;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreProyecto;
        private System.Windows.Forms.DataGridViewTextBoxColumn proponente;
        private System.Windows.Forms.DataGridViewTextBoxColumn organizacion;
    }
}