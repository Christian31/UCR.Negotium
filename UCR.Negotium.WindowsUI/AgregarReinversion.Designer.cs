using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    partial class AgregarReinversion : Form
    {
        private Proyecto proyecto;

        private Evaluador evaluador;

        private List<RequerimientoReinversion> requerimientoReinversionList;

        private IContainer components = null;

        private DataGridView dgvAgregaReinversion;

        private Button button1;

        private Button button2;

        private DataGridViewTextBoxColumn Codigo;

        private DataGridViewTextBoxColumn descripcion_requerimiento;

        private DataGridViewTextBoxColumn cantidad;

        private DataGridViewTextBoxColumn costoUnitario;

        private DataGridViewTextBoxColumn vidaUtil;

        private DataGridViewCheckBoxColumn Agregar;

        public AgregarReinversion(Evaluador evaluador, Proyecto proyecto)
        {
            this.InitializeComponent();
            this.proyecto = proyecto;
            this.LlenaDgvReinversion();
            this.evaluador = evaluador;
            this.requerimientoReinversionList = new List<RequerimientoReinversion>();
            this.requerimientoReinversionList = proyecto.RequerimientosReinversion;
        }

        private void LlenaDgvReinversion()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("RequerimientReinversion");
            ds.Tables["RequerimientReinversion"].Columns.Add("Codigo", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("Descripcion", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("Cantidad", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("CostoUnitario", Type.GetType("System.String"));
            ds.Tables["RequerimientReinversion"].Columns.Add("VidaUtil", Type.GetType("System.String"));
            foreach (RequerimientoInversion requerimiento in this.proyecto.RequerimientosInversion)
            {
                if (requerimiento.Depreciable) { 
                    DataRow row = ds.Tables["RequerimientReinversion"].NewRow();
                    row["Codigo"] = requerimiento.CodRequerimientoInversion;
                    row["Descripcion"] = requerimiento.DescripcionRequerimiento;
                    row["Cantidad"] = requerimiento.Cantidad;
                    row["CostoUnitario"] = requerimiento.CostoUnitario;
                    row["VidaUtil"] = requerimiento.VidaUtil;
                    ds.Tables["RequerimientReinversion"].Rows.Add(row);
                }
            }
            DataTable dtRequerimientos = ds.Tables["RequerimientReinversion"];
            this.dgvAgregaReinversion.DataSource = dtRequerimientos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dgvAgregaReinversion.RowCount; i++)
                {
                    bool flag = Convert.ToBoolean(this.dgvAgregaReinversion.Rows[i].Cells[0].Value);
                    if (flag)
                    {
                        foreach (RequerimientoInversion requerimiento in this.proyecto.RequerimientosInversion)
                        {
                            bool flag2 = requerimiento.CodRequerimientoInversion == int.Parse(this.dgvAgregaReinversion.Rows[i].Cells[1].Value.ToString());
                            if (flag2)
                            {
                                RequerimientoReinversion requerimientoReinversion = new RequerimientoReinversion();
                                requerimientoReinversion.Cantidad = requerimiento.Cantidad;
                                requerimientoReinversion.CostoUnitario = requerimiento.CostoUnitario;
                                requerimientoReinversion.Depreciable = requerimiento.Depreciable;
                                requerimientoReinversion.DescripcionRequerimiento = requerimiento.DescripcionRequerimiento;
                                this.requerimientoReinversionList.Add(requerimientoReinversion);
                            }
                        }
                    }
                }
                this.proyecto.RequerimientosReinversion = this.requerimientoReinversionList;
                new RegistrarProyecto(this.evaluador, this.proyecto)
                {
                    MdiParent = base.MdiParent
                }.Show();
                base.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgregarReinversion));
            this.dgvAgregaReinversion = new System.Windows.Forms.DataGridView();
            this.Codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcion_requerimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costoUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vidaUtil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Agregar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgregaReinversion)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAgregaReinversion
            // 
            this.dgvAgregaReinversion.AllowUserToAddRows = false;
            this.dgvAgregaReinversion.AllowUserToDeleteRows = false;
            this.dgvAgregaReinversion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgregaReinversion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Codigo,
            this.descripcion_requerimiento,
            this.cantidad,
            this.costoUnitario,
            this.vidaUtil,
            this.Agregar});
            this.dgvAgregaReinversion.Location = new System.Drawing.Point(22, 70);
            this.dgvAgregaReinversion.Name = "dgvAgregaReinversion";
            this.dgvAgregaReinversion.Size = new System.Drawing.Size(720, 179);
            this.dgvAgregaReinversion.TabIndex = 0;
            // 
            // Codigo
            // 
            this.Codigo.DataPropertyName = "Codigo";
            this.Codigo.HeaderText = "Código";
            this.Codigo.Name = "Codigo";
            this.Codigo.ReadOnly = true;
            this.Codigo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // descripcion_requerimiento
            // 
            this.descripcion_requerimiento.DataPropertyName = "Descripcion";
            this.descripcion_requerimiento.HeaderText = "Descripción";
            this.descripcion_requerimiento.Name = "descripcion_requerimiento";
            this.descripcion_requerimiento.ReadOnly = true;
            this.descripcion_requerimiento.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.descripcion_requerimiento.Width = 340;
            // 
            // cantidad
            // 
            this.cantidad.DataPropertyName = "Cantidad";
            this.cantidad.HeaderText = "Cantidad";
            this.cantidad.Name = "cantidad";
            this.cantidad.ReadOnly = true;
            this.cantidad.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cantidad.Width = 60;
            // 
            // costoUnitario
            // 
            this.costoUnitario.DataPropertyName = "CostoUnitario";
            this.costoUnitario.HeaderText = "Costo Unitario";
            this.costoUnitario.Name = "costoUnitario";
            this.costoUnitario.ReadOnly = true;
            this.costoUnitario.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.costoUnitario.Width = 75;
            // 
            // vidaUtil
            // 
            this.vidaUtil.DataPropertyName = "VidaUtil";
            this.vidaUtil.HeaderText = "Vida útil";
            this.vidaUtil.Name = "vidaUtil";
            this.vidaUtil.ReadOnly = true;
            this.vidaUtil.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vidaUtil.Width = 50;
            // 
            // Agregar
            // 
            this.Agregar.HeaderText = "Agregar";
            this.Agregar.Name = "Agregar";
            this.Agregar.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Agregar.Width = 50;
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(210, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Agregar seleccionados";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(496, 280);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancelar";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(295, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Lista de Inversiones Depreciables";
            // 
            // AgregarReinversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(759, 356);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgvAgregaReinversion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AgregarReinversion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AgregarReinversion";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgregaReinversion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label label1;
    }
}