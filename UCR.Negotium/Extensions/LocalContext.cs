using System.Collections.Generic;
using System.Data;
using System.Windows;
using UCR.Negotium.Base.Enumerados;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Extensions
{
    public static class LocalContext
    {

        #region Flujo Caja
        private static DataView FlujoCaja = null;

        public static DataView GetFlujoCaja(Proyecto proyecto, DataView dgvCapitalTrabajo, DataView dgvFinanciamiento, DataView dgvTotalesReinversiones, string totalInversiones, string recuperacionCT)
        {
            if (FlujoCaja == null)
            {
                if(proyecto.TipoProyecto.CodTipo == 1)
                {
                    SetFlujoCaja(DatatableBuilder.GenerarFlujoCaja(proyecto, dgvCapitalTrabajo, 
                        dgvFinanciamiento, dgvTotalesReinversiones, totalInversiones, 
                        recuperacionCT).AsDataView());
                }
                else
                {
                    SetFlujoCaja(DatatableBuilder.GenerarFlujoCajaSocial(proyecto, 
                        dgvCapitalTrabajo, dgvFinanciamiento, dgvTotalesReinversiones, 
                        totalInversiones, recuperacionCT).AsDataView());
                }
            }
            return FlujoCaja;
        }

        public static void SetFlujoCaja(DataView flujoCaja)
        {
            FlujoCaja = flujoCaja;
        }
        #endregion

        #region Moneda
        private static ProyectoData proyectoData = new ProyectoData();

        public static string GetSignoMoneda(int codProyecto)
        {
            CacheSignoMoneda signoMoneda = new CacheSignoMoneda();
            signoMoneda = CacheSignoMonedas.Find(signo => signo.CodProyecto.Equals(codProyecto));

            if (signoMoneda != null)
            {
                if (signoMoneda.Refresh)
                {
                    CacheSignoMonedas.Remove(signoMoneda);
                    CacheSignoMonedas.Add(new CacheSignoMoneda()
                    {
                        CodProyecto = codProyecto,
                        SignoMoneda = proyectoData.GetSignoMonedaProyecto(codProyecto)
                    });
                }
            }
            else
            {
                CacheSignoMonedas.Add(new CacheSignoMoneda()
                {
                    CodProyecto = codProyecto,
                    SignoMoneda = proyectoData.GetSignoMonedaProyecto(codProyecto)??string.Empty
                });
            }
            return CacheSignoMonedas.Find(signo => signo.CodProyecto.Equals(codProyecto)).SignoMoneda;
        }

        public static void RefreshSignoMoneda(int codProyecto)
        {
            CacheSignoMoneda signoMoneda = CacheSignoMonedas.Find(signo => signo.CodProyecto.Equals(codProyecto));
            if(signoMoneda != null)
                signoMoneda.Refresh = true;
        }

        public static bool SetMoneda(int codProyecto, int codMoneda)
        {
            if(proyectoData.EditarMoneda(codProyecto, codMoneda))
            {
                RefreshSignoMoneda(codProyecto);
                return true;
            }
            
            return false;
        }

        private static List<CacheSignoMoneda> CacheSignoMonedas = new List<CacheSignoMoneda>();
        #endregion

        #region Reload Modulos
        public static void ReloadUserControls(int codProyecto, Modulo srcModule)
        {
            RegistrarProyectoWindow mainWindow = null;
            foreach(var window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(RegistrarProyectoWindow))
                {
                    mainWindow = (RegistrarProyectoWindow)window;
                    break;
                }  
            }

            if(mainWindow != null)
            {
                switch (srcModule)
                {
                    case Modulo.Caracterizacion:
                        break;
                    case Modulo.Proponente:
                        mainWindow.resumen.CodProyecto = codProyecto;
                        break;
                    case Modulo.Inversiones:
                        mainWindow.inversiones.CodProyecto =
                            mainWindow.reinversiones.CodProyecto =
                            mainWindow.depreciaciones.CodProyecto = codProyecto;
                        break;
                    case Modulo.Reinversiones:
                        mainWindow.reinversiones.CodProyecto =
                            mainWindow.depreciaciones.CodProyecto = codProyecto;
                        break;
                    case Modulo.ProyeccionVentas:
                        mainWindow.proyeccionVentas.CodProyecto = codProyecto;
                        break;
                    case Modulo.Costos:
                        mainWindow.costos.CodProyecto =
                            mainWindow.capitalTrabajo.CodProyecto = codProyecto;
                        break;
                    case Modulo.Financiamiento:
                        mainWindow.financiamientoUc.CodProyecto = codProyecto;
                        break;
                    default:
                        mainWindow.resumen.CodProyecto = mainWindow.orgProponente.CodProyecto =
                            mainWindow.infoGeneral.CodProyecto = mainWindow.caracterizacion.CodProyecto =
                            mainWindow.inversiones.CodProyecto = mainWindow.reinversiones.CodProyecto =
                            mainWindow.capitalTrabajo.CodProyecto = mainWindow.depreciaciones.CodProyecto =
                            mainWindow.costos.CodProyecto = mainWindow.proyeccionVentas.CodProyecto =
                            mainWindow.financiamientoUc.CodProyecto = mainWindow.analisisAmbiental.CodProyecto = codProyecto;

                        break;
                }

                mainWindow.ReloadBase(codProyecto);
            }
        }
        #endregion

    }

    public class CacheSignoMoneda
    {
        public int CodProyecto { get; set; }
        public string SignoMoneda { get; set; }
        public bool Refresh { get; set; }
    }
}
