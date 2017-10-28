using NVelocity;
using NVelocity.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Extensions
{
    public class GenerarReporte
    {
        private Proyecto proyecto;
        Dictionary<string, string> reinvTotales;
        Dictionary<string, string> proyeccionesTotales;
        Dictionary<string, Tuple<string, string>> costosTotales;
        Dictionary<string, Tuple<string, string, string>> capitalTrabajo;
        List<DataView> depreciacionesPaging;
        Dictionary<string, string> depTotales;
        DataView amortizacionFinanciamiento;
        string invTotales, recuperacionCT, TIR, VAN, VANParticipantes, VANInvolucrados, VANIndirectos;
        List<DataView> flujoCajaPaging;

        private string signoMoneda;

        public GenerarReporte(Proyecto proyecto, string totalInversiones, DataView totalReinversiones, DataView proyeccionesTotal, DataView costosTotal, DataView capital, string recuperacionCT, DataView depTotales, DataView amortizacionFinanciamiento, DataView flujoCaja, string tir, string van)
        {
            this.proyecto = proyecto;
            this.invTotales = totalInversiones;

            signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);
            ObtieneProvincia();
            ObtieneCanton();
            ObtieneDistrito();
            ObtieneTipoOrganizacion();

            reinvTotales = SetToDictionary(totalReinversiones);
            proyeccionesTotales = SetToDictionary(proyeccionesTotal);
            costosTotales = SetToCostosTotales(costosTotal);
            capitalTrabajo = SetToCapitalTrabajo(capital);
            this.recuperacionCT = recuperacionCT;
            depreciacionesPaging = DatatableBuilder.GenerarDepreciacionesPaging(proyecto, 5);
            this.depTotales = SetToDictionary(depTotales);
            this.amortizacionFinanciamiento = amortizacionFinanciamiento;
            flujoCajaPaging = DatatableBuilder.FlujoCajaToPaging(flujoCaja, 5);
            TIR = tir;
            VAN = van;

            double vanDouble = Convert.ToDouble(VAN.Replace(signoMoneda, string.Empty));

            VANParticipantes = signoMoneda + " " + Math.Round(vanDouble / proyecto.PersonasParticipantes, 2).ToString("#,##0.##");
            VANInvolucrados = signoMoneda + " " + Math.Round(vanDouble / proyecto.FamiliasInvolucradas, 2).ToString("#,##0.##");
            VANIndirectos = signoMoneda + " " + Math.Round(vanDouble / proyecto.PersonasBeneficiadas, 2).ToString("#,##0.##");
        }

        private Dictionary<string, string> SetToDictionary(DataView dataView)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if(dataView.Table != null)
            {
                for (int i = 1; i < dataView.Table.Columns.Count; i++)
                {
                    dictionary.Add(dataView.Table.Columns[i].ColumnName,
                        dataView.Table.Rows[0].ItemArray[i].ToString());
                }
            }

            return dictionary;
        }

        private Dictionary<string, Tuple<string, string>> SetToCostosTotales(DataView dataView)
        {
            var dictionaryRoot = SetToDictionary(dataView);

            List<VariacionAnualCosto> listToAppend = proyecto.VariacionCostos;
            Dictionary<string, Tuple<string, string>> dictionary = new Dictionary<string, Tuple<string, string>>();

            if (listToAppend.Count.Equals(0))
            {
                foreach (var entry in dictionaryRoot)
                {
                    dictionary.Add(entry.Key, Tuple.Create("0", entry.Value));
                }
            }
            else
            {
                foreach (var entry in dictionaryRoot)
                {
                    dictionary.Add(entry.Key, Tuple.Create(
                        listToAppend.Find(variacion => variacion.Ano.Equals(entry.Key)).PorcentajeIncremento.ToString(),
                        entry.Value));
                }
            }


            return dictionary;
        }

        private Dictionary<string, Tuple<string, string, string>> SetToCapitalTrabajo(DataView dataView)
        {
            Dictionary<string, Tuple<string, string, string>> dictionary = new Dictionary<string, Tuple<string, string, string>>();

            if(dataView.Table != null)
            {
                for (int i = 1; i < dataView.Table.Columns.Count; i++)
                {
                    dictionary.Add(dataView.Table.Columns[i].ColumnName, Tuple.Create(
                        dataView.Table.Rows[0].ItemArray[i].ToString(), dataView.Table.Rows[1].ItemArray[i].ToString(), 
                        dataView.Table.Rows[2].ItemArray[i].ToString()));
                }
            }

            return dictionary;
        }

        private void ObtieneProvincia()
        {
            ProvinciaData provinciaData = new ProvinciaData();
            proyecto.Provincia = provinciaData.GetProvincias()
                .Find(prov => prov.CodProvincia.Equals(proyecto.Provincia.CodProvincia));
        }

        private void ObtieneCanton()
        {
            CantonData cantonData = new CantonData();
            proyecto.Canton = cantonData.GetCantonesPorProvincia(proyecto.Provincia.CodProvincia)
                .Find(cant => cant.CodCanton.Equals(proyecto.Canton.CodCanton));
        }

        private void ObtieneDistrito()
        {
            DistritoData distritoData = new DistritoData();
            proyecto.Distrito = distritoData.GetDistritosPorCanton(proyecto.Canton.CodCanton)
                .Find(dist => dist.CodDistrito.Equals(proyecto.Distrito.CodDistrito));
        }

        private void ObtieneTipoOrganizacion()
        {
            TipoOrganizacionData tipoOrgData = new TipoOrganizacionData();
            proyecto.OrganizacionProponente.Tipo = tipoOrgData.GetTipoOrganizaciones().Find(
                tipo => tipo.CodTipo.Equals(proyecto.OrganizacionProponente.Tipo.CodTipo));
        }

        public bool CrearReporte()
        {
            string htmlString = LlenarPlantillaProyecto();
            string destinoReporte = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report");

            string reportPdfPath = string.Format("{0}Sample_{1}.pdf", destinoReporte, DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (File.Exists(reportPdfPath))
                File.Delete(reportPdfPath);

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            htmlToPdf.PageWidth = 250;
            htmlToPdf.PageHeight = 300;
            htmlToPdf.Margins.Left = htmlToPdf.Margins.Right = 0.8F;
            //htmlToPdf.PageFooterHtml = "Este reporte fue generado automáticamente por Negotium - Universidad de Costa Rica";
            htmlToPdf.GeneratePdfFromFile(htmlString, null, reportPdfPath);

            System.Diagnostics.Process.Start(reportPdfPath);
            return true;
        }

        private string LlenarPlantillaProyecto()
        {
            string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report");
            string templatePath = Path.Combine(reportFolder, "template.html");
            string reportPath = Path.Combine(reportFolder, "sample.html");

            File.Copy(templatePath, reportPath, true);

            StreamReader objReader;
            objReader = new StreamReader(reportPath);
            string bodyReport = objReader.ReadToEnd();
            objReader.Close();

            Velocity.Init();
            var context = new VelocityContext();

            context.Put("proyecto", proyecto);
            context.Put("inversionesTotal", invTotales);
            context.Put("reinversionesTotal", reinvTotales);
            context.Put("proyeccionesTotal", proyeccionesTotales);
            context.Put("costosTotal", costosTotales);
            context.Put("capitalTrabajo", capitalTrabajo);
            context.Put("recuperacionCT", recuperacionCT);
            context.Put("depreciaciones", depreciacionesPaging);
            context.Put("depreciacionesTotal", depTotales);
            context.Put("financiamiento", amortizacionFinanciamiento);
            context.Put("flujoCaja", flujoCajaPaging);
            context.Put("TIR", TIR);
            context.Put("VAN", VAN);
            context.Put("VANParticipantes", VANParticipantes);
            context.Put("VANInvolucrados", VANInvolucrados);
            context.Put("VANIndirectos", VANIndirectos);

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            using (var reader = new StringReader(bodyReport))
            {
                Velocity.Evaluate(context, writer, null, reader);
            }

            StreamWriter objWriter = new StreamWriter(reportPath);
            objWriter.Write(sb.ToString());
            objWriter.Close();

            return reportPath;
        }
    }
}
