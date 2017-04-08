using MahApps.Metro.Controls;
using UCR.Negotium.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium
{
    /// <summary>
    /// Interaction logic for RegistrarProyectoWindow.xaml
    /// </summary>
    public partial class RegistrarProyectoWindow : MetroWindow
    {
        private Proyecto proyecto;
        private ProyectoData proyectoData;
        private ObjetoInteresData objetoInteresData;
        private ProponenteData proponenteData;
        private RequerimientoInversionData inversionData;
        private RequerimientoReinversionData reinversionData;
        private ProyeccionVentaArticuloData proyeccionData;
        private CostoData costoData;
        private CrecimientoOfertaObjetoInteresData crecimientoOfertaData;

        public RegistrarProyectoWindow(int codProyecto = 0)
        {
            InitializeComponent();
            DataContext = this;
            proyectoData = new ProyectoData();
            objetoInteresData = new ObjetoInteresData();
            proponenteData = new ProponenteData();
            inversionData = new RequerimientoInversionData();
            reinversionData = new RequerimientoReinversionData();
            proyeccionData = new ProyeccionVentaArticuloData();
            crecimientoOfertaData = new CrecimientoOfertaObjetoInteresData();
            costoData = new CostoData();

            proyecto = new Proyecto();
            if (!codProyecto.Equals(0))
            {
                proyecto = proyectoData.GetProyecto(codProyecto);
                proyecto.ObjetoInteres = objetoInteresData.GetObjetoInteres(codProyecto);
                proyecto.Proponente = proponenteData.GetProponente(codProyecto);
                proyecto.RequerimientosInversion = inversionData.GetRequerimientosInversion(codProyecto);
                proyecto.RequerimientosReinversion = reinversionData.GetRequerimientosReinversion(codProyecto);
                proyecto.Proyecciones = proyeccionData.GetProyeccionesVentaArticulo(codProyecto);
                proyecto.Costos = costoData.GetCostos(codProyecto);
                proyecto.CrecimientosAnuales = GetCrecimientosAnuales(codProyecto);
            }
        }

        private List<CrecimientoOfertaObjetoInteres> GetCrecimientosAnuales(int codProyecto)
        {
            DataTable dt = crecimientoOfertaData.GetCrecimientoOfertaObjetoIntereses(codProyecto);
            List<CrecimientoOfertaObjetoInteres> list = new List<CrecimientoOfertaObjetoInteres>();

            foreach (DataRow row in dt.Rows)
            {
                CrecimientoOfertaObjetoInteres creTemp = new CrecimientoOfertaObjetoInteres();
                creTemp.CodCrecimiento = Convert.ToInt32(row[0]);
                creTemp.AnoCrecimiento = Convert.ToInt32(row[1]);
                creTemp.PorcentajeCrecimiento = Convert.ToDouble(row[2]);

                list.Add(creTemp);
            }

            return list;
        }

        public Proyecto ProyectoSelected
        {
            get
            {
                return proyecto;
            }
            set
            {
                proyecto = value;
            }
        }

        public string InversionesTotal
        {
            get
            {
                double valor = 0;
                ProyectoSelected.RequerimientosInversion.ForEach(reqInver => valor += reqInver.Subtotal);
                return "₡ " + valor.ToString("#,##0.##");
            }
            set
            {
                InversionesTotal = value;
            }
        }

        public DataView DTCostos
        {
            get
            {
                return DatatableBuilder.GenerarDTCostos(ProyectoSelected).AsDataView();
            }
        }

        public DataView DTCostosGenerados
        {
            get
            {
                return DatatableBuilder.GeneraDTCostosGenerados(ProyectoSelected).AsDataView();
            }
        }

        public DataView DTTotalesReinversiones
        {
            get
            {
                return DatatableBuilder.GenerarDTTotalesReinversiones(ProyectoSelected).AsDataView();
            }
        }

        public DataView DTProyeccionesVentas
        {
            get
            {
                return DatatableBuilder.GenerarDTIngresosGenerados(ProyectoSelected).AsDataView();
            }
        }

        public DataView DTDepreciaciones
        {
            get
            {
                return DatatableBuilder.GenerarDTDepreciaciones(ProyectoSelected).AsDataView();
            }
        }

        //public DataTable DTCapitalTrabajo
        //{
        //    get
        //    {
        //        return DatatableBuilder.GenerarDTCapitalTrabajo(ProyectoSelected);
        //    }
        //    set
        //    {
        //        this.DTCostos = value;
        //    }
        //}
    }
}
