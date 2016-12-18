﻿//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.WindowsUI
{
    public partial class RegistrarProyecto : Form
    {
        //Variables globales
        bool mostroMensaje = false, mostrarMensajeSeguridad = true;
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
        public RegistrarProyecto(Evaluador evaluador, Proyecto proyecto, int indexTab)
        {
            this.proyecto = proyecto;
            this.evaluador = evaluador;
            InitializeComponent();
            LlenaComboProvincia();
            LlenaComboCantones();
            LlenarComboDistritos();
            LlenarComboUnidadMedida();
            LlenaComboTipoOrganizacion();
            InformacionToolTip();
            LlenaInformacionProyecto();
            LlenaInformacionProponente();
            LlenaInformacionOrganizacion();
            mostrarMensajeSeguridad = true;
            

            LlenaFooter();

            tbxRegistrarProyecto.SelectedIndex = indexTab;
            tbxRegistrarProyecto_Selected(this, null);
        }


        /******************************************INICIO DE VALIDACIONES DE CHECKBOX*******************************************************/
        // El siguiente metodo es para cuando se realiza una accion en el checkbox primero desmarque la casilla
        // del otro check box y luego seleccione la casilla que se esta presionando
        private void chbConIngreso_CheckedChanged(object sender, EventArgs e)
        {
            if ((chbSinIngreso.Checked))
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
            if ((chbConIngreso.Checked))
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
            if (proyecto == null && !tbxRegistrarProyecto.SelectedIndex.Equals(0))
            {
                mostroMensaje = true;
                MessageBox.Show("Por favor ingrese todos los datos para poder avanzar a la siguiente pestaña",
                "Datos vacios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxRegistrarProyecto.SelectedIndex = 0;
            }
            if (!mostroMensaje)
            {
                int indice = tbxRegistrarProyecto.SelectedIndex;
                switch (indice)
                {
                    case 3:
                        LlenaDgvInversiones();
                        break;
                    case 4:
                        LlenaDgvReinversiones();
                        LlenaDgvTotalesReinversiones();
                        break;
                    case 5:
                        LlenaDgvProyeccionesVentas();
                        LlenaDgvIngresosGenerados();
                        break;
                    case 6:
                        LlenaDgvCostos();
                        LlenaDgvCostosGenerados();
                        break;

                    case 7:
                        LlenaDgvCapitalTrabajo();
                        break;

                    case 8:
                        LlenaDgvDepreciaciones();
                        break;

                    case 9:
                        //Llena datos de financiamiento
                        InteresFinanciamientoData interesData = new InteresFinanciamientoData();
                        FinanciamientoData financiamientoData = new FinanciamientoData();
                        List<InteresFinanciamiento> listTemp = new List<InteresFinanciamiento>();
                        listTemp = interesData.GetInteresesFinanciamiento(proyecto.CodProyecto, 0);
                        if (listTemp.Count > 0)
                        {
                            proyecto.InteresFinanciamientoIF = listTemp[0];
                        }
                        //
                        if (proyecto.FinanciamientoIF == null)
                        {
                            proyecto.FinanciamientoIF = financiamientoData.GetFinanciamiento(proyecto.CodProyecto, false);
                        }

                        if (proyecto.FinanciamientoIF != null)
                        {
                            tbMontoFinanciamientoIF.Text = proyecto.FinanciamientoIF.MontoFinanciamiento.ToString();
                            nupTiempoFinanciamientoIF.Value = (Decimal)proyecto.FinanciamientoIF.TiempoFinanciamiento;
                            if (proyecto.InteresFinanciamientoIF.PorcentajeInteresFinanciamiento != 0)
                            {
                                nupPorcentajeInteresIF.Value = (Decimal)proyecto.InteresFinanciamientoIF.PorcentajeInteresFinanciamiento;
                            }
                            LlenaDgvFinanciamientoIF();
                        }

                        if (proyecto.FinanciamientoIV == null)
                        {
                            proyecto.FinanciamientoIV = financiamientoData.GetFinanciamiento(proyecto.CodProyecto, true);
                        }

                        if (proyecto.FinanciamientoIV != null)
                        {
                            tbMontoVariable.Text = proyecto.FinanciamientoIV.MontoFinanciamiento.ToString();
                            nudTiempoVariable.Value = (Decimal)proyecto.FinanciamientoIV.TiempoFinanciamiento;
                            if (proyecto.InteresesFinanciamientoIV.Count == 0)
                            {
                                proyecto.InteresesFinanciamientoIV = interesData.GetInteresesFinanciamiento(proyecto.CodProyecto, 1);
                            }
                            LlenaDgvFinanciamientoIV();
                        }
                        break;

                    case 10:

                        if (dgvCapitalTrabajo.RowCount == 0 && this.proyecto.CostosGenerados.Count != 0)
                        {
                            LlenaDgvCapitalTrabajo();
                        }
                        LlenaDgvFlujoCajaIF();
                        LlenaDgvFlujoCajaIV();

                        break;

                    case 11:
                        lblNombreProyecto.Text = proyecto.NombreProyecto;
                        lblTipoProyecto.Text = proyecto.ConIngresos ? "Con Ingreso" : "Sin Ingreso";
                        lblObjetoInteres.Text = proyecto.ObjetoInteres.DescripcionObjetoInteres;
                        lblOrganizacionProponente.Text = proyecto.Proponente.Organizacion.NombreOrganizacion;
                        lblNombreProponente.Text = proyecto.Proponente.Nombre + " " + proyecto.Proponente.Apellidos;
                        lblTelefonoProponente.Text = proyecto.Proponente.Telefono;
                        lblNombreEvaluador.Text = evaluador.Nombre;
                        lblCedulaEvaluador.Text = evaluador.NumIdentificacion;

                        break;
                    default:
                        mostroMensaje = false;
                        break;
                }//switch
            }
            mostroMensaje = false;
        }

        //La funcionalidad de este metodo muestra un mensaje de confirmación por si presiona el boton de
        //cerrar ventana erroneamente y así se evita perder información ya ingresada pero que esta en
        //memoria volatil
        private void RegistrarProyecto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mostrarMensajeSeguridad == true)
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
            if (!txbCedulaJuridica.Text.ToString().Equals(string.Empty) && !txbCedulaProponente.Text.ToString().Equals(string.Empty))
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
            else
            {
                MessageBox.Show("Por favor ingrese todos los siguientes datos requeridos: "+
                    "\n 1. Cédula Jurídica (Para organizaciones) "+
                    "\n 2. Cédula del Representante",
                             "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        lblTotalInversiones.Text = "₡ " + total.ToString("#,##0.##");
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
            for (int i = 0; i < dgvInversiones.RowCount - 1; i++)
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

                    if (this.dgvInversiones.Rows[i].Cells["codRequerimiento"].Value.ToString().Equals(string.Empty))
                    {
                        requerimientoInversiones.CodRequerimientoInversion = 0;
                    }

                    else
                    {
                        requerimientoInversiones.CodRequerimientoInversion = Int32.Parse(this.dgvInversiones.Rows[i].Cells["codRequerimiento"].Value.ToString());
                    }

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

                    //if (unidadMedida.CodUnidad.Equals(0))
                    //{
                    //    unidadMedida.CodUnidad = 1;
                    //    unidadMedida.NombreUnidad = "Litros";
                    //}
                    //requerimientoInversiones.UnidadMedida = unidadMedida;

                    if (this.dgvInversiones.Rows[i].Cells["Depreciable"].Value.ToString().Equals(""))
                    {
                        requerimientoInversiones.Depreciable = false;
                    }
                    else
                    {
                        requerimientoInversiones.Depreciable =
                            Convert.ToBoolean(this.dgvInversiones.Rows[i].Cells["Depreciable"].Value);
                    }

                    if (i > listaRequerimientoInversion.Count - 1 && requerimientoInversiones.UnidadMedida.CodUnidad != 0)
                    {

                        if (this.dgvInversiones.Rows[i].Cells["VidaUtil"].Value.ToString().Equals(string.Empty))
                        {
                            requerimientoInversiones.VidaUtil = 0;
                        }
                        else
                        {
                            requerimientoInversiones.VidaUtil = Int32.Parse(this.dgvInversiones.Rows[i].Cells["VidaUtil"].Value.ToString());
                        }

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
                catch (Exception ex)
                {
                    validaInsersion = false;
                    Console.WriteLine(ex);
                }//catch
            }//for
            if (validaInsersion)
            {
                foreach (RequerimientoInversion inversion in listaRequerimientoInversion)
                {
                    proyecto.RequerimientosInversion.Add(inversion);
                }

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
            LlenaDgvTotalesReinversiones();

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
            for (int i = 0; i < dgvReinversiones.RowCount - 1; i++)
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

                        requerimientoReinversion.CostoUnitario =
                            Convert.ToDouble(Int32.Parse(this.dgvReinversiones.Rows[i].
                            Cells["CostoUnitarioReinversion"].Value.ToString()));

                        if (this.dgvReinversiones.Rows[i].Cells["Codigo"].Value.ToString().Equals(string.Empty))
                        {
                            requerimientoReinversion.CodRequerimientoReinversion = 0;
                        }
                        else
                        {
                            requerimientoReinversion.CodRequerimientoReinversion =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["Codigo"].Value.ToString());
                        }

                        requerimientoReinversion.VidaUtil =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["vidaUtilRe"].Value.ToString());
                        requerimientoReinversion.AnoReinversion =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["AnoReinversion"].Value.ToString());

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
                        if (tam <= i)
                        {
                            proyecto.RequerimientosReinversion.Add(requereinvData.InsertarRequerimientosReinversion(requerimientoReinversion, this.proyecto.CodProyecto));
                            validaInsersion = true;
                        }
                        else
                        {
                            requerimientoReinversion = requereinvData.EditarRequerimientoReinversion(requerimientoReinversion, this.proyecto.CodProyecto);
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
                LlenaDgvTotalesReinversiones();
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
            if (segundaSeleccionDistrito == false)
            {
                segundaSeleccionDistrito = true;
            }
            else
            {
                idDistritoSeleccionado = Int32.Parse(cbxDistrito.SelectedValue.ToString());
            }//else
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
                proponente.Genero = chbMasculino.Checked ? 'm' : 'f';
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
                ds.Tables["RequerimientInversion"].Columns.Add("codRequerimiento", Type.GetType("System.String"));
                ds.Tables["RequerimientInversion"].Columns.Add("descripcionRequerimiento", Type.GetType("System.String"));
                ds.Tables["RequerimientInversion"].Columns.Add("cantidad", Type.GetType("System.Int32"));
                ds.Tables["RequerimientInversion"].Columns.Add("costoUnitario", Type.GetType("System.Int32"));
                ds.Tables["RequerimientInversion"].Columns.Add("depreciable", Type.GetType("System.Boolean"));
                ds.Tables["RequerimientInversion"].Columns.Add("vidaUtil", Type.GetType("System.Int32"));
                ds.Tables["RequerimientInversion"].Columns.Add("Subtotal", Type.GetType("System.Int32"));
                Double totalInver = 0;
                foreach (RequerimientoInversion requerimiento in proyecto.RequerimientosInversion)
                {
                    DataRow row = ds.Tables["RequerimientInversion"].NewRow();
                    row["codRequerimiento"] = requerimiento.CodRequerimientoInversion;
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
                dgvInversiones.Columns["codRequerimiento"].Visible = false;

                int autoincrement = 0;
                foreach (RequerimientoInversion requerimiento in this.proyecto.RequerimientosInversion)
                {
                    this.dgvInversiones.Rows[autoincrement].Cells["UnidadMedida"].Value
                            = requerimiento.UnidadMedida.NombreUnidad.ToString();
                    autoincrement++;
                }//foreach

                lblTotalInversiones.Text = "₡ " + totalInver.ToString("#,##0.##");
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
                ds.Tables["RequerimientReinversion"].Columns.Add("Codigo", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("Descripcion", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("Cantidad", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("CostoUnitario", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("Depreciable", Type.GetType("System.Boolean"));
                ds.Tables["RequerimientReinversion"].Columns.Add("vidaUtilRe", Type.GetType("System.String"));
                ds.Tables["RequerimientReinversion"].Columns.Add("Subtotal", Type.GetType("System.String"));
                foreach (RequerimientoReinversion requerimiento in this.proyecto.RequerimientosReinversion)
                {
                    DataRow row = ds.Tables["RequerimientReinversion"].NewRow();
                    row["Codigo"] = requerimiento.CodRequerimientoReinversion;
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

                dgvReinversiones.Columns["Codigo"].Visible = false;
                dgvReinversiones.Columns["SubtotalReinversion"].Width = 162;

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
            }//if
        }//LlneaDgvReinversiones

        //El siguiente metodo llena el dgvTotalReinversiones a partir del dgvReinversiones
        private void LlenaDgvTotalesReinversiones()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("TotalesReinversiones");
            DataRow row = ds.Tables["TotalesReinversiones"].NewRow();

            List<double> listVals = new List<double>();
            for (int i = this.proyecto.AnoInicial + 1; i <= this.proyecto.AnoInicial + this.proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                double val = 0;
                foreach (RequerimientoReinversion reqReinv in this.proyecto.RequerimientosReinversion)
                {
                    if (reqReinv.AnoReinversion == i)
                    {
                        val += reqReinv.Subtotal;
                    }
                }
                listVals.Add(val);
            }

            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                int anoActual = proyecto.AnoInicial + i;
                ds.Tables["TotalesReinversiones"].Columns.Add(anoActual.ToString(), Type.GetType("System.String"));
                row[anoActual.ToString()] = "₡ " + listVals[i - 1].ToString("#,##0.##");
            }//for

            ds.Tables["TotalesReinversiones"].Rows.Add(row);
            DataTable dtTotalReinversiones = ds.Tables["TotalesReinversiones"];
            this.dgvTotalesReinversiones.DataSource = dtTotalReinversiones;
            this.dgvTotalesReinversiones.ReadOnly = true;
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
                ds.Tables["Proyecciones"].Columns.Add("Codigo", Type.GetType("System.String"));
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
                    row["Codigo"] = proyTemp.CodArticulo;
                    row["Articulo"] = proyTemp.NombreArticulo;
                    row["Unidad"] = proyTemp.UnidadMedida.NombreUnidad;
                    row["Enero"] = ("₡ " + proyTemp.DetallesProyeccionVenta[0].Subtotal.ToString("#,##0.##"));
                    row["Febrero"] = ("₡ " + proyTemp.DetallesProyeccionVenta[1].Subtotal.ToString("#,##0.##"));
                    row["Marzo"] = ("₡ " + proyTemp.DetallesProyeccionVenta[2].Subtotal.ToString("#,##0.##"));
                    row["Abril"] = ("₡ " + proyTemp.DetallesProyeccionVenta[3].Subtotal.ToString("#,##0.##"));
                    row["Mayo"] = ("₡ " + proyTemp.DetallesProyeccionVenta[4].Subtotal.ToString("#,##0.##"));
                    row["Junio"] = ("₡ " + proyTemp.DetallesProyeccionVenta[5].Subtotal.ToString("#,##0.##"));
                    row["Julio"] = ("₡ " + proyTemp.DetallesProyeccionVenta[6].Subtotal.ToString("#,##0.##"));
                    row["Agosto"] = ("₡ " + proyTemp.DetallesProyeccionVenta[7].Subtotal.ToString("#,##0.##"));
                    row["Setiembre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[8].Subtotal.ToString("#,##0.##"));
                    row["Octubre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[9].Subtotal.ToString("#,##0.##"));
                    row["Noviembre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[10].Subtotal.ToString("#,##0.##"));
                    row["Diciembre"] = ("₡ " + proyTemp.DetallesProyeccionVenta[11].Subtotal.ToString("#,##0.##"));

                    ds.Tables["Proyecciones"].Rows.Add(row);
                }//foreach

                DataTable dtProyecciones = ds.Tables["Proyecciones"];
                this.dgvProyeccionesVentas.DataSource = dtProyecciones;
                this.dgvProyeccionesVentas.Columns["Codigo"].Visible = false;
            }//if
        }//LlenaDgvProyeccionesVentas

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
                ds.Tables["IngresosGenerados"].Columns.Add((this.proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(this.proyecto.AnoInicial + a).ToString()] = "₡ " + IngreGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["IngresosGenerados"].Rows.Add(row);

            DataTable dtIngresosGenerados = ds.Tables["IngresosGenerados"];
            dgvIngresosGenerados.DataSource = dtIngresosGenerados;
            dgvIngresosGenerados.Columns[0].HeaderText = "";
            
        }//LlenaDgvIngresosGenerados

        private void tbMontoFinanciamientoIF_TextChanged(object sender, EventArgs e)
        {
            LlenaDgvFinanciamientoIF();
        }

        private void nupTiempoFinanciamientoIF_ValueChanged(object sender, EventArgs e)
        {
            LlenaDgvFinanciamientoIF();
        }

        private void nupPorcentajeInteresIF_ValueChanged(object sender, EventArgs e)
        {
            LlenaDgvFinanciamientoIF();
        }

        private void btnGuardarFinanciamientoIF_Click(object sender, EventArgs e)
        {
            Financiamiento financiamientoTemp = new Financiamiento();
            financiamientoTemp.MontoFinanciamiento = Convert.ToDouble(tbMontoFinanciamientoIF.Text);
            financiamientoTemp.TiempoFinanciamiento = Convert.ToInt32(nupTiempoFinanciamientoIF.Value);
            financiamientoTemp.VariableFinanciamiento = false;
            InteresFinanciamiento intFinanTemp = new InteresFinanciamiento();
            intFinanTemp.PorcentajeInteresFinanciamiento = Convert.ToDouble(nupPorcentajeInteresIF.Value);
            intFinanTemp.VariableInteres = false;

            FinanciamientoData finanData = new FinanciamientoData();

            if (finanData.InsertarFinanciamiento(financiamientoTemp, this.proyecto.CodProyecto))
            {
                this.proyecto.FinanciamientoIF = financiamientoTemp;
                InteresFinanciamientoData intFinanData = new InteresFinanciamientoData();
                if (intFinanData.InsertarInteresFinanciamiento(intFinanTemp, this.proyecto.CodProyecto))
                {
                    this.proyecto.InteresFinanciamientoIF = intFinanTemp;
                    LlenaDgvFinanciamientoIF();
                    MessageBox.Show("Financiamiento fijo registrado con éxito",
                           "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("El Financiamiento no se han podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El Financiamiento no se han podido registrar",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarFinanciamientoIV_Click(object sender, EventArgs e)
        {
            Financiamiento financiamientoVariableTemp = new Financiamiento();
            financiamientoVariableTemp.MontoFinanciamiento = Convert.ToDouble(tbMontoVariable.Text.ToString());
            financiamientoVariableTemp.VariableFinanciamiento = true;
            financiamientoVariableTemp.TiempoFinanciamiento = Convert.ToInt32(nudTiempoVariable.Value);

            FinanciamientoData finanData = new FinanciamientoData();

            if (financiamientoVariableTemp.TiempoFinanciamiento.Equals(this.proyecto.InteresesFinanciamientoIV.Count))
            {
                if (finanData.InsertarFinanciamiento(financiamientoVariableTemp, this.proyecto.CodProyecto))
                {
                    this.proyecto.FinanciamientoIV = financiamientoVariableTemp;
                    LlenaDgvFinanciamientoIV();
                    MessageBox.Show("Financiamiento registrado con éxito",
                               "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("El Financiamiento no se han podido registrar",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Los porcentajes de interés y el tiempo de financiamiento no coinciden",
                               "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbMontoVariable_TextChanged(object sender, EventArgs e)
        {
            LlenaDgvFinanciamientoIV();
        }

        private void nudTiempoVariable_ValueChanged(object sender, EventArgs e)
        {
            //LlenaDgvFinanciamientoIV();
        }

        private void llInteresesVariables_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarMensajeSeguridad = false;
            Financiamiento financiamientoV = new Financiamiento();
            Double myInt;
            bool isNumerical = Double.TryParse(tbMontoVariable.Text.ToString(), out myInt);
            if (isNumerical && myInt > 0)
            {
                financiamientoV.MontoFinanciamiento = myInt;
            }
            if (Convert.ToInt32(nudTiempoVariable.Value)>0)
            {
                financiamientoV.TiempoFinanciamiento = Convert.ToInt32(nudTiempoVariable.Value);
            }
            financiamientoV.TiempoFinanciamiento = Convert.ToInt32(nudTiempoVariable.Value);
            financiamientoV.VariableFinanciamiento = true;
            proyecto.FinanciamientoIV = financiamientoV;
            InteresFinanciamientoUI interesesFinanciamiento = new InteresFinanciamientoUI(evaluador, proyecto);
            interesesFinanciamiento.MdiParent = this.MdiParent;
            interesesFinanciamiento.Show();
            this.Close();
        }

        private void LlenaDgvDepreciaciones()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("Depreciaciones");

            ds.Tables["Depreciaciones"].Columns.Add("Descripcion", Type.GetType("System.String"));
            for(int i = 1; i <=this.proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                ds.Tables["Depreciaciones"].Columns.Add((this.proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
            }

            for (int i = 0; i < this.proyecto.Depreciaciones.Count; i++)
            {
                DataRow row = ds.Tables["Depreciaciones"].NewRow();
                row["Descripcion"] = this.proyecto.Depreciaciones[i].NombreDepreciacion;

                for (int a = 1; a <= this.proyecto.HorizonteEvaluacionEnAnos; a++)
                {
                    row[(this.proyecto.AnoInicial + a).ToString()] = "₡ " + this.proyecto.Depreciaciones[i].MontoDepreciacion[a - 1].ToString("#,##0.##"); 
                }
                   
                ds.Tables["Depreciaciones"].Rows.Add(row);

            }//foreach

            DataTable dtDepreciaciones = ds.Tables["Depreciaciones"];
            this.dgvDepreciaciones.DataSource = dtDepreciaciones;
        }

        private void LlenaDgvCostos()
        {
            if ((this.proyecto.Costos != null) && (this.proyecto.Costos.Count > 0))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("Costos");
                ds.Tables["Costos"].Columns.Add("Codigo", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Articulo", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Unidad", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("Categoria", Type.GetType("System.String"));
                ds.Tables["Costos"].Columns.Add("AñoInicial", Type.GetType("System.String"));
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
                    row["Codigo"] = costoTemp.CodCosto;
                    row["Articulo"] = costoTemp.NombreCosto;
                    row["Unidad"] = costoTemp.UnidadMedida.NombreUnidad;
                    row["Categoria"] = costoTemp.CategoriaCosto;
                    row["AñoInicial"] = costoTemp.AnoCosto;
                    row["Enero"] = "₡ " + costoTemp.CostosMensuales[0].Subtotal.ToString("#,##0.##");
                    row["Febrero"] = "₡ " + costoTemp.CostosMensuales[1].Subtotal.ToString("#,##0.##");
                    row["Marzo"] = "₡ " + costoTemp.CostosMensuales[2].Subtotal.ToString("#,##0.##");
                    row["Abril"] = "₡ " + costoTemp.CostosMensuales[3].Subtotal.ToString("#,##0.##");
                    row["Mayo"] = "₡ " + costoTemp.CostosMensuales[4].Subtotal.ToString("#,##0.##");
                    row["Junio"] = "₡ " + costoTemp.CostosMensuales[5].Subtotal.ToString("#,##0.##");
                    row["Julio"] = "₡ " + costoTemp.CostosMensuales[6].Subtotal.ToString("#,##0.##");
                    row["Agosto"] = "₡ " + costoTemp.CostosMensuales[7].Subtotal.ToString("#,##0.##");
                    row["Setiembre"] = "₡ " + costoTemp.CostosMensuales[8].Subtotal.ToString("#,##0.##");
                    row["Octubre"] = "₡ " + costoTemp.CostosMensuales[9].Subtotal.ToString("#,##0.##");
                    row["Noviembre"] = "₡ " + costoTemp.CostosMensuales[10].Subtotal.ToString("#,##0.##");
                    row["Diciembre"] = "₡ " + costoTemp.CostosMensuales[11].Subtotal.ToString("#,##0.##");

                    ds.Tables["Costos"].Rows.Add(row);

                }//foreach

                DataTable dtCostos = ds.Tables["Costos"];
                this.dgvCostos.DataSource = dtCostos;
                this.dgvCostos.Columns["Codigo"].Visible = false;
                this.dgvCostos.Columns[3].HeaderText = "Año Inicial";
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
                ds.Tables["CostosGenerados"].Columns.Add((this.proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(this.proyecto.AnoInicial + a).ToString()] = "₡ " + costoGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["CostosGenerados"].Rows.Add(row);

            DataTable dtCostosGenerados = ds.Tables["CostosGenerados"];
            dgvCostosGenerados.DataSource = dtCostosGenerados;
            dgvCostosGenerados.Columns[0].HeaderText = "";

        }//LlenaDgvCostosGenerados

        private void btnEditarProyeccion_Click(object sender, EventArgs e)
        {

        }

        private void btnEditarCosto_Click(object sender, EventArgs e)
        {

        }

        private void EsRepresentanteIndividual_CheckedChanged(object sender, EventArgs e)
        {
            if (EsRepresentanteIndividual.Checked)
            {
                cbxTipoOrganizacion.SelectedIndex = cbxTipoOrganizacion.Items.Count - 1;
                txbNombreOrganizacion.Text = txbNombreProponente.Text +" "+ txbApellidos.Text;
                txbCedulaJuridica.Text = txbCedulaProponente.Text;
                txbTelefonoOrganizacion.Text = txbTelefonoProponente.Text;
            }
            else
            {
                cbxTipoOrganizacion.SelectedValue = this.proyecto.Proponente.Organizacion.Tipo.CodTipo;
                txbNombreOrganizacion.Text = this.proyecto.Proponente.Organizacion.NombreOrganizacion;
                txbCedulaJuridica.Text = this.proyecto.Proponente.Organizacion.CedulaJuridica;
                txbTelefonoOrganizacion.Text = this.proyecto.Proponente.Organizacion.Telefono;
            }
        }

        private void btnEliminarInversion_Click(object sender, EventArgs e)
        {
            if (dgvInversiones.SelectedRows[0].Cells[1].Value != null)
            {
                int codInversion = Convert.ToInt32(dgvInversiones.SelectedRows[0].Cells[1].Value.ToString());
                bool exist = false;
                foreach (RequerimientoReinversion reinversion in this.proyecto.RequerimientosReinversion)
                {
                    if (codInversion.Equals(reinversion.CodRequerimientoInversion))
                    {
                        exist = true;
                    }
                }
                if (!exist)
                {
                    RequerimientoInversionData reqInvData = new RequerimientoInversionData();
                    bool res = reqInvData.EliminarRequerimientoInversion(codInversion);
                    
                    if (res)
                    {
                        dgvInversiones.Rows.RemoveAt(dgvInversiones.SelectedRows[0].Index);
                        this.proyecto.RequerimientosInversion.Remove(this.proyecto.RequerimientosInversion.FindLast(r => r.CodRequerimientoInversion.Equals(codInversion)));

                        double total = 0;
                        for (int i = 0; i < dgvInversiones.RowCount - 1; i++)
                        {
                            if ((this.dgvInversiones.Rows[i].Cells["Subtotal"].Value != null) ||
                                (this.dgvInversiones.Rows[i].Cells["Subtotal"].Value.ToString() != ""))
                            {
                                total += float.Parse(this.dgvInversiones.Rows[i].Cells["Subtotal"].Value.ToString());
                            }//if
                        }//for
                        lblTotalInversiones.Text = "₡ " + total.ToString("#,##0.##");

                        MessageBox.Show("Inversión eliminada con éxito",
                               "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("La inversión no se ha podido eliminar",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("La inversión no se puede eliminar, posee reinversiones asociadas",
                           "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LlenaDgvCapitalTrabajo()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("CapitalTrabajo");
            ds.Tables["CapitalTrabajo"].Columns.Add("Rubro", Type.GetType("System.String"));

            DataRow row = ds.Tables["CapitalTrabajo"].NewRow();
            row["Rubro"] = "Costos variables";
            int a = 1;
            ds.Tables["CapitalTrabajo"].Columns.Add(this.proyecto.AnoInicial.ToString(), Type.GetType("System.String"));
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                ds.Tables["CapitalTrabajo"].Columns.Add((this.proyecto.AnoInicial + a).ToString(), Type.GetType("System.String"));
                row[(this.proyecto.AnoInicial + a).ToString()] = "₡ " + costoGenerado.ToString("#,##0.##");
                a++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row);

            //llena fila de capital de trabajo
            DataRow row2 = ds.Tables["CapitalTrabajo"].NewRow();
            row2["Rubro"] = "Capital de trabajo";
            int a2 = 1;
            foreach (double costoGenerado in proyecto.CostosGenerados)
            {
                row2[(this.proyecto.AnoInicial + a2).ToString()] = "₡ " + ((costoGenerado / 12) * 1.5).ToString("#,##0.##");
                a2++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row2);

            //llena incremental

            Double recCT = 0;
            Double val = 0;
            DataRow row3 = ds.Tables["CapitalTrabajo"].NewRow();
            row3["Rubro"] = "Incremental";

            val = -((proyecto.CostosGenerados[0] / 12) * 1.5);
            row3[this.proyecto.AnoInicial.ToString()] = "₡ " + (val).ToString("#,##0.##");
            recCT = recCT + val;

            int a3 = 1;
            for (int i=1; i<proyecto.CostosGenerados.Count; i++)
            {
                val = ((proyecto.CostosGenerados[i] / 12) * 1.5) - ((proyecto.CostosGenerados[i - 1] / 12) * 1.5);
                row3[(this.proyecto.AnoInicial + a3).ToString()] = "₡ " + (val).ToString("#,##0.##");
                recCT = recCT + val;
                a3++;
            }
            ds.Tables["CapitalTrabajo"].Rows.Add(row3);

            DataTable dtCapitalTrabajo = ds.Tables["CapitalTrabajo"];
            dgvCapitalTrabajo.DataSource = dtCapitalTrabajo;

            lblRecuperacionCT.Text = "₡ " + recCT.ToString("#,##0.##");
        }

        private void nudPersonasParticipantes_ValueChanged(object sender, EventArgs e)
        {
            LlenaVANDivide();
        }

        private void LlenaVANDivide()
        {
            tbxVANPersonas.Text = "₡" + (Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty).ToString()) / 
                Convert.ToDouble(nudPersonasParticipantes.Value)).ToString("#,##0.##");

            tbxVANFamilias.Text = "₡" + (Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty).ToString()) /
                Convert.ToDouble(nudFamiliasInvolucradas.Value)).ToString("#,##0.##");
        }

        private void nudFamiliasInvolucradas_ValueChanged(object sender, EventArgs e)
        {
            LlenaVANDivide();
        }

        private void btnEliminarReinversion_Click(object sender, EventArgs e)
        {
            if (dgvReinversiones.SelectedRows[0].Cells[1].Value != null)
            {
                int codReinversion = Convert.ToInt32(dgvReinversiones.SelectedRows[0].Cells[2].Value.ToString());
                RequerimientoReinversionData reqReinvData = new RequerimientoReinversionData();

                if (reqReinvData.EliminarRequerimientoReinversion(codReinversion))
                {
                    dgvReinversiones.Rows.RemoveAt(dgvReinversiones.SelectedRows[0].Index);
                    this.proyecto.RequerimientosReinversion.Remove(this.proyecto.RequerimientosReinversion.FindLast(r => r.CodRequerimientoReinversion.Equals(codReinversion)));
                    LlenaDgvTotalesReinversiones();

                    MessageBox.Show("Reinversión eliminada con éxito",
                           "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("La reinversión no se han podido eliminar",
                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminarProyeccionVentas_Click(object sender, EventArgs e)
        {
            int codProyeccionArticulo = Convert.ToInt32(dgvProyeccionesVentas.SelectedRows[0].Cells[0].Value.ToString());
            ProyeccionVentaArticuloData proyeccionArticulosData = new ProyeccionVentaArticuloData();

            if (proyeccionArticulosData.EliminarProyeccionVenta(codProyeccionArticulo))
            {
                dgvProyeccionesVentas.Rows.RemoveAt(dgvProyeccionesVentas.SelectedRows[0].Index);
                this.proyecto.Proyecciones.Remove(this.proyecto.Proyecciones.FindLast(r => r.CodArticulo.Equals(codProyeccionArticulo)));
                LlenaDgvIngresosGenerados();
                
                MessageBox.Show("Proyección de venta de articulo eliminada con éxito",
                       "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("La proyección de venta de articulo no se han podido eliminar",
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarCosto_Click(object sender, EventArgs e)
        {
            int codCosto = Convert.ToInt32(dgvCostos.SelectedRows[0].Cells[1].Value.ToString());
            CostoData costoData = new CostoData();

            if (costoData.EliminarCosto(codCosto))
            {
                dgvCostos.Rows.RemoveAt(dgvCostos.SelectedRows[0].Index);
                this.proyecto.Costos.Remove(this.proyecto.Costos.FindLast(r => r.CodCosto.Equals(codCosto)));
                LlenaDgvCostosGenerados();

                MessageBox.Show("Costo eliminado con éxito",
                       "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("El costo no se han podido eliminar",
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarFlujoCaja_Click(object sender, EventArgs e)
        {
            this.proyecto.PersonasBeneficiadas = Convert.ToInt32(nudBeneficiariosIndirectos.Value);
            this.proyecto.PersonasParticipantes = Convert.ToInt32(nudPersonasParticipantes.Value);
            this.proyecto.TasaCostoCapital = Convert.ToDouble(nudTasaCostoCapital.Value);
            this.proyecto.FamiliasInvolucradas = Convert.ToInt32(nudFamiliasInvolucradas.Value);
            ProyectoData proyectoData = new ProyectoData();
            if (proyectoData.ActualizarProyectoFlujoCaja(this.proyecto))
            {
                MessageBox.Show("Proyecto actualizado con éxito",
                                "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ocurrió un error en la actualización", "No actualizado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenaDgvFinanciamientoIF()
        {
            if (proyecto.FinanciamientoIF != null)
            {
                Double myInt;
                bool isNumerical = Double.TryParse(tbMontoFinanciamientoIF.Text.ToString(), out myInt);
                if (isNumerical && myInt > 0 && Convert.ToDouble(nupTiempoFinanciamientoIF.Value) > 0 && Convert.ToDouble(nupPorcentajeInteresIF.Value) > 0)
                {
                    double monto = Convert.ToDouble(tbMontoFinanciamientoIF.Text);
                    double tiempo = Convert.ToDouble(nupTiempoFinanciamientoIF.Value);
                    double interesIF = Convert.ToDouble(nupPorcentajeInteresIF.Value);
                    double interesIFtemp = interesIF / 100;
                    double cuota = Math.Round((monto * interesIFtemp) / (1 - (Math.Pow((1 + interesIFtemp), (-tiempo)))), 2);

                    DataSet ds = new DataSet();
                    ds.Tables.Add("AmortizacionPrestamo");
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo1");
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo2");
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo3");
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo4");
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo5");

                    for (int i = 1; i <= tiempo; i++)
                    {
                        DataRow row1 = ds.Tables["AmortizacionPrestamo"].NewRow();
                        row1["titulo1"] = this.proyecto.AnoInicial + i;

                        row1["titulo2"] = "₡ " + monto.ToString("#,##0.##"); 

                        row1["titulo3"] = "₡ " + cuota.ToString("#,##0.##");

                        double interes = Math.Round((monto * interesIFtemp), 2);
                        row1["titulo4"] = "₡ " + interes.ToString("#,##0.##");

                        double amortizacion = Math.Round(cuota - interes, 2);
                        row1["titulo5"] = "₡ " + amortizacion.ToString("#,##0.##");

                        ds.Tables["AmortizacionPrestamo"].Rows.Add(row1);

                        monto = Math.Round((monto - amortizacion), 2);
                        if (monto < 1)
                        {
                            monto = 0;
                        }
                    }
                    DataTable dtCostosGenerados = ds.Tables["AmortizacionPrestamo"];
                    dgvFinanciamientoIF.DataSource = dtCostosGenerados;
                    dgvFinanciamientoIF.Columns[0].HeaderText = "Año de Pago";
                    dgvFinanciamientoIF.Columns[0].Width = 130;
                    dgvFinanciamientoIF.Columns[1].Width = 160;
                    dgvFinanciamientoIF.Columns[2].Width = 150;
                    dgvFinanciamientoIF.Columns[3].Width = 150;
                    dgvFinanciamientoIF.Columns[4].Width = 160;
                    dgvFinanciamientoIF.Columns[1].HeaderText = "Saldo";
                    dgvFinanciamientoIF.Columns[2].HeaderText = "Cuota";
                    dgvFinanciamientoIF.Columns[3].HeaderText = "Interés";
                    dgvFinanciamientoIF.Columns[4].HeaderText = "Amortización";
                    
                }
            }
        }

        private void LlenaDgvFinanciamientoIV()
        {
            Double myInt;
            bool isNumerical = Double.TryParse(tbMontoVariable.Text.ToString(), out myInt);
            if (isNumerical && myInt > 0 && Convert.ToDouble(nudTiempoVariable.Value) > 0 && proyecto.InteresesFinanciamientoIV.Count > 0)
            {
                if (proyecto.FinanciamientoIV != null)
                {
                    double monto = proyecto.FinanciamientoIV.MontoFinanciamiento;
                    int tiempo = proyecto.FinanciamientoIV.TiempoFinanciamiento;
                    List<InteresFinanciamiento> intereses = new List<InteresFinanciamiento>();
                    intereses = proyecto.InteresesFinanciamientoIV;

                    DataSet ds = new DataSet();
                    ds.Tables.Add("AmortizacionPrestamo");
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo1", Type.GetType("System.String"));
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo2", Type.GetType("System.String"));
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo3", Type.GetType("System.String"));
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo4", Type.GetType("System.String"));
                    ds.Tables["AmortizacionPrestamo"].Columns.Add("titulo5", Type.GetType("System.String"));
                    int tiempoTemp = tiempo;

                    for (int i = 1; i <= tiempo; i++)
                    {
                        Double interesTemp = intereses[i-1].PorcentajeInteresFinanciamiento / 100;

                        DataRow row1 = ds.Tables["AmortizacionPrestamo"].NewRow();
                        row1["titulo1"] = this.proyecto.AnoInicial + i;

                        row1["titulo2"] = "₡ " + monto.ToString("#,##0.##");

                        double cuota = Math.Round((monto * interesTemp) / (1 - (Math.Pow((1 + interesTemp), (-tiempoTemp)))), 2);
                        row1["titulo3"] = "₡ " + cuota.ToString("#,##0.##");

                        double interes = Math.Round((monto * interesTemp), 2);
                        row1["titulo4"] = "₡ " + interes.ToString("#,##0.##");

                        double amortizacion = Math.Round(cuota - interes, 2);
                        row1["titulo5"] = "₡ " + amortizacion.ToString("#,##0.##");

                        ds.Tables["AmortizacionPrestamo"].Rows.Add(row1);

                        monto = Math.Round((monto - amortizacion), 2);
                        tiempoTemp--;
                    }

                    DataTable dtCostosGenerados = ds.Tables["AmortizacionPrestamo"];
                    dgvFinanciamientoVariable.DataSource = dtCostosGenerados;
                    dgvFinanciamientoVariable.Columns[0].HeaderText = "Año de Pago";
                    dgvFinanciamientoVariable.Columns[0].Width = 130;
                    dgvFinanciamientoVariable.Columns[1].Width = 160;
                    dgvFinanciamientoVariable.Columns[2].Width = 150;
                    dgvFinanciamientoVariable.Columns[3].Width = 150;
                    dgvFinanciamientoVariable.Columns[4].Width = 160;
                    dgvFinanciamientoVariable.Columns[1].HeaderText = "Saldo";
                    dgvFinanciamientoVariable.Columns[2].HeaderText = "Cuota";
                    dgvFinanciamientoVariable.Columns[3].HeaderText = "Interés";
                    dgvFinanciamientoVariable.Columns[4].HeaderText = "Amortización";
                }

            }
        }

        private void LlenaDgvFlujoCajaIF()
        {
            if (proyecto.RequerimientosInversion.Count != 0 && proyecto.Costos.Count != 0 && proyecto.Proyecciones.Count != 0 && proyecto.FinanciamientoIF != null)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("FlujoCajaIntFijo");
                ds.Tables["FlujoCajaIntFijo"].Columns.Add("Rubro", Type.GetType("System.String"));
                for (int i = 0; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    ds.Tables["FlujoCajaIntFijo"].Columns.Add((this.proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
                }

                #region ventas
                DataRow row = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row["Rubro"] = "Ventas";
                int a = 1;
                foreach (double IngreGenerado in proyecto.IngresosGenerados)
                {
                    row[(this.proyecto.AnoInicial + a).ToString()] = "₡ " + IngreGenerado.ToString("#,##0.##");
                    a++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row);
                #endregion

                #region costos
                DataRow row2 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row2["Rubro"] = "Costos Totales";
                int a2 = 1;
                foreach (double costoGenerado in proyecto.CostosGenerados)
                {
                    row2[(this.proyecto.AnoInicial + a2).ToString()] = "₡ " + (-costoGenerado).ToString("#,##0.##");
                    a2++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row2);
                #endregion

                #region depreciaciones
                DataRow row3 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row3["Rubro"] = "Depreciaciones";
                int a3 = 1;
                foreach (double depreciacionAnual in proyecto.TotalDepreciaciones)
                {
                    row3[(this.proyecto.AnoInicial + a3).ToString()] = "₡ -" + depreciacionAnual.ToString("#,##0.##");
                    a3++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row3);
                #endregion

                #region utilidad operativa
                DataRow row4 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row4["Rubro"] = "Utilidad Operativa";
                int a4 = 1;
                foreach (double utilidad in proyecto.UtilidadOperativa)
                {
                    row4[(this.proyecto.AnoInicial + a4).ToString()] = "₡ " + utilidad.ToString("#,##0.##");
                    a4++;
                }

                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row4);
                #endregion

                #region intereses
                DataRow row5 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row5["Rubro"] = "Intereses";
                int a5 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    if (i < dgvFinanciamientoIF.Rows.Count)
                    {
                        row5[(this.proyecto.AnoInicial + a5).ToString()] = "₡ -" + dgvFinanciamientoIF.Rows[i].Cells[2].Value.ToString();
                    }
                    else
                    {
                        row5[(this.proyecto.AnoInicial + a5).ToString()] = "₡ 0";
                    }
                    a5++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row5);
                #endregion

                #region utilidad sin impuesto
                DataRow row6 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row6["Rubro"] = "Utilidad Antes de Impuesto";
                int a6 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    string val = ds.Tables["FlujoCajaIntFijo"].Rows[4].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty);
                    row6[(this.proyecto.AnoInicial + a6).ToString()] = "₡ " + Math.Round(this.proyecto.UtilidadOperativa[i] + Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[4].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                    a6++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row6);
                #endregion

                #region impuesto
                DataRow row7 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row7["Rubro"] = "Impuesto (% Impuestos)";
                int a7 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    double val = Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[5].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty));
                    double valImp = 0;
                    if (this.proyecto.PagaImpuesto)
                    {
                        valImp = this.proyecto.PorcentajeImpuesto;
                    }

                    row7[(this.proyecto.AnoInicial + a7).ToString()] = "₡ " + Math.Round(((valImp * val) / 100), 2).ToString("#,##0.##");
                    a7++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row7);
                #endregion

                #region utilidad neta
                DataRow row8 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row8["Rubro"] = "Utilidad Neta";
                int a8 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row8[(this.proyecto.AnoInicial + a8).ToString()] = "₡ " + Math.Round(Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[5].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)) - Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[6].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                    a8++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row8);
                #endregion

                #region depreciaciones
                DataRow row9 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row9["Rubro"] = "Depreciaciones";
                int a9 = 1;
                foreach (double depreciacionAnual in proyecto.TotalDepreciaciones)
                {
                    row9[(this.proyecto.AnoInicial + a9).ToString()] = "₡ " + depreciacionAnual.ToString("#,##0.##");
                    a9++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row9);
                #endregion

                #region flujo operativo
                DataRow row10 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row10["Rubro"] = "Flujo Operativo";
                int a10 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row10[(this.proyecto.AnoInicial + a10).ToString()] = "₡ " + Math.Round(Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[7].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)) + Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[8].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                    a10++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row10);
                #endregion

                #region inversiones
                DataRow row11 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row11["Rubro"] = "Inversiones";
                int a11 = 0;
                row11[(this.proyecto.AnoInicial + a11).ToString()] = "₡ -" + lblTotalInversiones.Text.ToString().Replace("₡", string.Empty);
                a11++;
                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row11[(this.proyecto.AnoInicial + a11).ToString()] = "₡ -" + dgvTotalesReinversiones.Rows[0].Cells[i].Value.ToString().Replace("₡ ", string.Empty);
                    a11++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row11);
                #endregion

                #region prestamo
                DataRow row12 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row12["Rubro"] = "Préstamo";
                int a12 = 0;
                row12[(this.proyecto.AnoInicial + a12).ToString()] = "₡ " + this.proyecto.FinanciamientoIF.MontoFinanciamiento;
                a12++;
                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    if (i < dgvFinanciamientoIF.Rows.Count)
                    {
                        row12[(this.proyecto.AnoInicial + a12).ToString()] = "₡ -" + dgvFinanciamientoIF.Rows[i].Cells[3].Value.ToString();
                    }
                    else
                    {
                        row12[(this.proyecto.AnoInicial + a12).ToString()] = "₡ 0";
                    }
                    a12++;
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row12);

                #endregion

                #region inv capital trabajo
                DataRow row13 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row13["Rubro"] = "Inv. Capital Trabajo";
                int a13 = 0;
                for (int i = 1; i < this.proyecto.HorizonteEvaluacionEnAnos + 1; i++)
                {
                    row13[(this.proyecto.AnoInicial + a13).ToString()] = dgvCapitalTrabajo.Rows[2].Cells[i].Value.ToString();
                    a13++;
                }
                row13[(this.proyecto.AnoInicial + a13).ToString()] = "₡ 0";
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row13);
                #endregion

                #region recuperacionCT
                DataRow row14 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row14["Rubro"] = "Recuperación CT";
                row14[(this.proyecto.AnoInicial + this.proyecto.HorizonteEvaluacionEnAnos).ToString()] = lblRecuperacionCT.Text.ToString();

                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row14);
                #endregion

                #region valorResidual
                DataRow row15 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row15["Rubro"] = "Valor Residual";
                row15[(this.proyecto.AnoInicial + this.proyecto.HorizonteEvaluacionEnAnos).ToString()] = "₡ " + this.proyecto.ValorResidual.ToString("#,##0.##");

                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row15);
                #endregion

                #region flujo efectivo
                DataRow row16 = ds.Tables["FlujoCajaIntFijo"].NewRow();
                row16["Rubro"] = "Flujo Efectivo";

                for (int i = 0; i <= this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row16[(this.proyecto.AnoInicial + i).ToString()] = "₡ " + Math.Round(
                        Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[9].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[9].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[10].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[10].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[11].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[11].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[12].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[12].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[13].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[13].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[14].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntFijo"].Rows[14].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString()))
                        , 2).ToString("#,##0.##");
                }
                ds.Tables["FlujoCajaIntFijo"].Rows.Add(row16);
                #endregion

                DataTable dtFlujoCajaFija = ds.Tables["FlujoCajaIntFijo"];
                dgvFlujoCajaIntFijo.DataSource = dtFlujoCajaFija;
                dgvFlujoCajaIntFijo.Columns[0].Width = 180;
                LlenaCalculosFinalesIntFijo();
            }
        }

        private void LlenaCalculosFinalesIntFijo()
        {
            double[] flujoEfectivo = new double[this.proyecto.HorizonteEvaluacionEnAnos + 1];
            double[] flujoEfectivoSinInicio = new double[this.proyecto.HorizonteEvaluacionEnAnos];

            for (int i = 0; i <= this.proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                flujoEfectivo[i] = Convert.ToDouble(dgvFlujoCajaIntFijo.Rows[15].Cells[i+1].Value.ToString().Replace("₡ ", string.Empty).ToString());

            }

            for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                flujoEfectivoSinInicio[i] = flujoEfectivo[i + 1];
            }

            double IRR = Microsoft.VisualBasic.Financial.IRR(ref flujoEfectivo, 0.3) * 100;
            double VNA = flujoEfectivo[0] + Microsoft.VisualBasic.Financial.NPV(Convert.ToDouble(nudTasaCostoCapital.Value), ref flujoEfectivoSinInicio);

            tbxTIR.Text = (IRR).ToString("#,##0.##") + " %";
            tbxVAN.Text = "₡ " + (VNA).ToString("#,##0.##");

            nudTasaCostoCapital.Value = Convert.ToDecimal(this.proyecto.TasaCostoCapital);
            nudBeneficiariosIndirectos.Value = Convert.ToDecimal(this.proyecto.PersonasBeneficiadas);
            nudPersonasParticipantes.Value = Convert.ToDecimal(this.proyecto.PersonasParticipantes);
            nudFamiliasInvolucradas.Value = Convert.ToDecimal(this.proyecto.FamiliasInvolucradas);
        }

        private void LlenaDgvFlujoCajaIV()
        {
            if (proyecto.RequerimientosInversion.Count != 0 && proyecto.Costos.Count != 0 && proyecto.Proyecciones.Count != 0 && proyecto.FinanciamientoIV != null)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add("FlujoCajaIntVariable");
                ds.Tables["FlujoCajaIntVariable"].Columns.Add("Rubro", Type.GetType("System.String"));
                for (int i = 0; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    ds.Tables["FlujoCajaIntVariable"].Columns.Add((this.proyecto.AnoInicial + i).ToString(), Type.GetType("System.String"));
                }

                #region ventas
                DataRow row = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row["Rubro"] = "Ventas";
                int a = 1;
                foreach (double IngreGenerado in proyecto.IngresosGenerados)
                {
                    
                    row[(this.proyecto.AnoInicial + a).ToString()] = "₡ " + IngreGenerado.ToString("#,##0.##");
                    a++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row);
                #endregion

                #region costos
                DataRow row2 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row2["Rubro"] = "Costos Totales";
                int a2 = 1;
                foreach (double costoGenerado in proyecto.CostosGenerados)
                {
                    //ds.Tables["FlujoCajaIntFijo"].Columns.Add((this.proyecto.AnoInicial + a2).ToString(), Type.GetType("System.String"));
                    row2[(this.proyecto.AnoInicial + a2).ToString()] = "₡ " + (-costoGenerado).ToString("#,##0.##");
                    a2++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row2);
                #endregion

                #region depreciaciones
                DataRow row3 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row3["Rubro"] = "Depreciaciones";
                int a3 = 1;
                foreach (double depreciacionAnual in proyecto.TotalDepreciaciones)
                {
                    row3[(this.proyecto.AnoInicial + a3).ToString()] = "₡ -" + depreciacionAnual.ToString("#,##0.##");
                    a3++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row3);
                #endregion

                #region utilidad operativa
                DataRow row4 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row4["Rubro"] = "Utilidad Operativa";
                int a4 = 1;
                foreach (double utilidad in proyecto.UtilidadOperativa)
                {
                    row4[(this.proyecto.AnoInicial + a4).ToString()] = "₡ " + utilidad.ToString("#,##0.##");
                    a4++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row4);
                #endregion

                #region intereses
                DataRow row5 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row5["Rubro"] = "Intereses";
                int a5 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    if (i < dgvFinanciamientoVariable.Rows.Count)
                    {
                        row5[(this.proyecto.AnoInicial + a5).ToString()] = "₡ -" + dgvFinanciamientoVariable.Rows[i].Cells[2].Value.ToString();
                    }
                    else
                    {
                        row5[(this.proyecto.AnoInicial + a5).ToString()] = "₡ 0";
                    }
                    a5++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row5);
                #endregion

                #region utilidad sin impuesto
                DataRow row6 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row6["Rubro"] = "Utilidad Antes de Impuesto";
                int a6 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row6[(this.proyecto.AnoInicial + a6).ToString()] = "₡ " + Math.Round(this.proyecto.UtilidadOperativa[i] + Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[4].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                    a6++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row6);
                #endregion

                #region impuesto
                DataRow row7 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row7["Rubro"] = "Impuesto (% Impuestos)";
                int a7 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    double val = Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[5].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty));
                    double valImp = 0;
                    if (this.proyecto.PagaImpuesto)
                    {
                        valImp = this.proyecto.PorcentajeImpuesto;
                    }

                    row7[(this.proyecto.AnoInicial + a7).ToString()] = "₡ " + Math.Round(((valImp * val) / 100), 2).ToString("#,##0.##");
                    a7++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row7);
                #endregion

                #region utilidad neta
                DataRow row8 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row8["Rubro"] = "Utilidad Neta";
                int a8 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row8[(this.proyecto.AnoInicial + a8).ToString()] = "₡ " + Math.Round(Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[5].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)) - Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[6].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                    a8++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row8);
                #endregion

                #region depreciaciones
                DataRow row9 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row9["Rubro"] = "Depreciaciones";
                int a9 = 1;
                foreach (double depreciacionAnual in proyecto.TotalDepreciaciones)
                {
                    row9[(this.proyecto.AnoInicial + a9).ToString()] = "₡ " + depreciacionAnual.ToString("#,##0.##");
                    a9++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row9);
                #endregion

                #region flujo operativo
                DataRow row10 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row10["Rubro"] = "Flujo Operativo";
                int a10 = 1;

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row10[(this.proyecto.AnoInicial + a10).ToString()] = "₡ " + Math.Round(Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[7].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)) + Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[8].ItemArray[i + 2].ToString().Replace("₡ ", string.Empty)), 2).ToString("#,##0.##");
                    a10++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row10);
                #endregion

                #region inversiones
                DataRow row11 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row11["Rubro"] = "Inversiones";
                int a11 = 0;
                row11[(this.proyecto.AnoInicial + a11).ToString()] = "₡ -" + lblTotalInversiones.Text.ToString().Replace("₡", string.Empty);
                a11++;
                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row11[(this.proyecto.AnoInicial + a11).ToString()] = "₡ -" + dgvTotalesReinversiones.Rows[0].Cells[i].Value.ToString();
                    a11++;
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row11);
                #endregion

                #region prestamo
                DataRow row12 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row12["Rubro"] = "Préstamo";
                int a12 = 0;
                row12[(this.proyecto.AnoInicial + a12).ToString()] = "₡ " + this.proyecto.FinanciamientoIV.MontoFinanciamiento;
                a12++;
                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    if (i < dgvFinanciamientoVariable.Rows.Count)
                    {
                        row12[(this.proyecto.AnoInicial + a12).ToString()] = "₡ -" + dgvFinanciamientoVariable.Rows[i].Cells[3].Value.ToString();
                    }
                    else
                    {
                        row12[(this.proyecto.AnoInicial + a12).ToString()] = "₡ 0";
                    }
                    a12++;
                }

                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row12);
                #endregion

                #region inv capital trabajo
                DataRow row13 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row13["Rubro"] = "Inv. Capital Trabajo";
                int a13 = 0;
                for (int i = 1; i < this.proyecto.HorizonteEvaluacionEnAnos + 1; i++)
                {
                    row13[(this.proyecto.AnoInicial + a13).ToString()] = dgvCapitalTrabajo.Rows[2].Cells[i].Value.ToString();
                    a13++;
                }
                row13[(this.proyecto.AnoInicial + a13).ToString()] = "₡ 0";
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row13);
                #endregion

                #region recuperacionCT
                DataRow row14 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row14["Rubro"] = "Recuperación CT";
                row14[(this.proyecto.AnoInicial + this.proyecto.HorizonteEvaluacionEnAnos).ToString()] = lblRecuperacionCT.Text.ToString();

                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row14);
                #endregion

                #region valorResidual
                DataRow row15 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row15["Rubro"] = "Valor Residual";
                row15[(this.proyecto.AnoInicial + this.proyecto.HorizonteEvaluacionEnAnos).ToString()] = "₡ " + this.proyecto.ValorResidual.ToString("#,##0.##");

                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row15);
                #endregion

                #region flujo efectivo
                DataRow row16 = ds.Tables["FlujoCajaIntVariable"].NewRow();
                row16["Rubro"] = "Flujo Efectivo";

                for (int i = 0; i <= this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    row16[(this.proyecto.AnoInicial + i).ToString()] = "₡ " + Math.Round(
                        Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[9].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[9].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[10].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[10].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[11].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[11].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[12].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[12].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[13].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[13].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString())) +
                        Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[14].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).Equals(string.Empty) ? 0 : Convert.ToDouble(ds.Tables["FlujoCajaIntVariable"].Rows[14].ItemArray[i + 1].ToString().Replace("₡ ", string.Empty).ToString()))
                        , 2).ToString("#,##0.##");
                }
                ds.Tables["FlujoCajaIntVariable"].Rows.Add(row16);
                #endregion
                DataTable dtFlujoCajaFija = ds.Tables["FlujoCajaIntVariable"];
                dgvFlujoCajaIntVariable.DataSource = dtFlujoCajaFija;
                dgvFlujoCajaIntVariable.Columns[0].Width = 180;
            }
        }
        /**************************************FIN Metodos de utilidad*****************************************/
    }//Clase registrar proyecto
}//namespace