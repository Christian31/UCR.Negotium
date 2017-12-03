using System.Collections.Generic;
using System.Data;
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
                SetFlujoCaja(DatatableBuilder.GenerarFlujoCaja(proyecto, dgvCapitalTrabajo, dgvFinanciamiento, dgvTotalesReinversiones,
                    totalInversiones, recuperacionCT).AsDataView());
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
                    SignoMoneda = proyectoData.GetSignoMonedaProyecto(codProyecto)
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

    }

    public class CacheSignoMoneda
    {
        public int CodProyecto { get; set; }
        public string SignoMoneda { get; set; }
        public bool Refresh { get; set; }
    }
}
