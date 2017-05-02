//@Copyright Yordan Campos Piedra
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;
using UCR.Negotium.WindowsUI.DatasetExtension;
using UCR.Negotium.WindowsUI.Extensions;

namespace UCR.Negotium.WindowsUI
{
    public partial class RegistrarProyectoWindow : Form
    {
        #region variables
        bool mostroMensaje = false, mostrarMensajeSeguridad = true;
        int idProvinciaSeleccionada = 0, idCantonSeleccionado = 0, idDistritoSeleccionado = 0;
        bool segundaSeleccionProvincia = false, segundaSeleccionCanton = false, segundaSeleccionDistrito = false;
        Proyecto proyecto;
        Encargado evaluador;
        #endregion

        #region constructor
        //Constructor sobrecargado que solo recibe al evaluador que esta logeado como parametro
        public RegistrarProyectoWindow(Encargado evaluador)
        {
            this.evaluador = evaluador;
            InitializeComponent();
            LlenaComboProvincia();
            LlenaComboCantones();
            LlenarComboDistritos();
            LlenarComboUnidadMedida();
            LlenaComboTipoOrganizacion();
            lblNombreEvaluador.Text = evaluador.Nombre;
            InformacionToolTip();
            mostrarMensajeSeguridad = true;
        }

        //constructor sobrecargado que se utiliza para abrir proyectos con informacion en memoria volatil
        public RegistrarProyectoWindow(Encargado evaluador, Proyecto proyecto, int indexTab)
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
            mostrarMensajeSeguridad = true;
            LlenaFooter();

            tbxRegistrarProyecto.SelectedIndex = indexTab;
            tbxRegistrarProyecto_Selected(this, null);
        }
        #endregion

