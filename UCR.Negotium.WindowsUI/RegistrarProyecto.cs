//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class RegistrarProyecto : Form
    {
        //Variables globales
        bool mostroMensaje = false, mostrarMensajeSeguridad=true;
        int idProvinciaSeleccionada = 0, idCantonSeleccionado = 0, idDistritoSeleccionado = 0;
        bool segundaSeleccionProvincia = false, segundaSeleccionCanton = false, segundaSeleccionDistrito = false;
        Proyecto proyecto;
        Evaluador evaluador;
        
        //Constructor sobrecargado que solo recibe al evaluador que esta logeado como parametro
        public RegistrarProyecto(Evaluador evaluador)
        {
            this.evaluador = evaluador;
            InitializeComponent();
            LlenaComboProvincia();
            LlenaComboCantones();
            LlenarComboDistritos();
            LlenarComboUnidadMedida();
            LlenaComboTipoOrganizacion();
            lblNombreEvaluador.Text = evaluador.Nombre;
            lblCedulaEvaluador.Text = evaluador.NumIdentificacion;
            InformacionToolTip();
            mostrarMensajeSeguridad = true;
        }

        //constructor sobrecargado que se utiliza para abrir proyectos con informacion en memoria volatil
        public RegistrarProyecto(Evaluador evaluador, Proyecto proyecto)
        {
            this.proyecto = proyecto;
            this.evaluador = evaluador;
            InitializeComponent();
            LlenaComboProvincia();
            LlenaComboCantones();
            LlenarComboDistritos();
            LlenarComboUnidadMedida();
            LlenaComboTipoOrganizacion();
            lblNombreEvaluador.Text = evaluador.Nombre;
            lblCedulaEvaluador.Text = evaluador.NumIdentificacion;
            InformacionToolTip();
            LlenaInformacionProyecto();
            LlenaInformacionProponente();
            LlenaInformacionOrganizacion();
            mostrarMensajeSeguridad = true;
            LlenaDgvInversiones();
            LlenaDgvReinversiones();
            LlenaValoresTotalesReinversiones();
            LlenaDgvTotalesReinversiones();
            LlenaDgvProyeccionesVentas();
            LlenaDgvIngresosGenerados();
            LlenaDgvCostos();
            LlenaDgvCostosGenerados();
            LlenaDgvCapitalTrabajo();
            LlenaFooter();
        }

        
        /******************************************INICIO DE VALIDACIONES DE CHECKBOX*******************************************************/
        // El siguiente metodo es para cuando se realiza una accion en el checkbox primero desmarque la casilla
        // del otro check box y luego seleccione la casilla que se esta presionando
        private void chbConIngreso_CheckedChanged(object sender, EventArgs e)
        {
            if ((chbSinIngreso.Checked) )
            {
                //Este if evita que se encicle las validaciones ya que cada vez que se hace un
                //chechbox.Checked se ejecuta el evento de ese checkbox
                if (chbConIngreso.Checked)
                    chbConIngreso.Checked = false;
                chbSinIngreso.Checked = false;
                chbConIngreso.Checked = true;
            }//if
        }

        // El siguiente metodo es para cuando se realiza una accion en el checkbox primero desmarque la casilla
        // del otro check box y luego seleccione la casilla que se esta presionando
        private void chbSinIngreso_CheckedChanged(object sender, EventArgs e)
        {
            if ((chbConIngreso.Checked) )
            {
                //Este if evita que se encicle las validaciones ya que cada vez que se hace un
                //chechbox.Checked se ejecuta el evento de ese checkbox
                if (chbSinIngreso.Checked)
                    chbSinIngreso.Checked = false;
                chbConIngreso.Checked = false;
                chbSinIngreso.Checked = true;
            }//if
        }

        private void chbSiPagaImpuesto_CheckedChanged(object sender, EventArgs e)
        {
            if ((chbNoPagaImpuesto.Checked))
            {
                //Este if evita que se encicle las validaciones ya que cada vez que se hace un
                //chechbox.Checked se ejecuta el evento de ese checkbox
                if (chbSiPagaImpuesto.Checked)
                    chbSiPagaImpuesto.Checked = false;
                chbNoPagaImpuesto.Checked = false;
                chbSiPagaImpuesto.Checked = true;
            }//if
        }

        private void chbNoPagaImpuesto_CheckedChanged(object sender, EventArgs e)
        {
            if ((chbSiPagaImpuesto.Checked))
            {
                //Este if evita que se encicle las validaciones ya que cada vez que se hace un
                //chechbox.Checked se ejecuta el evento de ese checkbox
                if (chbNoPagaImpuesto.Checked)
                    chbNoPagaImpuesto.Checked = false;
                chbSiPagaImpuesto.Checked = false;
                chbNoPagaImpuesto.Checked = true;
            }//if
        }

        private void chbFemenino_CheckedChanged(object sender, EventArgs e)
        {
            if ((chbMasculino.Checked))
            {
                //Este if evita que se encicle las validaciones ya que cada vez que se hace un
                //chechbox.Checked se ejecuta el evento de ese checkbox
                if (chbFemenino.Checked)
                    chbFemenino.Checked = false;
                chbMasculino.Checked = false;
                chbFemenino.Checked = true;
            }//if
        }

        private void chbMasculino_CheckedChanged(object sender, EventArgs e)
        {
            if ((chbFemenino.Checked))
            {
                //Este if evita que se encicle las validaciones ya que cada vez que se hace un
                //chechbox.Checked se ejecuta el evento de ese checkbox
                if (chbMasculino.Checked)
                    chbMasculino.Checked = false;
                chbFemenino.Checked = false;
                chbMasculino.Checked = true;
            }//if
        }
        /******************************************FIN DE VALIDACIONES DE CHECKBOX*******************************************************/


        /**********************************************INICIO DE ACCIONES ANTE EVENTOS******************************************/

        //Esta acción se ejecuta cuando se cambia entre pestañas del tabcontrol la funcionalidad de este metodo
        //va a ser validar que la información del tab actual este guardada
        private void tbxRegistrarProyecto_Selected(object sender, TabControlEventArgs e)
        {
            //Primero asigna el indice de la pestaña que esta seleccionada luego se ejecuta las validaciones
            //dependiendo de la pestaña que corresponda
            int indice = tbxRegistrarProyecto.SelectedIndex;
            switch (indice)
            {
                case 1:
                    if (txbNombreProyecto.Text.Trim().Equals(""))
                    {
                        if (mostroMensaje == false)
                        {
                            mostroMensaje = true;
                            MessageBox.Show("Por favor ingrese todos los datos para poder avanzar a la siguiente pestaña",
                            "Datos vacios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }//if
                        tbxRegistrarProyecto.SelectedIndex = 0;
                    }//if
                    break;
                case 2:
                    
                    if ((txbNombreOrganizacion.Text.Trim().Equals("")) || (txbCedulaJuridica.Text.Trim().Equals("")))
                    {
                        if (mostroMensaje == false)
                        {
                            mostroMensaje = true;
                            MessageBox.Show("Por favor ingrese todos los datos para poder avanzar a la siguiente pestaña",
                            "Datos vacios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }//if
                        tbxRegistrarProyecto.SelectedIndex = 1;
                    }//if
                    break;

                case 3:

                    if (dgvInversiones.Rows.Count == 0)
                    {
                        if (mostroMensaje == false)
                        {
                            mostroMensaje = true;
                            MessageBox.Show("Por favor ingrese todos los datos para poder avanzar a la siguiente pestaña",
                            "Datos vacios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }//if
                        tbxRegistrarProyecto.SelectedIndex = 1;
                    }//if
                    break;

                case 4:

                    if ((txbJustificacionMercado.Text.Trim().Equals("")) || (txbCaracterizacionBienServicio.Text.Trim().Equals("")))
                    {
                        if (mostroMensaje == false)
                        {
                            mostroMensaje = true;
                            MessageBox.Show("Por favor ingrese todos los datos para poder avanzar a la siguiente pestaña",
                            "Datos vacios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }//if
                        tbxRegistrarProyecto.SelectedIndex = 1;
                    }//if
                    break;
                default:
                    mostroMensaje = false;
                    break;
            }//switch
            mostroMensaje = false;
        }

        private void llbCambiarNombreProyecto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxRegistrarProyecto.SelectedIndex = 0;
        }

        private void llbCambiarTipoProyecto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxRegistrarProyecto.SelectedIndex = 0;
        }

        private void llbCambiarObjetoInteres_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxRegistrarProyecto.SelectedIndex = 0;
        }

        private void llbOrganizacionProponente_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxRegistrarProyecto.SelectedIndex = 1;
        }

        private void llbTelefonoProponente_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxRegistrarProyecto.SelectedIndex = 1;
        }

        private void llbNombreProponente_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxRegistrarProyecto.SelectedIndex = 1;
        }

        //La funcionalidad de este metodo muestra un mensaje de confirmación por si presiona el boton de
        //cerrar ventana erroneamente y así se evita perder información ya ingresada pero que esta en
        //memoria volatil
        private void RegistrarProyecto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mostrarMensajeSeguridad==true)
            {
                if (MessageBox.Show("¿Seguro que desea salir?", "Salir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            if (proyecto == null)
            {
                InsertarProyecto();
            }
            else
            {
                ActualizarProyecto();
            }
            
        }

        //Este boton guarda la información de la organizacion proponente y del proponente
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (proyecto.Proponente.NumIdentificacion == "-1")
            {
                InsertarOrganizacionProponente();
            }//if
            else
            {
                ActualizarOrganizacionProponente();
                ActualizarProponente();
                LlenaFooter();
                MessageBox.Show("Organizacion y proponente actualizados con éxito",
                            "Actualizados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }//else
        }

        private void btnGuardar3_Click(object sender, EventArgs e)
        {
            ActualizarProyecto();
        }

        //Evento que se ejecuta cuando el valor de una celda del dgvInversiones cambia
        private void dgvInversiones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try {
                //Basicamente valida si se cambio el valor de la cantidad o del costo unitario de una
                //inversion, si esto ocurre actualiza el valor del total
                if (dgvInversiones.RowCount > 1)
                {
                    if (this.dgvInversiones.Columns[e.ColumnIndex].Name == "Cantidad" ||
                        this.dgvInversiones.Columns[e.ColumnIndex].Name == "CostoUnitario")
                    {
                        int cantidad = 0;
                        int costoUnitario = 0;
                        if ((this.dgvInversiones.Rows[e.RowIndex].Cells["cantidad"].Value != null) ||
                            (this.dgvInversiones.Rows[e.RowIndex].Cells["cantidad"].Value.ToString() != ""))
                        {
                            cantidad = int.Parse(this.dgvInversiones.Rows[e.RowIndex].Cells["cantidad"].Value.ToString());
                        }//if
                        if ((this.dgvInversiones.Rows[e.RowIndex].Cells["costoUnitario"].Value != null) ||
                            (this.dgvInversiones.Rows[e.RowIndex].Cells["costoUnitario"].Value.ToString() != ""))
                        {
                            costoUnitario = int.Parse(this.dgvInversiones.Rows[e.RowIndex].Cells["costoUnitario"].Value.ToString());
                        }//if
                        this.dgvInversiones.Rows[e.RowIndex].Cells["Subtotal"].Value = cantidad * costoUnitario;
                        double total = 0;
                        for (int i = 0; i < dgvInversiones.RowCount - 1; i++)
                        {
                            if ((this.dgvInversiones.Rows[i].Cells["Subtotal"].Value != null) ||
                                (this.dgvInversiones.Rows[i].Cells["Subtotal"].Value.ToString() != ""))
                            {
                                total += float.Parse(this.dgvInversiones.Rows[i].Cells["Subtotal"].Value.ToString());
                            }//if
                        }//for
                        lblTotalInversiones.Text = total.ToString("#,##0");
                    }//if
                }//if
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void btnGuardarInversion_Click(object sender, EventArgs e)
        {
            //la siguiente variable valida si la insersión se realizó o no dependiendo de esa variable muestra
            //alguno de los mensajes de error
            bool validaInsersion = false;

            List<RequerimientoInversion> listaRequerimientoInversion = new List<RequerimientoInversion>();
            for (int i = 0; i < dgvInversiones.RowCount; i++)
            {
                //Se valida que el dgvReinversiones tenga una unidadmedida agregada falta validar lo demás
                if (this.dgvInversiones.Rows[i].Cells["UnidadMedida"].Value != null)
                {
                    try
                    {
                        RequerimientoInversion requerimientoInversiones = new RequerimientoInversion();
                        requerimientoInversiones.DescripcionRequerimiento = 
                            this.dgvInversiones.Rows[i].Cells["Descripcion"].Value.ToString();
                        requerimientoInversiones.Cantidad = 
                            Int32.Parse(this.dgvInversiones.Rows[i].Cells["Cantidad"].Value.ToString());
                        requerimientoInversiones.CostoUnitario = Convert.ToDouble(Int32.Parse(this.dgvInversiones.Rows[i].
                            Cells["CostoUnitario"].Value.ToString()));
                        // Se realiza una instancia del dgv combobox column para poder manipularlo
                        DataGridViewComboBoxColumn unidadMedidaColumn = 
                            dgvInversiones.Columns["UnidadMedida"] as DataGridViewComboBoxColumn;
                        DataTable dtUnidadMedida = (DataTable)unidadMedidaColumn.DataSource;
                        UnidadMedida unidadMedida = new UnidadMedida();
                        foreach (DataRow fila in dtUnidadMedida.Rows)
                        {
                            if (this.dgvInversiones.Rows[i].Cells["UnidadMedida"].Value.ToString() == fila["cod_unidad"].ToString())
                            {
                                unidadMedida.CodUnidad = Int32.Parse(fila["cod_unidad"].ToString());
                                unidadMedida.NombreUnidad = fila["nombre_unidad"].ToString();
                                break;
                            }//if
                        }//foreach
                        requerimientoInversiones.UnidadMedida = unidadMedida;
                        //Object value = this.dgvInversiones.Rows[i].Cells["Depreciable"].Value;
                        if(this.dgvInversiones.Rows[i].Cells["Depreciable"].Value.ToString().Equals(""))
                        {
                            requerimientoInversiones.Depreciable = false;
                        }
                        else
                        {
                            requerimientoInversiones.Depreciable = 
                                Convert.ToBoolean(this.dgvInversiones.Rows[i].Cells["Depreciable"].Value);
                        }

                        if (i > listaRequerimientoInversion.Count-1 && requerimientoInversiones.UnidadMedida.CodUnidad != 0) {
                            requerimientoInversiones.VidaUtil = Int32.Parse(this.dgvInversiones.Rows[i].Cells["VidaUtil"].Value.ToString());
                            RequerimientoInversionData requerimientosInversionData = new RequerimientoInversionData();
                            if (requerimientoInversiones.CodRequerimientoInversion == 0)
                            {
                                requerimientosInversionData.InsertarRequerimientosInvesion(requerimientoInversiones, this.proyecto.CodProyecto);
                                listaRequerimientoInversion.Add(requerimientoInversiones);
                                validaInsersion = true;
                            }
                            else
                            {
                                requerimientosInversionData.EditarRequerimientosInvesion(requerimientoInversiones, this.proyecto.CodProyecto);
                                validaInsersion = true;
                            }
                        }
                    }//try
                    catch(Exception ex)
                    {
                        validaInsersion = false;
                        Console.WriteLine(ex);
                    }//catch
                }//if
            }//for
            if (validaInsersion == true)
            {
                proyecto.RequerimientosInversion = listaRequerimientoInversion;
                dgvInversiones.Update();
                dgvInversiones.Refresh();

                MessageBox.Show("Inversión registrada con éxito",
                            "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }//if
            else
            {
                MessageBox.Show("La inversión no se ha podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//else
        }//btnGuardarInversion_Click

        private void button1_Click(object sender, EventArgs e)
        {
            mostrarMensajeSeguridad = false;
            AgregarReinversion agregarReinversion = new AgregarReinversion(evaluador, proyecto);
            agregarReinversion.MdiParent = this.MdiParent;
            agregarReinversion.Show();
            this.Close();
        }

        private void inversiones_Enter(object sender, EventArgs e)
        {
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            DataGridViewComboBoxColumn comboboxColumn = dgvInversiones.Columns["UnidadMedida"] as DataGridViewComboBoxColumn;
            comboboxColumn.DataSource = unidadMedidaData.GetUnidadesMedida();
            comboboxColumn.DisplayMember = "nombre_unidad";
            comboboxColumn.ValueMember = "cod_unidad";
        }

        private void dgvInversiones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            DataGridViewComboBoxColumn comboboxColumn = dgvInversiones.Columns["UnidadMedida"] as DataGridViewComboBoxColumn;
            comboboxColumn.DataSource = unidadMedidaData.GetUnidadesMedida();
            comboboxColumn.DisplayMember = "nombre_unidad";
            comboboxColumn.ValueMember = "cod_unidad";

            int autoincrement = 0;
            //EL siguiente foreach sirve para cargar la unidad medida al combobox del dgv correspondiente
            foreach (RequerimientoInversion requerimiento in this.proyecto.RequerimientosInversion)
            {
                if (autoincrement < dgvInversiones.RowCount)
                {
                    this.dgvInversiones.Rows[autoincrement].Cells["UnidadMedida"].Value
                        = requerimiento.UnidadMedida.NombreUnidad.ToString();
                    autoincrement++;
                }
            }//foreach
        }

        private void reinversiones_Enter(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvReinversiones.RowCount; i++)
            {
                int cantidad = 0;
                int costoUnitario = 0;
                if (this.dgvReinversiones.Rows[i].Cells["CantidadReinversion"].Value != null)
                {
                    cantidad = int.Parse(this.dgvReinversiones.Rows[i].Cells["CantidadReinversion"]
                        .Value.ToString());
                }//if
                if (this.dgvReinversiones.Rows[i].Cells["CostoUnitarioReinversion"].Value != null)
                {
                    costoUnitario = int.Parse(this.dgvReinversiones.Rows[i].Cells["CostoUnitarioReinversion"]
                        .Value.ToString());
                    this.dgvReinversiones.Rows[i].Cells["SubtotalReinversion"].Value = cantidad * costoUnitario;
                }//if
            }//for
            int autoincrement = 0;
            //EL siguiente foreach sirve para cargar el año de inversión al combobox del dgv correspondiente
            foreach (RequerimientoReinversion requerimiento in this.proyecto.RequerimientosReinversion)
            {
                this.dgvReinversiones.Rows[autoincrement].Cells["AnoReinversion"].Value
                        = requerimiento.AnoReinversion.ToString();
                autoincrement++;
            }//foreach

            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            DataGridViewComboBoxColumn comboboxColumn = dgvReinversiones.Columns["unidadMedidaRe"] as DataGridViewComboBoxColumn;
            comboboxColumn.DataSource = unidadMedidaData.GetUnidadesMedida();
            comboboxColumn.DisplayMember = "nombre_unidad";
            comboboxColumn.ValueMember = "cod_unidad";
        }

        private void dgvReinversiones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            DataTable dtUnidades = unidadMedidaData.GetUnidadesMedida();
            DataGridViewComboBoxColumn comboboxColumn = dgvReinversiones.Columns["unidadMedidaRe"] as DataGridViewComboBoxColumn;
            comboboxColumn.DataSource = dtUnidades;
            comboboxColumn.DisplayMember = "nombre_unidad";
            comboboxColumn.ValueMember = "cod_unidad";

            List<String> anosReinversion = new List<String>();
            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                int anoActual = proyecto.AnoInicial + i;
                anosReinversion.Add(anoActual.ToString());
            }//for

            DataGridViewComboBoxColumn anoInversionColumn = dgvReinversiones.Columns["AnoReinversion"]
                    as DataGridViewComboBoxColumn;
            anoInversionColumn.DataSource = anosReinversion;

            int autoincrement = 0;
            //EL siguiente foreach sirve para cargar la unidad medida al combobox del dgv correspondiente
            foreach (RequerimientoReinversion requerimiento in this.proyecto.RequerimientosReinversion)
            {
                if (autoincrement < dgvReinversiones.RowCount)
                {
                    this.dgvReinversiones.Rows[autoincrement].Cells["unidadMedidaRe"].Value
                        = requerimiento.UnidadMedida.NombreUnidad.ToString();

                    this.dgvReinversiones.Rows[autoincrement].Cells["AnoReinversion"].Value
                            = requerimiento.AnoReinversion.ToString();

                    autoincrement++;
                }
            }//foreach
            this.dgvReinversiones.Rows[dgvReinversiones.RowCount - 1].Cells["AnoReinversion"].Value = proyecto.AnoInicial + 1;
            this.dgvReinversiones.Rows[dgvReinversiones.RowCount - 1].Cells["unidadMedidaRe"].Value = dtUnidades.Rows[0][0];



        }

        private void dgvReinversiones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            LlenaValoresTotalesReinversiones();

            try
            {
                //Basicamente valida si se cambio el valor de la cantidad o del costo unitario de una
                //inversion, si esto ocurre actualiza el valor del total
                if (dgvReinversiones.RowCount > 1)
                {
                    if (this.dgvReinversiones.Columns[e.ColumnIndex].Name == "CantidadReinversion" ||
                        this.dgvReinversiones.Columns[e.ColumnIndex].Name == "CostoUnitarioReinversion")
                    {
                        Int32 cantidad = 0;
                        Int32 costoUnitario = 0;
                        if ((this.dgvReinversiones.Rows[e.RowIndex].Cells["CantidadReinversion"].Value != null) ||
                            (this.dgvReinversiones.Rows[e.RowIndex].Cells["CantidadReinversion"].Value.ToString() != ""))
                        {
                            cantidad = Convert.ToInt32(this.dgvReinversiones.Rows[e.RowIndex].Cells["CantidadReinversion"].Value.ToString());
                        }//if
                        if ((this.dgvReinversiones.Rows[e.RowIndex].Cells["CostoUnitarioReinversion"].Value != null) ||
                            (this.dgvReinversiones.Rows[e.RowIndex].Cells["CostoUnitarioReinversion"].Value.ToString() != ""))
                        {
                            costoUnitario = int.Parse(this.dgvReinversiones.Rows[e.RowIndex].Cells["CostoUnitarioReinversion"].Value.ToString());
                        }//if
                        this.dgvReinversiones.Rows[e.RowIndex].Cells["SubtotalReinversion"].Value = cantidad * costoUnitario;
                        
                    }//if
                }//if
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //Este metodo se ejecuta ante la accion de guardar requerimientos de reinversión
        private void btnGuardar5_Click(object sender, EventArgs e)
        {
            bool validaInsersion = false;
            List<RequerimientoReinversion> listaRequerimientoReinversion = new List<RequerimientoReinversion>();
            List<RequerimientoReinversion> listaRequerimientoReinversionPersistente = new List<RequerimientoReinversion>();
            RequerimientoReinversionData requereinvData = new RequerimientoReinversionData();
            listaRequerimientoReinversionPersistente = requereinvData.GetRequerimientosReinversion(this.proyecto.CodProyecto);
            Int32 tam = listaRequerimientoReinversionPersistente.Count;
            for (int i = 0; i < dgvReinversiones.RowCount; i++)
            {
                if (this.dgvReinversiones.Rows[i].Cells["AnoReinversion"].Value != null)
                {
                    try
                    {
                        RequerimientoReinversion requerimientoReinversion = new RequerimientoReinversion();
                        requerimientoReinversion.DescripcionRequerimiento =
                            this.dgvReinversiones.Rows[i].Cells["DescripcionReinversion"].Value.ToString();
                        requerimientoReinversion.Cantidad =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["CantidadReinversion"].Value.ToString());
                        requerimientoReinversion.CostoUnitario = 
                            Convert.ToDouble(Int32.Parse(this.dgvReinversiones.Rows[i].
                            Cells["CostoUnitarioReinversion"].Value.ToString()));

                        requerimientoReinversion.VidaUtil =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["vidaUtilRe"].Value.ToString());
                        requerimientoReinversion.AnoReinversion =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["AnoReinversion"].Value.ToString());
                        requerimientoReinversion.UnidadMedida.CodUnidad =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["unidadMedidaRe"].Value.ToString());
                        requerimientoReinversion.UnidadMedida.NombreUnidad =
                            this.dgvReinversiones.Rows[i].Cells["unidadMedidaRe"].Value.ToString();

                        if (this.dgvReinversiones.Rows[i].Cells["DepreciableReinversion"].Value.ToString().Equals(""))
                        {
                            requerimientoReinversion.Depreciable = false;
                        }
                        else
                        {
                            requerimientoReinversion.Depreciable =
                                Convert.ToBoolean(this.dgvReinversiones.Rows[i].Cells["DepreciableReinversion"].Value);
                        }

                        listaRequerimientoReinversion.Add(requerimientoReinversion);
                        if (tam <= i) {
                            proyecto.RequerimientosReinversion.Add(requereinvData.InsertarRequerimientosReinversion(requerimientoReinversion, this.proyecto.CodProyecto));
                            validaInsersion = true;
                        }

        }//try
                    catch (Exception ex)
                    {
                        validaInsersion = false;
                        Console.WriteLine(ex);
                    }//catch
                }//if
            }//for


            if (validaInsersion == true)
            {
                LlenaValoresTotalesReinversiones();
                proyecto.RequerimientosReinversion = listaRequerimientoReinversion;
                    MessageBox.Show("Reinversión registrada con éxito",
                                "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }//if
            else
            {
                MessageBox.Show("La reinversión no se ha podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//else
        }//btnGuardar5_Click

        /*******************************************FIN DE ACCIONES ANTE EVENTOS*****************************************************/

        /****************************************METODOS PARA CARGAR COMBOBOX*************************************/
        //Los siguientes metodos se encargar de llenar los combobox
        private void LlenaComboProvincia()
        {
            ProvinciaData provinciaData = new ProvinciaData();
            cbxProvincia.DataSource = provinciaData.GetProvincias();
            cbxProvincia.DisplayMember = "nombre_provincia";
            cbxProvincia.ValueMember = "cod_provincia";
            idProvinciaSeleccionada = Int32.Parse(cbxProvincia.SelectedValue.ToString());
        }//LlenaComboProvincia

        private void LlenaComboCantones()
        {
            CantonData cantonData = new CantonData();
            cbxCanton.DataSource = cantonData.GetCantonesPorProvincia(idProvinciaSeleccionada);
            cbxCanton.DisplayMember = "nombre_canton";
            cbxCanton.ValueMember = "cod_canton";
            idCantonSeleccionado = Int32.Parse(cbxCanton.SelectedValue.ToString());
        }

        private void LlenarComboDistritos()
        {
            DistritoData distritoData = new DistritoData();
            cbxDistrito.DataSource = distritoData.GetDistritosPorCanton(idCantonSeleccionado);
            cbxDistrito.DisplayMember = "nombre_distrito";
            cbxDistrito.ValueMember = "cod_distrito";
            idDistritoSeleccionado = Int32.Parse(cbxDistrito.SelectedValue.ToString());

        }//LlenarComboDistritos

        private void LlenarComboUnidadMedida()
        {
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            cbxUnidadMedida.DataSource = unidadMedidaData.GetUnidadesMedida();
            cbxUnidadMedida.DisplayMember = "nombre_unidad";
            cbxUnidadMedida.ValueMember = "cod_unidad";

        }

        private void LlenaComboTipoOrganizacion()
        {
            TipoOrganizacionData tipoOrganizacionData = new TipoOrganizacionData();
            cbxTipoOrganizacion.DataSource = tipoOrganizacionData.GetTiposDeOrganizacion();
            cbxTipoOrganizacion.DisplayMember = "descripcion";
            cbxTipoOrganizacion.ValueMember = "cod_tipo";
        }

        private void LlenaFooter() {
            string info = "Info: Nombre del Proyecto: " + proyecto.NombreProyecto +
                " - Horizonte del Proyecto: " + proyecto.HorizonteEvaluacionEnAnos +" Años"+
                " - Proponente: " + proyecto.Proponente.Nombre + " " + proyecto.Proponente.Apellidos;

            lblFoo1.Text = info;
            lblFoo2.Text = info;
            lblFoo3.Text = info;
            lblFoo4.Text = info;
            lblFoo5.Text = info;
            lblFoo6.Text = info;
            lblFoo7.Text = info;
        }

        /****************************************FIN METODOS PARA CARGAR COMBOBOX*************************************/

        /****************************************METODOS PARA ACCIONES DE COMBOBOX**************************************************/
        //Cada vez que se seleccione una provincia distinta en el combobox va a actualizar el combobox de cantones
        private void cbxProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Este if es para que se ejecute solo a partir de la segunda vez que se selecciona algo en el combobox
            //Debido a que si se permite ejecutar la primera lo q se trae es un dataRowView y no el ID de la 
            //provincia que es lo q ocupamos
            if (segundaSeleccionProvincia == false)
            {
                segundaSeleccionProvincia = true;
            }
            else
            {
                idProvinciaSeleccionada = Int32.Parse(cbxProvincia.SelectedValue.ToString());
                LlenaComboCantones();
            }//else
        }

        //Cada vez que se seleccione un canton distint en el combobox va a actualizar el combobox de distritos
        private void cbxCanton_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Este if es para que se ejecute solo a partir de la segunda vez que se selecciona algo en el combobox
            //Debido a que si se permite ejecutar la primera lo q se trae es un dataRowView y no el ID del canton
            //que es lo q ocupamos
            if (segundaSeleccionCanton == false)
            {
                segundaSeleccionCanton = true;
            }
            else
            {
                idCantonSeleccionado = Int32.Parse(cbxCanton.SelectedValue.ToString());
                LlenarComboDistritos();
            }//else
        }

        private void cbxDistrito_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Este if es para que se ejecute solo a partir de la segunda vez que se selecciona algo en el combobox
            //Debido a que si se permite ejecutar la primera lo q se trae es un dataRowView y no el ID del canton
            //que es lo q ocupamos
            if (segundaSeleccionDistrito == false)
            {
                segundaSeleccionDistrito = true;
            }
            else
            {
                idDistritoSeleccionado = Int32.Parse(cbxDistrito.SelectedValue.ToString());
            }//else
        }

        private void RegistrarProyecto_Activated(object sender, EventArgs e)
        {
            
        }

        //Estos metodos de ObtieneParaInsertar va a obtener del combobox el objeto completo para
        //ser insertado en el objeto de proyecto
        private Provincia ObtieneProvinciaParaInsertar()
        {
            DataTable dtProvincia = (DataTable)cbxProvincia.DataSource;
            Provincia provincia = new Provincia();
            foreach (DataRow fila in dtProvincia.Rows)
            {
                if (idProvinciaSeleccionada == Int32.Parse(fila["cod_provincia"].ToString()))
                {
                    provincia.CodProvincia = Int32.Parse(fila["cod_provincia"].ToString());
                    provincia.NombreProvincia = fila["nombre_provincia"].ToString();
                    return provincia;
                }//if
            }//foreach
            return provincia;
        }//ObtieneProvinciaParaInsertar

        private Canton ObtieneCantonParaInsertar()
        {
            DataTable dtCanton = (DataTable)cbxCanton.DataSource;
            Canton canton = new Canton();
            foreach (DataRow fila in dtCanton.Rows)
            {
                if (idCantonSeleccionado == Int32.Parse(fila["cod_canton"].ToString()))
                {
                    canton.CodCanton = Int32.Parse(fila["cod_canton"].ToString());
                    canton.NombreCanton = fila["nombre_canton"].ToString();
                    return canton;
                }//if
            }//foreach
            return canton;
        }//ObtieneCantonParaInsertar

        private Distrito ObtieneDistritoParaInsertar()
        {
            DataTable dtDistrito = (DataTable)cbxDistrito.DataSource;
            Distrito distrito = new Distrito();
            foreach (DataRow fila in dtDistrito.Rows)
            {
                if (idDistritoSeleccionado == Int32.Parse(fila["cod_distrito"].ToString()))
                {
                    distrito.CodDistrito = Int32.Parse(fila["cod_distrito"].ToString());
                    distrito.NombreDistrito = fila["nombre_distrito"].ToString();
                    return distrito;
                }//if
            }//foreach
            return distrito;
        }//ObtieneCantonParaInsertar

        private UnidadMedida ObtieneUnidadMedidaParaInsertar()
        {
            DataTable dtUnidadMedida = (DataTable)cbxUnidadMedida.DataSource;
            UnidadMedida unidadMedida = new UnidadMedida();
            int unidadMedidaSeleccionada = Int32.Parse(cbxUnidadMedida.SelectedValue.ToString());
            foreach (DataRow fila in dtUnidadMedida.Rows)
            {
                if (unidadMedidaSeleccionada == Int32.Parse(fila["cod_unidad"].ToString()))
                {
                    unidadMedida.CodUnidad = Int32.Parse(fila["cod_unidad"].ToString());
                    unidadMedida.NombreUnidad = fila["nombre_unidad"].ToString();
                    return unidadMedida;
                }//if
            }//foreach
            return unidadMedida;
        }//ObtieneUnidadMedidaParaInsertar

        private TipoOrganizacion ObtieneTipoOrganizacion()
        {
            DataTable dtTipoOrganizacion = (DataTable)cbxTipoOrganizacion.DataSource;
            TipoOrganizacion tipoOrganizacion = new TipoOrganizacion();
            int tipoOrganizacionSeleccionada = Int32.Parse(cbxTipoOrganizacion.SelectedValue.ToString());
            foreach (DataRow fila in dtTipoOrganizacion.Rows)
            {
                if (tipoOrganizacionSeleccionada == Int32.Parse(fila["cod_tipo"].ToString()))
                {
                    tipoOrganizacion.CodTipo = Int32.Parse(fila["cod_tipo"].ToString());
                    tipoOrganizacion.Descripcion = fila["descripcion"].ToString();
                    return tipoOrganizacion;
                }//if
            }//foreach
            return tipoOrganizacion;
        }

        /******************************Metodos para realizar las insersiones******************************************/
        private void InsertarProyecto()
        {
            try
            {
                proyecto = new Proyecto();
                proyecto.AnoInicial = Convert.ToInt32(nudAnoInicialProyecto.Value);
                proyecto.Canton = ObtieneCantonParaInsertar();
                proyecto.ConIngresos = chbConIngreso.Checked;
                proyecto.DemandaAnual = Convert.ToInt32(nudDemandaAnual.Value);
                proyecto.OfertaAnual = Convert.ToInt32(nudOferaAnual.Value);
                proyecto.DireccionExacta = txbDireccionExacta.Text;
                proyecto.Distrito = ObtieneDistritoParaInsertar();
                proyecto.Evaluador = evaluador;
                proyecto.HorizonteEvaluacionEnAnos = Convert.ToInt32(nudHorizonteEvaluacion.Value);
                proyecto.NombreProyecto = txbNombreProyecto.Text;
                proyecto.PagaImpuesto = chbSiPagaImpuesto.Checked;
                proyecto.PorcentajeImpuesto = Convert.ToDouble(nudPorcentajeImpuesto.Value);
                proyecto.Provincia = ObtieneProvinciaParaInsertar();
                proyecto.ResumenEjecutivo = txbResumenEjecutivo.Text;
                ProyectoData proyectoData = new ProyectoData();
                proyecto.CodProyecto = proyectoData.InsertarProyecto(proyecto);
                if (proyecto.CodProyecto != -1)
                {
                    InsertarObjetoInteres(proyecto.CodProyecto);
                    MessageBox.Show("Proyecto insertado con éxito", "Insertado",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }//if
            }//try
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error con la insersión", "No insertado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//catch
        }//InsertarProyecto

        private void InsertarObjetoInteres(int codProyecto)
        {
            try
            {
                ObjetoInteresProyecto objetoInteres = new ObjetoInteresProyecto();
                ObjetoInteresData objetoInteresData = new ObjetoInteresData();
                objetoInteres.DescripcionObjetoInteres = txbObjetoInteres.Text;
                objetoInteres.UnidadMedida = ObtieneUnidadMedidaParaInsertar();
                objetoInteresData.InsertarObjetoDeInteres(objetoInteres, codProyecto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void InsertarOrganizacionProponente()
        {
            try
            {
                OrganizacionProponente organizacion = new OrganizacionProponente();
                OrganizacionProponenteData organizacionData = new OrganizacionProponenteData();
                organizacion.CedulaJuridica = txbCedulaJuridica.Text;
                organizacion.Descripcion = txbDescripcion.Text;
                organizacion.NombreOrganizacion = txbNombreOrganizacion.Text;
                organizacion.Telefono = txbTelefonoOrganizacion.Text;
                organizacion.Tipo = ObtieneTipoOrganizacion();
                organizacion.CodOrganizacion = organizacionData.InsertarOrganizacionProponente(organizacion);
                if (organizacion.CodOrganizacion != -1)
                {
                    InsertarProponente(organizacion);
                    MessageBox.Show("Organizacion y proponente insertados con éxito", "Insertados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }//if
            }//try
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrió un error con la insersión", "No insertado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//catch
        }//InsertarOrganizacionProponente

        private void InsertarProponente(OrganizacionProponente organizacion)
        {
            try
            {
                Proponente proponente = new Proponente();
                ProponenteData proponenteData = new ProponenteData();
                proponente.Apellidos = txbApellidos.Text;
                proponente.Email = txbEmailProponente.Text;
                if (chbMasculino.Checked == true)
                {
                    proponente.Genero = 'm';
                }//if
                else
                {
                    proponente.Genero = 'f';
                }//else
                proponente.Nombre = txbNombreProponente.Text;
                proponente.NumIdentificacion = txbCedulaProponente.Text;
                proponente.Organizacion = organizacion;
                proponente.PuestoEnOrganizacion = txbPuestoEnOrganizacion.Text;
                proponente.Telefono = txbTelefonoProponente.Text;
                proponenteData.InsertarProponente(proponente, this.proyecto.CodProyecto);
                this.proyecto.Proponente = proponente;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }//InsertarProponente
         /******************************FIN Metodos para realizar las insersiones******************************************/

        /*************************************Metodos para actualizar***************************************************/

        private void ActualizarProyecto()
        {
            try
            {
                proyecto.AnoInicial = Convert.ToInt32(nudAnoInicialProyecto.Value);
                proyecto.Canton = ObtieneCantonParaInsertar();
                proyecto.ConIngresos = chbConIngreso.Checked;
                proyecto.DemandaAnual = Convert.ToInt32(nudDemandaAnual.Value);
                proyecto.OfertaAnual = Convert.ToInt32(nudOferaAnual.Value);
                proyecto.DireccionExacta = txbDireccionExacta.Text;
                proyecto.Distrito = ObtieneDistritoParaInsertar();
                proyecto.Evaluador = evaluador;
                proyecto.HorizonteEvaluacionEnAnos = Convert.ToInt32(nudHorizonteEvaluacion.Value);
                proyecto.NombreProyecto = txbNombreProyecto.Text;
                proyecto.PagaImpuesto = chbSiPagaImpuesto.Checked;
                proyecto.PorcentajeImpuesto = Convert.ToDouble(nudPorcentajeImpuesto.Value);
                proyecto.Provincia = ObtieneProvinciaParaInsertar();
                proyecto.ResumenEjecutivo = txbResumenEjecutivo.Text;
                proyecto.DescripcionPoblacionBeneficiaria = txbDescripcionPoblacionBeneficiaria.Text;
                proyecto.DescripcionSostenibilidadDelProyecto = txbDescripcionSostenibilidadProyecto.Text;
                proyecto.JustificacionDeMercado = txbJustificacionMercado.Text;
                proyecto.CaraterizacionDelBienServicio = txbCaracterizacionBienServicio.Text;
                ProyectoData proyectoData = new ProyectoData();
                ActualizarObjetoInteres();
                if (proyectoData.ActualizarProyecto(proyecto) == true)
                {
                    MessageBox.Show("Proyecto actualizado con éxito", "Actualizado",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }//try
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrió un error en la actualización", "No actualizado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//catch  
        }//ActualizarProyecto

        private void ActualizarObjetoInteres()
        {
            try
            {
                ObjetoInteresProyecto objetoInteres = new ObjetoInteresProyecto();
                ObjetoInteresData objetoInteresData = new ObjetoInteresData();
                objetoInteres.DescripcionObjetoInteres = txbObjetoInteres.Text;
                objetoInteres.UnidadMedida = ObtieneUnidadMedidaParaInsertar();
                objetoInteresData.ActualizarObjetoInteres(objetoInteres, proyecto.CodProyecto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }//ActualizarObjetoInteres

        private void ActualizarOrganizacionProponente()
        {
            try
            {
                OrganizacionProponenteData organizacionData = new OrganizacionProponenteData();
                proyecto.Proponente.Organizacion.CedulaJuridica = txbCedulaJuridica.Text;
                proyecto.Proponente.Organizacion.Descripcion = txbDescripcion.Text;
                proyecto.Proponente.Organizacion.NombreOrganizacion = txbNombreOrganizacion.Text;
                proyecto.Proponente.Organizacion.Telefono = txbTelefonoOrganizacion.Text;
                proyecto.Proponente.Organizacion.Tipo = ObtieneTipoOrganizacion();
                organizacionData.ActualizarOrganizacionProponente(proyecto.Proponente.Organizacion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }//ActualizarOrganizacionProponente

        private void ActualizarProponente()
        {
            try
            {
                ProponenteData proponenteData = new ProponenteData();
                proyecto.Proponente.Apellidos = txbApellidos.Text;
                proyecto.Proponente.Email = txbEmailProponente.Text;
                if (chbMasculino.Checked == true)
                {
                    proyecto.Proponente.Genero = 'm';
                }//if
                else
                {
                    proyecto.Proponente.Genero = 'f';
                }//else
                proyecto.Proponente.Nombre = txbNombreProponente.Text;
                proyecto.Proponente.NumIdentificacion = txbCedulaProponente.Text;
                proyecto.Proponente.PuestoEnOrganizacion = txbPuestoEnOrganizacion.Text;
                proyecto.Proponente.Telefono = txbTelefonoProponente.Text;
                proponenteData.ActualizarProponente(proyecto.Proponente);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /**************************************Metodos de utilidad*****************************************/


        //El siguente metodo llena el total de las reinversiones
        private void LlenaValoresTotalesReinversiones()
        {
            inicializaTotalesReinversiones();

            List<string> listVals = new List<string>();
            for (int i = proyecto.AnoInicial + 1; i <= proyecto.AnoInicial + proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                double val = 0;
                foreach (RequerimientoReinversion reqReinv in proyecto.RequerimientosReinversion)
                {
                    if (reqReinv.AnoReinversion == i)
                    {
                        val = val + (reqReinv.Cantidad * reqReinv.CostoUnitario);
                    }
                }
                listVals.Add(val.ToString());
            }

            for (int j = 0; j < dgvTotalesReinversiones.ColumnCount; j++)
            {
                this.dgvTotalesReinversiones.Rows[0].Cells[j].Value = listVals[j];
            }

                this.dgvTotalesReinversiones.ReadOnly = true;
        }//LlenaValoresTotalesReinversiones

        //El siguiente metodo inicializa el dgvtotal de las reinversiones
        private void inicializaTotalesReinversiones()
        {
            for (int j = 0; j < dgvTotalesReinversiones.ColumnCount; j++)
            {
                this.dgvTotalesReinversiones.Rows[0].Cells[j].Value = 0;
            }//for
        }//inicializaTotalesReinversiones    

        // Este metodo se encarga de modificar los mensajes que van a salir en los textbox 
        // para mostrar los tips
        private void InformacionToolTip()
        {
            this.ttMensaje.SetToolTip(this.txbNombreProyecto, "Ingrese un nombre para el proyecto");
            this.ttMensaje.SetToolTip(this.txbResumenEjecutivo, "Esta es la información del resumen ejecutivo");
            this.ttMensaje.SetToolTip(this.txbObjetoInteres, "Este campo es para especificar cual va a ser el bien que será evaluado");
            this.ttMensaje.SetToolTip(this.nudAnoInicialProyecto, "Especifica ¿cuándo se va a iniciar el proyecto?");
            this.ttMensaje.SetToolTip(this.nudHorizonteEvaluacion, "El horizonte del proyecto en años");
            this.ttMensaje.SetToolTip(this.nudDemandaAnual, "Información de la demanda anual");
            this.ttMensaje.SetToolTip(this.nudOferaAnual, "Información de la oferta anual");
        }//InformacionToolTip

        //Estos 2 metodos de dataerror son redefinidos para evitar una pulga propia de los DataGridView
        //Que causa que se este ejecutando el dataerror (ES LA UNICA FORMA DE RESOLVER ESE ERROR)
        private void dgvInversiones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvReinversiones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        // El siguiente metodo se va a encargar de llenar la información del proyecto que esta entrando 
        // como parametro cuando el proyecto que se selecciona es para modificar
        private void LlenaInformacionProyecto()
        {
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            txbNombreProyecto.Text = proyecto.NombreProyecto;
            txbResumenEjecutivo.Text = proyecto.ResumenEjecutivo;
            cbxProvincia.SelectedValue = proyecto.Provincia.CodProvincia;
            cbxCanton.SelectedValue = proyecto.Canton.CodCanton;
            cbxDistrito.SelectedValue = proyecto.Distrito.CodDistrito;
            txbDireccionExacta.Text = proyecto.DireccionExacta;
            if (proyecto.ConIngresos == true)
            {
                chbConIngreso.Checked = true;
            }//if
            else
            {
                chbSinIngreso.Checked = true;
            }//else
            ObjetoInteresProyecto objetoInteres = GetObjetoInteres();
            txbObjetoInteres.Text = proyecto.ObjetoInteres.DescripcionObjetoInteres;
            cbxUnidadMedida.SelectedValue = unidadMedidaData.GetUnidadMedida(objetoInteres.UnidadMedida.CodUnidad).CodUnidad;
            nudAnoInicialProyecto.Value = proyecto.AnoInicial;
            nudHorizonteEvaluacion.Value = proyecto.HorizonteEvaluacionEnAnos;
            nudDemandaAnual.Value = proyecto.DemandaAnual;
            nudOferaAnual.Value = proyecto.OfertaAnual;
            if (proyecto.PagaImpuesto == true)
            {
                chbSiPagaImpuesto.Checked = true;
            }//if
            else
            {
                chbNoPagaImpuesto.Checked = true;
            }//else
            nudPorcentajeImpuesto.Value = (Decimal)proyecto.PorcentajeImpuesto;
            txbDescripcionPoblacionBeneficiaria.Text = proyecto.DescripcionPoblacionBeneficiaria;
            txbJustificacionMercado.Text = proyecto.JustificacionDeMercado;
            txbCaracterizacionBienServicio.Text = proyecto.CaraterizacionDelBienServicio;
            txbDescripcionSostenibilidadProyecto.Text = proyecto.DescripcionSostenibilidadDelProyecto;
        }//LlenaInformacionProyecto
        
        //Llena la informacion de la organizacion cuando se selecciona un proyecto para modificar
        private void LlenaInformacionOrganizacion()
        {
            txbNombreOrganizacion.Text = proyecto.Proponente.Organizacion.NombreOrganizacion;
            cbxTipoOrganizacion.SelectedValue = proyecto.Proponente.Organizacion.Tipo.CodTipo;
            txbCedulaJuridica.Text = proyecto.Proponente.Organizacion.CedulaJuridica;
            txbTelefonoOrganizacion.Text = proyecto.Proponente.Organizacion.Telefono;
            txbDescripcion.Text = proyecto.Proponente.Organizacion.Descripcion;

        }

        private void btnVerResumen4_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarProyeccion_Click(object sender, EventArgs e)
        {

        }

        //Llena la informacion del proponente cuando se selecciona un proyecto para modificar
        private void LlenaInformacionProponente()
        {
            txbNombreProponente.Text = proyecto.Proponente.Nombre;
            txbApellidos.Text = proyecto.Proponente.Apellidos;
            txbCedulaProponente.Text = proyecto.Proponente.NumIdentificacion;
            txbTelefonoProponente.Text = proyecto.Proponente.Telefono;
            txbEmailProponente.Text = proyecto.Proponente.Email;
            txbPuestoEnOrganizacion.Text = proyecto.Proponente.PuestoEnOrganizacion;
            if (proyecto.Proponente.Genero == 'm')
            {
                chbMasculino.Checked = true;
            }//if
            else
            {
                chbFemenino.Checked = true;
            }//else
        }//LlenaInformacionProponente

        private void btnAgregarProyecciones_Click(object sender, EventArgs e)
        {
            mostrarMensajeSeguridad = false;
            AgregarProyeccion agregarProyeccion = new AgregarProyeccion(evaluador, proyecto);
            agregarProyeccion.MdiParent = this.MdiParent;
            agregarProyeccion.Show();
            this.Close();
        }

        private ObjetoInteresProyecto GetObjetoInteres()
        {
            ObjetoInteresData objetoInteresData = new ObjetoInteresData();
            return objetoInteresData.GetObjetoInteres(proyecto.CodProyecto);
        }

        private void LlenaDgvInversiones()
        {
            if ((proyecto.RequerimientosInversion != null) && (this.proyecto.RequerimientosInversion.Count > 0))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("RequerimientInversion");
                ds.Tables["RequerimientInversion"].Columns.Add("descripcionRequerimiento", 
                    Type.GetType("System.String"));
                ds.Tables["RequerimientInversion"].Columns.Add("cantidad", Type.GetType("System.String"));
                ds.Tables["RequerimientInversion"].Columns.Add("costoUnitario", Type.GetType("System.String"));
                ds.Tables["RequerimientInversion"].Columns.Add("depreciable", Type.GetType("System.Boolean"));
                ds.Tables["RequerimientInversion"].Columns.Add("vidaUtil", Type.GetType("System.String"));
                ds.Tables["RequerimientInversion"].Columns.Add("Subtotal", Type.GetType("System.String"));
                Double totalInver = 0;
                foreach (RequerimientoInversion requerimiento in proyecto.RequerimientosInversion)
                {
                    DataRow row = ds.Tables["RequerimientInversion"].NewRow();
                    row["descripcionRequerimiento"] = requerimiento.DescripcionRequerimiento;
                    row["cantidad"] = requerimiento.Cantidad;
                    row["costoUnitario"] = requerimiento.CostoUnitario;
                    row["depreciable"] = requerimiento.Depreciable;
                    row["VidaUtil"] = requerimiento.VidaUtil;
                    row["Subtotal"] = requerimiento.Cantidad * requerimiento.CostoUnitario;
                    ds.Tables["RequerimientInversion"].Rows.Add(row);
                    totalInver = totalInver + (requerimiento.Cantidad * requerimiento.CostoUnitario);
                }//foreach
                DataTable dtRequerimientos = ds.Tables["RequerimientInversion"];
                dgvInversiones.DataSource = dtRequerimientos;
                int autoincrement = 0;
                foreach (RequerimientoInversion requerimiento in this.proyecto.RequerimientosInversion)
                {
                    this.dgvInversiones.Rows[autoincrement].Cells["UnidadMedida"].Value
                            = requerimiento.UnidadMedida.NombreUnidad.ToString();
                    autoincrement++;
                }//foreach

                lblTotalInversiones.Text = totalInver.ToString("#,##0");
                // Aqui realizo este cambio de index debido a que las reinversiones cargan los años de 
                // reinversión hasta que entra por segunda vez al tab de reinversiones
                tbxRegistrarProyecto.SelectedIndex = 5;
                tbxRegistrarProyecto.SelectedIndex = 3;
            }//if
        }//LlenaDgvInversiones

        private void btnAgregarCosto_Click(object sender, EventArgs e)
        {
            mostrarMensajeSeguridad = false;
            AgregarCosto agregarCosto = new AgregarCosto(evaluador, proyecto);
            agregarCosto.MdiParent = this.MdiParent;
            agregarCosto.Show();
            this.Close();
        }

        private void LlenaDgvReinversiones()
        {
            if (this.proyecto.RequerimientosReinversion != null && this.proyecto.RequerimientosReinversion.Count > 0)
            {
                List<String> anosReinversion = new List<String>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    anosReinversion.Add(anoActual.ToString());
                }//for
                DataSet ds = new DataSet();
                ds.Tables.Add("RequerimientReinversion");
                ds.Tables["RequerimientReinversion"].Columns.Add("Descripcion", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("Cantidad", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("CostoUnitario", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("Depreciable", Type.GetType("System.Boolean"));
                ds.Tables["RequerimientReinversion"].Columns.Add("vidaUtilRe", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("Subtotal", Type.GetType("System.String"));
                foreach (RequerimientoReinversion requerimiento in this.proyecto.RequerimientosReinversion)
                {
                    DataRow row = ds.Tables["RequerimientReinversion"].NewRow();
                    row["Descripcion"] = requerimiento.DescripcionRequerimiento;
                    row["Cantidad"] = requerimiento.Cantidad;
                    row["CostoUnitario"] = requerimiento.CostoUnitario;
                    row["Depreciable"] = requerimiento.Depreciable;
                    row["vidaUtilRe"] = requerimiento.VidaUtil;
                    row["Subtotal"] = 0;
                    ds.Tables["RequerimientReinversion"].Rows.Add(row);
                }//foreach
                DataTable dtRequerimientos = ds.Tables["RequerimientReinversion"];
                dgvReinversiones.DataSource = dtRequerimientos;

                DataGridViewComboBoxColumn anoInversionColumn = dgvReinversiones.Columns["AnoReinversion"] 
                    as DataGridViewComboBoxColumn;
                anoInversionColumn.DataSource = anosReinversion;
                int autoincrement = 0;
                foreach (RequerimientoReinversion requerimiento in this.proyecto.RequerimientosReinversion)
                {
                    this.dgvReinversiones.Rows[autoincrement].Cells["AnoReinversion"].Value
                            = requerimiento.AnoReinversion;

                    this.dgvReinversiones.Rows[autoincrement].Cells["unidadMedidaRe"].Value
                            = requerimiento.UnidadMedida.NombreUnidad.ToString();
                    autoincrement++;
                 }//foreach

                // Aqui realizo este cambio de index debido a que las reinversiones cargan los años de 
                // reinversión hasta que entra por segunda vez al tab de reinversiones
                tbxRegistrarProyecto.SelectedIndex = 5;
                tbxRegistrarProyecto.SelectedIndex = 4;
            }//if
        }//LlneaDgvReinversiones

        //El siguiente metodo llena el dgvTotalReinversiones a partir del dgvReinversiones
        private void LlenaDgvTotalesReinversiones()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("TotalesReinversiones");

            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                int anoActual = proyecto.AnoInicial + i;
                ds.Tables["TotalesReinversiones"].Columns.Add(anoActual.ToString(), Type.GetType("System.String"));
            }//for

            //dgvTotalesReinversiones.Rows.Add();
            ds.Tables["TotalesReinversiones"].Rows.Add();
            DataTable dtTotalReinversiones = ds.Tables["TotalesReinversiones"];
            dgvTotalesReinversiones.DataSource = dtTotalReinversiones;
        }//LlenaDgvTotalesReinversiones

        private void llbGestionVariacionCostos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarMensajeSeguridad = false;
            GestionVariacionCostos gestionVariacionCostos = new GestionVariacionCostos(evaluador, proyecto);
            gestionVariacionCostos.MdiParent = this.MdiParent;
            gestionVariacionCostos.Show();
            this.Close();
        }

        private void btnVerResumenCostos_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarDep_Click(object sender, EventArgs e)
        {

        }

        private void LlenaDgvProyeccionesVentas()
        {
            if ((proyecto.Proyecciones != null) && (this.proyecto.Proyecciones.Count > 0))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("Proyecciones");
                ds.Tables["Proyecciones"].Columns.Add("Articulo", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Unidad", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Enero", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Febrero", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Marzo", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Abril", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Mayo", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Junio", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Julio", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Agosto", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Setiembre", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Octubre", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Noviembre", Type.GetType("System.String"));
                ds.Tables["Proyecciones"].Columns.Add("Diciembre", Type.GetType("System.String"));
                foreach (ProyeccionVentaArticulo proyTemp in this.proyecto.Proyecciones)
                {
                    DataRow row = ds.Tables["Proyecciones"].NewRow();
                    row["Articulo"] = proyTemp.NombreArticulo;
                    row["Unidad"] = proyTemp.UnidadMedida.NombreUnidad;
                    row["Enero"] = ("₡" + proyTemp.DetallesProyeccionVenta[0].Precio +" | "+ proyTemp.DetallesProyeccionVenta[0].Cantidad + " ud").ToString();
                    row["Febrero"] = ("₡" + proyTemp.DetallesProyeccionVenta[1].Precio + " | " + proyTemp.DetallesProyeccionVenta[1].Cantidad + " ud").ToString();
                    row["Marzo"] = ("₡" + proyTemp.DetallesProyeccionVenta[2].Precio + " | " + proyTemp.DetallesProyeccionVenta[2].Cantidad + " ud").ToString();
                    row["Abril"] = ("₡" + proyTemp.DetallesProyeccionVenta[3].Precio + " | " + proyTemp.DetallesProyeccionVenta[3].Cantidad + " ud").ToString();
                    row["Mayo"] = ("₡" + proyTemp.DetallesProyeccionVenta[4].Precio + " | " + proyTemp.DetallesProyeccionVenta[4].Cantidad + " ud").ToString();
                    row["Junio"] = ("₡" + proyTemp.DetallesProyeccionVenta[5].Precio + " | " + proyTemp.DetallesProyeccionVenta[5].Cantidad + " ud").ToString();
                    row["Julio"] = ("₡" + proyTemp.DetallesProyeccionVenta[6].Precio + " | " + proyTemp.DetallesProyeccionVenta[6].Cantidad + " ud").ToString();
                    row["Agosto"] = ("₡" + proyTemp.DetallesProyeccionVenta[7].Precio + " | " + proyTemp.DetallesProyeccionVenta[7].Cantidad + " ud").ToString();
                    row["Setiembre"] = ("₡" + proyTemp.DetallesProyeccionVenta[8].Precio + " | " + proyTemp.DetallesProyeccionVenta[8].Cantidad + " ud").ToString();
                    row["Octubre"] = ("₡" + proyTemp.DetallesProyeccionVenta[9].Precio + " | " + proyTemp.DetallesProyeccionVenta[9].Cantidad + " ud").ToString();
                    row["Noviembre"] = ("₡" + proyTemp.DetallesProyeccionVenta[10].Precio + " | " + proyTemp.DetallesProyeccionVenta[10].Cantidad + " ud").ToString();
                    row["Diciembre"] = ("₡" + proyTemp.DetallesProyeccionVenta[11].Precio + " | " + proyTemp.DetallesProyeccionVenta[11].Cantidad + " ud").ToString();

                    ds.Tables["Proyecciones"].Rows.Add(row);
                    
                }//foreach

                DataTable dtProyecciones = ds.Tables["Proyecciones"];
                this.dgvProyeccionesVentas.DataSource = dtProyecciones;
            }//if
        }//LlenaDgvProyeccionesVentas

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelectedIndex = checkedListBox1.SelectedIndex;
            if (iSelectedIndex == -1) { return; }

            for (int iIndex = 0; iIndex < checkedListBox1.Items.Count; iIndex++)
            { 
                checkedListBox1.SetItemCheckState(iIndex, CheckState.Unchecked);
                checkedListBox1.SetItemCheckState(iSelectedIndex, CheckState.Checked);
            }

            int tiempo = Convert.ToInt32(nudTiempoFinanciamiento.Value);

            mostrarMensajeSeguridad = false;
            InteresFinanciamientoUI gestionCrecimientoOferta = new InteresFinanciamientoUI(evaluador, proyecto, iSelectedIndex, tiempo);
            gestionCrecimientoOferta.MdiParent = this.MdiParent;
            gestionCrecimientoOferta.Show();
            this.Close();
        }

        private void btnGuardarFinanciamiento_Click(object sender, EventArgs e)
        {
            Llenadgvnanciamiento();
        }

        private void llbRegistrarCrecimientosOferta_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarMensajeSeguridad = false;
            GestionCrecimientosOferta gestionCrecimientoOferta = new GestionCrecimientosOferta(evaluador, proyecto);
            gestionCrecimientoOferta.MdiParent = this.MdiParent;
            gestionCrecimientoOferta.Show();
            this.Close();
        }

        private void LlenaDgvIngresosGenerados()
        {

            DataSet ds = new DataSet();
            ds.Tables.Add("IngresosGenerados");
            ds.Tables["IngresosGenerados"].Columns.Add("titulo", Type.GetType("System.String"));

            DataRow row = ds.Tables["IngresosGenerados"].NewRow();
            row["titulo"] = "Ingresos";

            int a = 1;
            foreach (double IngreGenerado in proyecto.IngresosGenerados) {
                ds.Tables["IngresosGenerados"].Columns.Add("Año " + a, Type.GetType("System.String"));
                row["Año " + a] = "₡ " + IngreGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["IngresosGenerados"].Rows.Add(row);

            DataTable dtIngresosGenerados = ds.Tables["IngresosGenerados"];
            dgvIngresosGenerados.DataSource = dtIngresosGenerados;
            dgvIngresosGenerados.Columns[0].HeaderText = "";
            
        }//LlenaDgvIngresosGenerados

        private void LlenaDgvCostos()
        {
            if ((this.proyecto.Costos != null) && (this.proyecto.Costos.Count > 0))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("Costos");
                ds.Tables["Costos"].Columns.Add("Articulo", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Unidad", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Variable", Type.GetType("System.Boolean"));
                ds.Tables["Costos"].Columns.Add("Categoria", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Enero", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Febrero", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Marzo", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Abril", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Mayo", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Junio", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Julio", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Agosto", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Setiembre", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Octubre", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Noviembre", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Diciembre", Type.GetType("System.String"));
                foreach (Costo costoTemp in this.proyecto.Costos)
                {
                    DataRow row = ds.Tables["Costos"].NewRow();
                    row["Articulo"] = costoTemp.NombreCosto;
                    row["Unidad"] = costoTemp.UnidadMedida.NombreUnidad;
                    row["Variable"] = costoTemp.CostoVariable;
                    row["Categoria"] = costoTemp.Categoria_costo;
                    row["Enero"] = ("₡" + costoTemp.CostosMensuales[0].CostoUnitario + " | " + costoTemp.CostosMensuales[0].Cantidad + " ud").ToString();
                    row["Febrero"] = ("₡" + costoTemp.CostosMensuales[1].CostoUnitario + " | " + costoTemp.CostosMensuales[1].Cantidad + " ud").ToString();
                    row["Marzo"] = ("₡" + costoTemp.CostosMensuales[2].CostoUnitario + " | " + costoTemp.CostosMensuales[2].Cantidad + " ud").ToString();
                    row["Abril"] = ("₡" + costoTemp.CostosMensuales[3].CostoUnitario + " | " + costoTemp.CostosMensuales[3].Cantidad + " ud").ToString();
                    row["Mayo"] = ("₡" + costoTemp.CostosMensuales[4].CostoUnitario + " | " + costoTemp.CostosMensuales[4].Cantidad + " ud").ToString();
                    row["Junio"] = ("₡" + costoTemp.CostosMensuales[5].CostoUnitario + " | " + costoTemp.CostosMensuales[5].Cantidad + " ud").ToString();
                    row["Julio"] = ("₡" + costoTemp.CostosMensuales[6].CostoUnitario + " | " + costoTemp.CostosMensuales[6].Cantidad + " ud").ToString();
                    row["Agosto"] = ("₡" + costoTemp.CostosMensuales[7].CostoUnitario + " | " + costoTemp.CostosMensuales[7].Cantidad + " ud").ToString();
                    row["Setiembre"] = ("₡" + costoTemp.CostosMensuales[8].CostoUnitario + " | " + costoTemp.CostosMensuales[8].Cantidad + " ud").ToString();
                    row["Octubre"] = ("₡" + costoTemp.CostosMensuales[9].CostoUnitario + " | " + costoTemp.CostosMensuales[9].Cantidad + " ud").ToString();
                    row["Noviembre"] = ("₡" + costoTemp.CostosMensuales[10].CostoUnitario + " | " + costoTemp.CostosMensuales[10].Cantidad + " ud").ToString();
                    row["Diciembre"] = ("₡" + costoTemp.CostosMensuales[11].CostoUnitario + " | " + costoTemp.CostosMensuales[11].Cantidad + " ud").ToString();

                    ds.Tables["Costos"].Rows.Add(row);

                }//foreach

                DataTable dtCostos = ds.Tables["Costos"];
                this.dgvCostos.DataSource = dtCostos;
            }//if
        }//LlenaDgvCostos

        private void LlenaDgvCostosGenerados()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("CostosGenerados");
            ds.Tables["CostosGenerados"].Columns.Add("titulo", Type.GetType("System.String"));

            DataRow row = ds.Tables["CostosGenerados"].NewRow();
            row["titulo"] = "Costos";

            int a = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                ds.Tables["CostosGenerados"].Columns.Add("Año " + a, Type.GetType("System.String"));
                row["Año " + a] = "₡ " + costoGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["CostosGenerados"].Rows.Add(row);

            DataTable dtCostosGenerados = ds.Tables["CostosGenerados"];
            dgvCostosGenerados.DataSource = dtCostosGenerados;
            dgvCostosGenerados.Columns[0].HeaderText = "";

        }//LlenaDgvCostosGenerados

        private void LlenaDgvCapitalTrabajo()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("CapitalTrabajo");
            ds.Tables["CapitalTrabajo"].Columns.Add("Rubro", Type.GetType("System.String"));

            DataRow row = ds.Tables["CapitalTrabajo"].NewRow();
            row["Rubro"] = "Costos variables";
            int a = 1;
            ds.Tables["CapitalTrabajo"].Columns.Add("Año 0", Type.GetType("System.String"));
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                ds.Tables["CapitalTrabajo"].Columns.Add("Año " + a, Type.GetType("System.String"));
                row["Año " + a] = "₡ " + costoGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row);

            //llena fila de capital de trabajo
            DataRow row2 = ds.Tables["CapitalTrabajo"].NewRow();
            row2["Rubro"] = "Capital de trabajo";
            int a2 = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                row2["Año " + a2] = "₡ " + ((costoGenerado / 12) * 1.5).ToString("#,##0.##");
                a2++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row2);

            //llena incremental

            Double recCT = 0;
            Double val = 0;
            DataRow row3 = ds.Tables["CapitalTrabajo"].NewRow();
            row3["Rubro"] = "Incremental";

            val = -((proyecto.CostosGenerados[0] / 12) * 1.5);
            row3["Año 0"] = "₡ " + (val).ToString("#,##0.##");
            recCT = recCT + val;

            int a3 = 1;
            for (int i=1; i<proyecto.CostosGenerados.Count; i++)
            {
                val = ((proyecto.CostosGenerados[i] / 12) * 1.5) - ((proyecto.CostosGenerados[i - 1] / 12) * 1.5);
                row3["Año " + a3] = "₡ " + (val).ToString("#,##0.##");
                recCT = recCT + val;
                a3++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row3);

            // Aqui realizo este cambio de index debido a que las reinversiones cargan los años de 
            // reinversión hasta que entra por segunda vez al tab de reinversiones
            tbxRegistrarProyecto.SelectedIndex = 5;
            tbxRegistrarProyecto.SelectedIndex = 4;

            DataTable dtCapitalTrabajo = ds.Tables["CapitalTrabajo"];
            dgvCapitalTrabajo.DataSource = dtCapitalTrabajo;

            lblRecuperacionCT.Text = "₡ " + recCT.ToString("#,##0.##");
        }

        private void Llenadgvnanciamiento()
        {
            double monto = Convert.ToInt32(tbMontoFinanciamiento.Text);
            int tiempo = proyecto.Financiamiento.TiempoFinanciamiento;
            List<InteresFinanciamiento> intereses = new List<InteresFinanciamiento>();
            intereses = proyecto.InteresesFinanciamiento;

            DataSet ds = new DataSet();
            ds.Tables.Add("AmortizacionPrestamo");
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo1", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo2", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo3", Type.GetType("System.String"));
            ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo4", Type.GetType("System.String"));

            for (int i = 0; i < tiempo; i++) {
                DataRow row1 = ds.Tables["AmortizacionPrestamo"].NewRow();
                row1["titulo1"] = monto;

                double cuota = (Math.Pow(monto * intereses[i].PorcentajeInteresFinanciamiento * (1 + intereses[i].PorcentajeInteresFinanciamiento), tiempo)) / (1 + Math.Pow(intereses[i].PorcentajeInteresFinanciamiento, tiempo - 1));
                row1["titulo2"] = cuota;

                double interes = monto * intereses[i].PorcentajeInteresFinanciamiento;
                row1["titulo3"] = interes;

                double amortizacion = monto - interes;
                row1["titulo4"] = amortizacion;

                ds.Tables["AmortizacionPrestamo"].Rows.Add(row1);

                monto = monto - amortizacion;
            }
            DataTable dtCostosGenerados = ds.Tables["AmortizacionPrestamo"];
            dgvFinanciamiento.DataSource = dtCostosGenerados;
            dgvFinanciamiento.Columns[0].HeaderText = "";

        }
        /**************************************FIN Metodos de utilidad*****************************************/
    }//Clase registrar proyecto
}//namespace