        #region generales

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
            chbConIngreso.Checked = proyecto.ConIngresos ? true : false;
            chbSinIngreso.Checked = !proyecto.ConIngresos ? true : false;
            //ObjetoInteresProyecto objetoInteres = GetObjetoInteres();
            txbObjetoInteres.Text = proyecto.ObjetoInteres.DescripcionObjetoInteres;
            cbxUnidadMedida.SelectedValue = proyecto.ObjetoInteres.UnidadMedida.CodUnidad;
            nudAnoInicialProyecto.Value = proyecto.AnoInicial;
            nudHorizonteEvaluacion.Value = proyecto.HorizonteEvaluacionEnAnos;
            chbSiPagaImpuesto.Checked = proyecto.PagaImpuesto ? true : false;
            chbNoPagaImpuesto.Checked = !proyecto.PagaImpuesto ? true : false;
            nudPorcentajeImpuesto.Value = (Decimal)proyecto.PorcentajeImpuesto;
            txbDescripcionPoblacionBeneficiaria.Text = proyecto.DescripcionPoblacionBeneficiaria;
            txbJustificacionMercado.Text = proyecto.JustificacionDeMercado;
            txbCaracterizacionBienServicio.Text = proyecto.CaraterizacionDelBienServicio;
            txbDescripcionSostenibilidadProyecto.Text = proyecto.DescripcionSostenibilidadDelProyecto;
        }//LlenaInformacionProyecto

        private void LlenarComboUnidadMedida()
        {
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            cbxUnidadMedida.DataSource = unidadMedidaData.GetUnidadesMedida();
            cbxUnidadMedida.DisplayMember = "nombre_unidad";
            cbxUnidadMedida.ValueMember = "cod_unidad";
        }

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

        private void LlenaFooter()
        {
            string proponente = (proyecto.Proponente != null && proyecto.Proponente.Nombre!= null && proyecto.Proponente.Apellidos != null) ? " - Proponente: " + proyecto.Proponente.Nombre + " " + proyecto.Proponente.Apellidos : string.Empty;
            string info = "Info: Nombre del Proyecto: " + proyecto.NombreProyecto +
                " - Horizonte del Proyecto: " + proyecto.HorizonteEvaluacionEnAnos + " Años" +
                " - Año de Inicio del Proyecto: " + proyecto.AnoInicial +
                proponente;

            lblFoo1.Text = lblFoo2.Text = lblFoo3.Text = lblFoo4.Text = lblFoo5.Text = lblFoo6.Text = lblFoo7.Text = lblFoo8.Text = info;
        }

        // Este metodo se encarga de modificar los mensajes que van a salir en los textbox 
        // para mostrar los tips
        private void InformacionToolTip()
        {
            this.ttMensaje.SetToolTip(this.txbNombreProyecto, "Ingrese un nombre para el proyecto");
            this.ttMensaje.SetToolTip(this.txbResumenEjecutivo, "Esta es la información del resumen ejecutivo");
            this.ttMensaje.SetToolTip(this.txbObjetoInteres, "Este campo es para especificar cual va a ser el bien que será evaluado");
            this.ttMensaje.SetToolTip(this.nudAnoInicialProyecto, "Especifica ¿cuándo se va a iniciar el proyecto?");
            this.ttMensaje.SetToolTip(this.nudHorizonteEvaluacion, "El horizonte del proyecto en años");
        }//InformacionToolTip

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
                    case 1:
                        if(proyecto.Proponente != null)
                        {
                            LlenaInformacionProponente();
                            LlenaInformacionOrganizacion();
                        }
                        break;
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
                        if (proyecto.FinanciamientoIF.CodFinanciamiento.Equals(0))
                        {
                            proyecto.FinanciamientoIF = financiamientoData.GetFinanciamiento(proyecto.CodProyecto, false);
                        }

                        if (!proyecto.FinanciamientoIF.CodFinanciamiento.Equals(0))
                        {
                            tbMontoFinanciamientoIF.Text = proyecto.FinanciamientoIF.MontoFinanciamiento.ToString();
                            nupTiempoFinanciamientoIF.Value = (Decimal)proyecto.FinanciamientoIF.TiempoFinanciamiento;
                            if (proyecto.InteresFinanciamientoIF.PorcentajeInteresFinanciamiento != 0)
                            {
                                nupPorcentajeInteresIF.Value = (Decimal)proyecto.InteresFinanciamientoIF.PorcentajeInteresFinanciamiento;
                            }
                            LlenaDgvFinanciamientoIF();
                        }

                        if (proyecto.FinanciamientoIV.CodFinanciamiento.Equals(0) && proyecto.FinanciamientoIV.MontoFinanciamiento.Equals(0) && proyecto.FinanciamientoIV.TiempoFinanciamiento.Equals(0))
                        {
                            proyecto.FinanciamientoIV = financiamientoData.GetFinanciamiento(proyecto.CodProyecto, true);
                        }

                        if (!proyecto.FinanciamientoIV.CodFinanciamiento.Equals(0) || !proyecto.FinanciamientoIV.MontoFinanciamiento.Equals(0) || !proyecto.FinanciamientoIV.TiempoFinanciamiento.Equals(0))
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
        #endregion

        #region informacion general

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

        //Cada vez que se seleccione un canton distinto en el combobox va a actualizar el combobox de distritos
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

        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            ValidarInformacionGeneral(isCleaning: true);
            if (!ValidarInformacionGeneral())
            {
                InsertarProyecto(1);
                LlenaFooter();
            }
        }

        private void btnVerResumen2_Click(object sender, EventArgs e)
        {
            GenerarReporte reporte = new GenerarReporte(this.proyecto);
            reporte.CrearReporte();
        }

        /// <summary>
        /// Actualiza/inserta proyecto
        /// action = 1 refresca informacion general
        /// action = 2 refresca caracterizacion
        /// </summary>
        /// <param name="action"></param>
        private void InsertarProyecto(int action)
        {
            try
            {
                proyecto = proyecto == null ? new Proyecto() : proyecto;
                ProyectoData proyectoData = new ProyectoData();

                if (action == 1)
                {
                    proyecto.AnoInicial = Convert.ToInt32(nudAnoInicialProyecto.Value);
                    proyecto.Canton = ObtieneCantonParaInsertar();
                    proyecto.ConIngresos = chbConIngreso.Checked;
                    proyecto.DireccionExacta = txbDireccionExacta.Text;
                    proyecto.Distrito = ObtieneDistritoParaInsertar();
                    proyecto.Encargado = evaluador;
                    proyecto.HorizonteEvaluacionEnAnos = Convert.ToInt32(nudHorizonteEvaluacion.Value);
                    proyecto.NombreProyecto = txbNombreProyecto.Text;
                    proyecto.PagaImpuesto = chbSiPagaImpuesto.Checked;
                    proyecto.PorcentajeImpuesto = Convert.ToDouble(nudPorcentajeImpuesto.Value);
                    proyecto.Provincia = ObtieneProvinciaParaInsertar();
                    proyecto.ResumenEjecutivo = txbResumenEjecutivo.Text;
                }
                
                else if (action == 2)
                {
                    //caracterizacion
                    proyecto.CaraterizacionDelBienServicio = txbCaracterizacionBienServicio.Text;
                    proyecto.DescripcionPoblacionBeneficiaria = txbDescripcionPoblacionBeneficiaria.Text;
                    proyecto.JustificacionDeMercado = txbJustificacionMercado.Text;
                    proyecto.DescripcionSostenibilidadDelProyecto = txbDescripcionSostenibilidadProyecto.Text;
                }

                if (proyecto.CodProyecto == 0)
                {
                    proyecto.CodProyecto = proyectoData.InsertarProyecto(proyecto);
                    if (proyecto.CodProyecto != -1)
                    {
                        InsertarObjetoInteres(proyecto.CodProyecto);
                        MessageBox.Show("Proyecto insertado con éxito", "Insertado",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ocurrió un error con la insersión", "No insertado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (proyectoData.ActualizarProyecto(proyecto) == true)
                    {
                        InsertarObjetoInteres();

                        MessageBox.Show("Proyecto actualizado con éxito", "Actualizado",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ocurrió un error con la insersión", "No insertado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//InsertarProyecto

        private void InsertarObjetoInteres(int codProyecto = 0)
        {
            ObjetoInteresProyecto objetoInteres = codProyecto == 0 ? proyecto.ObjetoInteres : new ObjetoInteresProyecto();
            try
            {
                ObjetoInteresData objetoInteresData = new ObjetoInteresData();
                objetoInteres.DescripcionObjetoInteres = txbObjetoInteres.Text;
                objetoInteres.UnidadMedida = ObtieneUnidadMedidaParaInsertar();

                if(codProyecto == 0)
                {
                    objetoInteresData.ActualizarObjetoInteres(objetoInteres, proyecto.CodProyecto);
                }
                else
                {
                    objetoInteresData.InsertarObjetoDeInteres(objetoInteres, codProyecto);
                }
            }
            catch
            {
                throw;
            }
        }

        
        #endregion

        #region proponente
        private void LlenaComboTipoOrganizacion()
        {
            TipoOrganizacionData tipoOrganizacionData = new TipoOrganizacionData();
            cbxTipoOrganizacion.DataSource = tipoOrganizacionData.GetTiposDeOrganizacion();
            cbxTipoOrganizacion.DisplayMember = "descripcion";
            cbxTipoOrganizacion.ValueMember = "cod_tipo";
        }

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

        //Llena la informacion de la organizacion cuando se selecciona un proyecto para modificar
        private void LlenaInformacionOrganizacion()
        {
            txbNombreOrganizacion.Text = proyecto.Proponente.Organizacion.NombreOrganizacion;
            cbxTipoOrganizacion.SelectedValue = proyecto.Proponente.Organizacion.Tipo.CodTipo;
            txbCedulaJuridica.Text = proyecto.Proponente.Organizacion.CedulaJuridica;
            txbTelefonoOrganizacion.Text = proyecto.Proponente.Organizacion.Telefono;
            txbDescripcion.Text = proyecto.Proponente.Organizacion.Descripcion;
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
            chbMasculino.Checked = proyecto.Proponente.Genero == 'm' ? true : false;
            chbFemenino.Checked = proyecto.Proponente.Genero != 'm' ? true : false;
            EsRepresentanteIndividual.Checked = proyecto.Proponente.EsRepresentanteIndividual;
        }//LlenaInformacionProponente

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

        private void EsRepresentanteIndividual_CheckedChanged(object sender, EventArgs e)
        {
            proyecto.Proponente.EsRepresentanteIndividual = EsRepresentanteIndividual.Checked;
            CambiaValoresRepresentanteIndividual(proyecto.Proponente.EsRepresentanteIndividual);
        }

        private void CambiaValoresRepresentanteIndividual(bool esRepresentante)
        {
            if (esRepresentante)
            {
                cbxTipoOrganizacion.SelectedIndex = cbxTipoOrganizacion.Items.Count - 1;
                cbxTipoOrganizacion.Enabled = false;
            }
            else
            {
                cbxTipoOrganizacion.Enabled = true;
                cbxTipoOrganizacion.SelectedValue = this.proyecto.Proponente.Organizacion.Tipo.CodTipo;
                if (cbxTipoOrganizacion.SelectedValue == null)
                {
                    cbxTipoOrganizacion.SelectedIndex = 0;
                }
            }
        }

        private void txbNombreProponente_TextChanged(object sender, EventArgs e)
        {
            CambiaValoresRepresentanteIndividual(proyecto.Proponente.EsRepresentanteIndividual);
        }

        private void txbApellidos_TextChanged(object sender, EventArgs e)
        {
            CambiaValoresRepresentanteIndividual(proyecto.Proponente.EsRepresentanteIndividual);
        }

        private void txbCedulaProponente_TextChanged(object sender, EventArgs e)
        {
            CambiaValoresRepresentanteIndividual(proyecto.Proponente.EsRepresentanteIndividual);
        }

        private void txbTelefonoProponente_TextChanged(object sender, EventArgs e)
        {
            CambiaValoresRepresentanteIndividual(proyecto.Proponente.EsRepresentanteIndividual);
        }

        //Este boton guarda la información de la organizacion proponente y del proponente
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            ValidarProponente(isCleaning:true);
            if (!ValidarProponente())
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

                LlenaFooter();
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
            catch (Exception ex)
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
                proponente.EsRepresentanteIndividual = EsRepresentanteIndividual.Checked;
                proponenteData.InsertarProponente(proponente, this.proyecto.CodProyecto);
                this.proyecto.Proponente = proponente;
            }
            catch
            {
                throw;
            }

        }//InsertarProponente

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
            catch
            {
                MessageBox.Show("Organizacion y proponente actualizados con éxito", "Actualizados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }//ActualizarOrganizacionProponente

        private void ActualizarProponente()
        {
            try
            {
                ProponenteData proponenteData = new ProponenteData();
                proyecto.Proponente.Apellidos = txbApellidos.Text;
                proyecto.Proponente.Email = txbEmailProponente.Text;
                proyecto.Proponente.Genero = chbMasculino.Checked ? 'm' : 'f';
                proyecto.Proponente.Nombre = txbNombreProponente.Text;
                proyecto.Proponente.NumIdentificacion = txbCedulaProponente.Text;
                proyecto.Proponente.PuestoEnOrganizacion = txbPuestoEnOrganizacion.Text;
                proyecto.Proponente.Telefono = txbTelefonoProponente.Text;
                proyecto.Proponente.EsRepresentanteIndividual = EsRepresentanteIndividual.Checked;
                proponenteData.ActualizarProponente(proyecto.Proponente);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region caracterizacion
        private void btnGuardar3_Click(object sender, EventArgs e)
        {
            InsertarProyecto(2);
        }
        #endregion

        #region inversiones
        //Evento que se ejecuta cuando el valor de una celda del dgvInversiones cambia
        private void dgvInversiones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (proyecto != null && proyecto.RequerimientosInversion != null)
            {
                List<int> intList = new List<int>(new int[] { 3, 4, 7, 8 });
                string valueToValidate = string.Empty;
                bool isInList = false;
                if (proyecto.RequerimientosInversion.Count != 0 && e.RowIndex > 0)
                {
                    valueToValidate = dgvInversiones[e.ColumnIndex, e.RowIndex].Value.ToString();
                    isInList = intList.IndexOf(e.ColumnIndex) != -1;
                    bool isValid = isInList ? ValidaNumeros(valueToValidate) : true;

                    if (isValid)
                    {
                        try
                        {
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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    else
                    {
                        dgvInversiones[e.ColumnIndex, e.RowIndex].Value = 0;
                        MessageBox.Show("Los datos ingresados son inválidos en ese campo",
                                    "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
           
        }

        private void btnGuardarInversion_Click(object sender, EventArgs e)
        {
            //la siguiente variable valida si la insersión se realizó o no dependiendo de esa variable muestra
            //alguno de los mensajes de error
            int validaInsersion = 0;

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

                    if (!dgvInversiones.Columns.Contains("codRequerimiento") || dgvInversiones.Rows[i].Cells["codRequerimiento"].Value.ToString().Equals(string.Empty))
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
                        if (this.dgvInversiones.Rows[i].Cells["UnidadMedida"].Value.ToString() == fila["nombre_unidad"].ToString())
                        {
                            unidadMedida.CodUnidad = Int32.Parse(fila["cod_unidad"].ToString());
                            unidadMedida.NombreUnidad = fila["nombre_unidad"].ToString();
                            break;
                        }//if

                    }//foreach
                    requerimientoInversiones.UnidadMedida = unidadMedida;

                    if (this.dgvInversiones.Rows[i].Cells["Depreciable"].Value == null || this.dgvInversiones.Rows[i].Cells["Depreciable"].Value.ToString().Equals(""))
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

                        if ((this.dgvInversiones.Rows[i].Cells["VidaUtil"].Value == null || this.dgvInversiones.Rows[i].Cells["VidaUtil"].Value.ToString().Equals(string.Empty)))
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
                            validaInsersion = 1;
                        }
                        else
                        {
                            requerimientosInversionData.EditarRequerimientosInvesion(requerimientoInversiones);
                            int index = proyecto.RequerimientosInversion.FindIndex(ri => ri.CodRequerimientoInversion.Equals(requerimientoInversiones.CodRequerimientoInversion));
                            proyecto.RequerimientosInversion[index] = requerimientoInversiones;
                            validaInsersion = 2;
                        }
                    }
                }//try
                catch (Exception ex)
                {
                    validaInsersion = 0;
                    Console.WriteLine(ex);
                }//catch
            }//for
            if (!validaInsersion.Equals(0))
            {
                if (validaInsersion.Equals(1))
                {
                    foreach (RequerimientoInversion inversion in listaRequerimientoInversion)
                    {
                        proyecto.RequerimientosInversion.Add(inversion);
                    }
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

        private void dgvInversiones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            DataGridViewComboBoxColumn comboboxColumn = dgvInversiones.Columns["UnidadMedida"] as DataGridViewComboBoxColumn;
            comboboxColumn.DataSource = unidadMedidaData.GetUnidadesMedida();
            comboboxColumn.DisplayMember = "nombre_unidad";
            comboboxColumn.ValueMember = "cod_unidad";

            dgvInversiones.Rows[e.RowIndex].Cells["UnidadMedida"].Value = (comboboxColumn.Items[0] as DataRowView).Row[1].ToString();


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

        private void btnVerResumen4_Click(object sender, EventArgs e)
        {

        }

        private void LlenaDgvInversiones()
        {
            if ((proyecto.RequerimientosInversion != null) && (this.proyecto.RequerimientosInversion.Count > 0))
            {
                DataTable dtRequerimientos;
                double totalInver = 0;
                DatatableBuilder.GenerarDTInversiones(this.proyecto, out dtRequerimientos, out totalInver);
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
            else
            {
                UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
                DataGridViewComboBoxColumn comboboxColumn = dgvInversiones.Columns["UnidadMedida"] as DataGridViewComboBoxColumn;
                comboboxColumn.DataSource = unidadMedidaData.GetUnidadesMedida();
                comboboxColumn.DisplayMember = "nombre_unidad";
                comboboxColumn.ValueMember = "cod_unidad";
                dgvInversiones.Rows[0].Cells["UnidadMedida"].Value = (comboboxColumn.Items[0] as DataRowView).Row[1].ToString();

            }
        }//LlenaDgvInversiones
        #endregion

        #region reinversiones
        private void btnAgregarReinversion_Click(object sender, EventArgs e)
        {
            mostrarMensajeSeguridad = false;
            AgregarReinversionDialog agregarReinversion = new AgregarReinversionDialog(evaluador, proyecto);
            agregarReinversion.MdiParent = this.MdiParent;
            agregarReinversion.Show();
            this.Close();
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

            List<String> anosReinversion = new List<String>();
            for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
            {
                int anoActual = proyecto.AnoInicial + i;
                anosReinversion.Add(anoActual.ToString());
            }//for

            DataGridViewComboBoxColumn anoInversionColumn = dgvReinversiones.Columns["AnoReinversion"]
                    as DataGridViewComboBoxColumn;
            anoInversionColumn.DataSource = anosReinversion;

            UnidadMedidaData unidadMedidaData = new UnidadMedidaData();
            DataGridViewComboBoxColumn comboboxColumn = dgvReinversiones.Columns["unidadMedidaRe"] as DataGridViewComboBoxColumn;
            DataTable dtUnidades = unidadMedidaData.GetUnidadesMedida();
            comboboxColumn.DataSource = dtUnidades;
            comboboxColumn.DisplayMember = "nombre_unidad";
            comboboxColumn.ValueMember = "cod_unidad";

            this.dgvReinversiones.Rows[dgvReinversiones.RowCount - 1].Cells["AnoReinversion"].Value = proyecto.AnoInicial + 1;
            this.dgvReinversiones.Rows[dgvReinversiones.RowCount - 1].Cells["unidadMedidaRe"].Value = dtUnidades.Rows[0][0];
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
            List<int> intList = new List<int>(new int[] { 4, 5, 7 });
            string valueToValidate = string.Empty;
            bool isInList = false;
            if (proyecto != null && proyecto.RequerimientosReinversion != null && proyecto.RequerimientosReinversion.Count != 0 && e.RowIndex > 0)
            {
                valueToValidate = dgvReinversiones[e.ColumnIndex, e.RowIndex].Value.ToString();
                isInList = intList.IndexOf(e.ColumnIndex) != -1;
                bool isValid = isInList ? ValidaNumeros(valueToValidate) : true;

                if (isValid)
                {
                    LlenaDgvTotalesReinversiones();

                    try
                    {
                        //Valida si se cambio el valor de la cantidad o del costo unitario de una
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
                else
                {
                    dgvReinversiones[e.ColumnIndex, e.RowIndex].Value = 0;
                    MessageBox.Show("Los datos ingresados son inválidos en ese campo",
                                "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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

                        if (!dgvReinversiones.Columns.Contains("Codigo") || dgvReinversiones.Rows[i].Cells["Codigo"].Value.ToString().Equals(string.Empty))
                        {
                            requerimientoReinversion.CodRequerimientoReinversion = 0;
                        }
                        else
                        {
                            requerimientoReinversion.CodRequerimientoReinversion =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["Codigo"].Value.ToString());
                        }

                        requerimientoReinversion.VidaUtil = this.dgvReinversiones.Rows[i].Cells["vidaUtilRe"].Value == null ? 0 :
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["vidaUtilRe"].Value.ToString());
                        requerimientoReinversion.AnoReinversion =
                            Int32.Parse(this.dgvReinversiones.Rows[i].Cells["AnoReinversion"].Value.ToString());

                        if (this.dgvReinversiones.Rows[i].Cells["DepreciableReinversion"].Value == null || this.dgvReinversiones.Rows[i].Cells["DepreciableReinversion"].Value.ToString().Equals(""))
                        {
                            requerimientoReinversion.Depreciable = false;
                        }
                        else
                        {
                            requerimientoReinversion.Depreciable =
                                Convert.ToBoolean(this.dgvReinversiones.Rows[i].Cells["DepreciableReinversion"].Value);
                        }

                        // Se realiza una instancia del dgv combobox column para poder manipularlo
                        DataGridViewComboBoxColumn unidadMedidaColumn =
                                dgvReinversiones.Columns["unidadMedidaRe"] as DataGridViewComboBoxColumn;
                        DataTable dtUnidadMedida = (DataTable)unidadMedidaColumn.DataSource;
                        UnidadMedida unidadMedida = new UnidadMedida();
                        foreach (DataRow fila in dtUnidadMedida.Rows)
                        {
                            if (this.dgvReinversiones.Rows[i].Cells["unidadMedidaRe"].Value.ToString() == fila["cod_unidad"].ToString())
                            {
                                unidadMedida.CodUnidad = Int32.Parse(fila["cod_unidad"].ToString());
                                unidadMedida.NombreUnidad = fila["nombre_unidad"].ToString();
                                break;
                            }//if
                        }//foreach
                        requerimientoReinversion.UnidadMedida = unidadMedida;

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

        //TODDO THIS
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
            this.dgvTotalesReinversiones.DataSource = DatatableBuilder.GenerarDTTotalesReinversiones(this.proyecto);
            this.dgvTotalesReinversiones.ReadOnly = true;
        }//LlenaDgvTotalesReinversiones
        #endregion

        #region proyeccion ventas
        private void btnAgregarProyecciones_Click(object sender, EventArgs e)
        {
            mostrarMensajeSeguridad = false;
            AgregarProyeccionDialog agregarProyeccion = new AgregarProyeccionDialog(evaluador, proyecto);
            agregarProyeccion.MdiParent = this.MdiParent;
            agregarProyeccion.Show();
            this.Close();
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

        private void btnEditarProyeccion_Click(object sender, EventArgs e)
        {
            int codProyeccionArticulo = Convert.ToInt32(dgvProyeccionesVentas.SelectedRows[0].Cells[0].Value.ToString());
            mostrarMensajeSeguridad = false;
            AgregarProyeccionDialog agregarProyeccion = new AgregarProyeccionDialog(evaluador, proyecto, codProyeccionArticulo);
            agregarProyeccion.MdiParent = this.MdiParent;
            agregarProyeccion.Show();
            this.Close();
        }

        private void LlenaDgvProyeccionesVentas()
        {
            if ((proyecto.Proyecciones != null) && (this.proyecto.Proyecciones.Count > 0))
            {
                this.dgvProyeccionesVentas.DataSource = DatatableBuilder.GenerarDTProyeccionesVentas(this.proyecto);
                this.dgvProyeccionesVentas.Columns["Codigo"].Visible = false;
            }//if
        }//LlenaDgvProyeccionesVentas

        private void llbRegistrarCrecimientosOferta_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarMensajeSeguridad = false;
            GestionCrecimientosOfertaDialog gestionCrecimientoOferta = new GestionCrecimientosOfertaDialog(evaluador, proyecto);
            gestionCrecimientoOferta.MdiParent = this.MdiParent;
            gestionCrecimientoOferta.Show();
            this.Close();
        }

        private void LlenaDgvIngresosGenerados()
        {
            dgvIngresosGenerados.DataSource = DatatableBuilder.GenerarDTIngresosGenerados(this.proyecto);
            dgvIngresosGenerados.Columns[0].HeaderText = "";
        }//LlenaDgvIngresosGenerados
        #endregion

        #region costos
        private void btnAgregarCosto_Click(object sender, EventArgs e)
        {
            mostrarMensajeSeguridad = false;
            AgregarCostoDialog agregarCosto = new AgregarCostoDialog(evaluador, proyecto);
            agregarCosto.MdiParent = this.MdiParent;
            agregarCosto.Show();
            this.Close();
        }

        private void btnEliminarCosto_Click(object sender, EventArgs e)
        {
            int codCosto = Convert.ToInt32(dgvCostos.SelectedRows[0].Cells[0].Value.ToString());
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

        private void btnEditarCosto_Click(object sender, EventArgs e)
        {
            int codCosto = Convert.ToInt32(dgvCostos.SelectedRows[0].Cells[0].Value.ToString());
            mostrarMensajeSeguridad = false;
            AgregarCostoDialog agregarCosto = new AgregarCostoDialog(evaluador, proyecto, codCosto);
            agregarCosto.MdiParent = this.MdiParent;
            agregarCosto.Show();
            this.Close();
        }

        private void btnVerResumenCostos_Click(object sender, EventArgs e)
        {

        }

        private void llbGestionVariacionCostos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarMensajeSeguridad = false;
            GestionVariacionCostosDialog gestionVariacionCostos = new GestionVariacionCostosDialog(evaluador, proyecto);
            gestionVariacionCostos.MdiParent = this.MdiParent;
            gestionVariacionCostos.Show();
            this.Close();
        }

        private void LlenaDgvCostos()
        {
            if ((this.proyecto.Costos != null) && (this.proyecto.Costos.Count > 0))
            {
                this.dgvCostos.DataSource = DatatableBuilder.GenerarDTCostos(this.proyecto);
                this.dgvCostos.Columns["Codigo"].Visible = false;
                this.dgvCostos.Columns[4].HeaderText = "Año Inicial";
            }//if
        }//LlenaDgvCostos

        private void LlenaDgvCostosGenerados()
        {
            dgvCostosGenerados.DataSource = DatatableBuilder.GeneraDTCostosGenerados(this.proyecto);
            dgvCostosGenerados.Columns[0].HeaderText = "";
        }//LlenaDgvCostosGenerados
        #endregion

        #region capital trabajo
        private void LlenaDgvCapitalTrabajo()
        {
            DataTable dtCapitalTrabajo;
            Double recCTResult;
            DatatableBuilder.GenerarDTCapitalTrabajo(this.proyecto, out dtCapitalTrabajo, out recCTResult);
            dgvCapitalTrabajo.DataSource = dtCapitalTrabajo;

            foreach (DataGridViewColumn column in dgvCapitalTrabajo.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            lblRecuperacionCT.Text = "₡ " + recCTResult.ToString("#,##0.##");
        }
        #endregion

        #region depreciaciones
        private void btnGuardarDep_Click(object sender, EventArgs e)
        {

        }

        private void LlenaDgvDepreciaciones()
        {
            this.dgvDepreciaciones.DataSource = DatatableBuilder.GenerarDTDepreciaciones(this.proyecto);
        }
        #endregion

        #region financiamiento
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
            InsertarFinanciamiento();
        }

        private void InsertarFinanciamiento()
        {
            Financiamiento financiamientoTemp = proyecto.FinanciamientoIF.CodFinanciamiento.Equals(0) ? new Financiamiento() : proyecto.FinanciamientoIF;
            financiamientoTemp.MontoFinanciamiento = Convert.ToDouble(tbMontoFinanciamientoIF.Text);
            financiamientoTemp.TiempoFinanciamiento = Convert.ToInt32(nupTiempoFinanciamientoIF.Value);
            
            InteresFinanciamiento intFinanTemp = new InteresFinanciamiento();
            intFinanTemp.PorcentajeInteresFinanciamiento = Convert.ToDouble(nupPorcentajeInteresIF.Value);
            
            FinanciamientoData finanData = new FinanciamientoData();
            if (financiamientoTemp.CodFinanciamiento.Equals(0))
            {
                financiamientoTemp.VariableFinanciamiento = false;
                if (finanData.InsertarFinanciamiento(financiamientoTemp, proyecto.CodProyecto))
                {
                    proyecto.FinanciamientoIF = financiamientoTemp;
                    InteresFinanciamientoData intFinanData = new InteresFinanciamientoData();
                    intFinanTemp.VariableInteres = false;

                    if (intFinanData.InsertarInteresFinanciamiento(intFinanTemp, proyecto.CodProyecto))
                    {
                        proyecto.InteresFinanciamientoIF = intFinanTemp;
                        LlenaDgvFinanciamientoIF();
                        MessageBox.Show("Financiamiento fijo registrado con éxito",
                               "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("El Financiamiento no se ha podido registrar",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("El Financiamiento no se han podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (finanData.ActualizarFinanciamiento(financiamientoTemp))
                {
                    proyecto.FinanciamientoIF = financiamientoTemp;
                    InteresFinanciamientoData intFinanData = new InteresFinanciamientoData();
                    if (intFinanData.ActualizarInteresFinanciamiento(intFinanTemp))
                    {
                        this.proyecto.InteresFinanciamientoIF = intFinanTemp;
                        LlenaDgvFinanciamientoIF();
                        MessageBox.Show("Financiamiento fijo actualizado con éxito",
                               "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("El Financiamiento no se ha podido actualizar",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("El Financiamiento no se han podido registrar",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (proyecto.FinanciamientoIV.CodFinanciamiento.Equals(0))
                {
                    if (finanData.InsertarFinanciamiento(financiamientoVariableTemp, this.proyecto.CodProyecto))
                    {
                        proyecto.FinanciamientoIV = financiamientoVariableTemp;
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
                    financiamientoVariableTemp.CodFinanciamiento = proyecto.FinanciamientoIV.CodFinanciamiento;
                    if (finanData.ActualizarFinanciamiento(financiamientoVariableTemp))
                    {
                        proyecto.FinanciamientoIV.MontoFinanciamiento = financiamientoVariableTemp.MontoFinanciamiento;
                        proyecto.FinanciamientoIV.TiempoFinanciamiento = financiamientoVariableTemp.TiempoFinanciamiento;
                        LlenaDgvFinanciamientoIV();
                        MessageBox.Show("Financiamiento actualizado con éxito",
                                   "Insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("El Financiamiento no se han podido actualizar",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            if (Convert.ToInt32(nudTiempoVariable.Value) > 0)
            {
                financiamientoV.TiempoFinanciamiento = Convert.ToInt32(nudTiempoVariable.Value);
            }
            financiamientoV.TiempoFinanciamiento = Convert.ToInt32(nudTiempoVariable.Value);
            financiamientoV.VariableFinanciamiento = true;
            proyecto.FinanciamientoIV = financiamientoV;
            InteresFinanciamientoIVDialog interesesFinanciamiento = new InteresFinanciamientoIVDialog(evaluador, proyecto);
            interesesFinanciamiento.MdiParent = MdiParent;
            interesesFinanciamiento.Show();
            Close();
        }

        //TODDO THIS
        private void LlenaDgvFinanciamientoIF()
        {
            nupTiempoFinanciamientoIF.Maximum = this.proyecto.HorizonteEvaluacionEnAnos;
            if (proyecto.FinanciamientoIF != null)
            {
                Double myInt;
                bool isNumerical = Double.TryParse(tbMontoFinanciamientoIF.Text.ToString(), out myInt);
                if (isNumerical && myInt > 0 && Convert.ToDouble(nupTiempoFinanciamientoIF.Value) > 0 && Convert.ToDouble(nupPorcentajeInteresIF.Value) > 0)
                {
                    double monto = Convert.ToDouble(tbMontoFinanciamientoIF.Text);
                    double tiempo = Convert.ToDouble(nupTiempoFinanciamientoIF.Value);
                    double interesIF = Convert.ToDouble(nupPorcentajeInteresIF.Value);

                    dgvFinanciamientoIF.DataSource = DatatableBuilder.GenerarDTFinanciamientoIF(this.proyecto, monto, tiempo, interesIF);
                    dgvFinanciamientoIF.Columns[0].HeaderText = "Año de Pago";
                    dgvFinanciamientoIF.Columns[1].HeaderText = "Saldo";
                    dgvFinanciamientoIF.Columns[2].HeaderText = "Cuota";
                    dgvFinanciamientoIF.Columns[3].HeaderText = "Interés";
                    dgvFinanciamientoIF.Columns[4].HeaderText = "Amortización";
                    dgvFinanciamientoIF.Columns[0].Width = 130;
                    dgvFinanciamientoIF.Columns[1].Width = 160;
                    dgvFinanciamientoIF.Columns[2].Width = 150;
                    dgvFinanciamientoIF.Columns[3].Width = 150;
                    dgvFinanciamientoIF.Columns[4].Width = 160;

                    foreach (DataGridViewColumn column in dgvFinanciamientoIF.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
        }

        //TODDO THIS
        private void LlenaDgvFinanciamientoIV()
        {
            Double myInt;
            bool isNumerical = Double.TryParse(tbMontoVariable.Text.ToString(), out myInt);
            if (isNumerical && myInt > 0 && Convert.ToDouble(nudTiempoVariable.Value) > 0 && proyecto.InteresesFinanciamientoIV.Count > 0)
            {
                if (proyecto.FinanciamientoIV != null)
                {
                    dgvFinanciamientoVariable.DataSource = DatatableBuilder.GenerarDTFinanciamientoIV(this.proyecto);
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

                    foreach (DataGridViewColumn column in dgvFinanciamientoVariable.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
        }
        #endregion

        #region flujo caja
        /// <summary>
        /// método que calcula TIR y VAN
        /// se requiere que el primer valor sea negativo y los demás positivos
        /// para el calculo efectivo del TIR
        /// </summary>
        private void LlenaCalculosFinales(bool intFijo = false)
        {
            try
            {
                double[] flujoEfectivo = new double[this.proyecto.HorizonteEvaluacionEnAnos + 1];
                double[] flujoEfectivoSinInicio = new double[this.proyecto.HorizonteEvaluacionEnAnos];

                if (intFijo)
                {
                    for (int i = 0; i <= this.proyecto.HorizonteEvaluacionEnAnos; i++)
                    {
                        flujoEfectivo[i] = Convert.ToDouble(dgvFlujoCajaIntFijo.Rows[16].Cells[i + 1].Value.ToString().Replace("₡", string.Empty));
                    }
                }
                else
                {
                    for (int i = 0; i <= this.proyecto.HorizonteEvaluacionEnAnos; i++)
                    {
                        flujoEfectivo[i] = Convert.ToDouble(dgvFlujoCajaIntVariable.Rows[16].Cells[i + 1].Value.ToString().Replace("₡", string.Empty));
                    }
                }

                for (int i = 0; i < this.proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    flujoEfectivoSinInicio[i] = flujoEfectivo[i + 1];
                }
                double IRR = Microsoft.VisualBasic.Financial.IRR(ref flujoEfectivo, 0.3) * 100;
                double VNA = flujoEfectivo[0] + Microsoft.VisualBasic.Financial.NPV(Convert.ToDouble(nudTasaCostoCapital.Value), ref flujoEfectivoSinInicio);

                if (intFijo)
                {
                    tbxTIR.Text = (IRR).ToString("#,##0.##") + " %";
                    tbxVAN.Text = "₡ " + (VNA).ToString("#,##0.##");
                }
                else
                {
                    tbxTIRIV.Text = (IRR).ToString("#,##0.##") + " %";
                    tbxVANIV.Text = "₡ " + (VNA).ToString("#,##0.##");
                }
            }
            catch
            {
                if (intFijo)
                {
                    tbxTIR.Text = "0 %";
                    tbxVAN.Text = "₡ 0";
                }
                else
                {
                    tbxTIRIV.Text = "0 %";
                    tbxVANIV.Text = "₡ 0";
                }

            }

            nudTasaCostoCapital.Value = Convert.ToDecimal(this.proyecto.TasaCostoCapital);
        }

        private void btnGuardarFlujoCaja_Click(object sender, EventArgs e)
        {
            this.proyecto.TasaCostoCapital = Convert.ToDouble(nudTasaCostoCapital.Value);
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

        private void dgvInversiones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        

        #region interes fijo
        private void LlenaDgvFlujoCajaIF()
        {
            if (proyecto.RequerimientosInversion.Count != 0 && proyecto.Costos.Count != 0 && proyecto.Proyecciones.Count != 0 && proyecto.FinanciamientoIF != null)
            {
                string totalInversiones = lblTotalInversiones.Text.ToString().Replace("₡", string.Empty);
                string recuperacionCT = lblRecuperacionCT.Text.ToString();
                dgvFlujoCajaIntFijo.DataSource = DatatableBuilder.GenerarDTFlujoCaja(true, this.proyecto, dgvCapitalTrabajo, dgvFinanciamientoIF, dgvTotalesReinversiones, totalInversiones, recuperacionCT);
                dgvFlujoCajaIntFijo.Columns[0].Width = 180;

                foreach (DataGridViewColumn column in dgvFlujoCajaIntFijo.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                LlenaCalculosFinales(true);
            }
        }

        private void llblDetalleIndicadores_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarMensajeSeguridad = false;
            double TIR = Convert.ToDouble(tbxTIR.Text.Replace(" %", string.Empty));
            double VAN = Convert.ToDouble(tbxVAN.Text.Replace("₡ ", string.Empty));
            IndicadoresDialog verIndicadores = new IndicadoresDialog(this.evaluador, this.proyecto, TIR, VAN);
            verIndicadores.MdiParent = this.MdiParent;
            verIndicadores.Show();
            Close();
        }
        #endregion

        #region interes variable
        private void LlenaDgvFlujoCajaIV()
        {
            string totalInversiones = lblTotalInversiones.Text.ToString().Replace("₡", string.Empty);
            string recuperacionCT = lblRecuperacionCT.Text.ToString();
            dgvFlujoCajaIntVariable.DataSource = DatatableBuilder.GenerarDTFlujoCaja(false, this.proyecto, dgvCapitalTrabajo, dgvFinanciamientoVariable, dgvTotalesReinversiones, totalInversiones, recuperacionCT);
            dgvFlujoCajaIntVariable.Columns[0].Width = 180;

            foreach (DataGridViewColumn column in dgvFlujoCajaIntVariable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            LlenaCalculosFinales();
        }

        private void lblDetalleIndicadoresIV_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mostrarMensajeSeguridad = false;
            double TIR = Convert.ToDouble(tbxTIRIV.Text.Replace(" %", string.Empty));
            double VAN = Convert.ToDouble(tbxVANIV.Text.Replace("₡ ", string.Empty));
            IndicadoresDialog verIndicadores = new IndicadoresDialog(this.evaluador, this.proyecto, TIR, VAN);
            verIndicadores.MdiParent = this.MdiParent;
            verIndicadores.Show();
            Close();
        }
        #endregion

        #endregion

        private void dgvReinversiones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }//Clase registrar proyecto
}//namespace