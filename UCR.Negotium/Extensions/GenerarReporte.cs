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
        private string invTotales;
        Dictionary<string, string> reinvTotales;
        Dictionary<string, string> proyeccionesTotales;
        Dictionary<string, string> costosTotales;

        private string signoMoneda;

        public GenerarReporte(Proyecto proyecto, string totalInversiones, DataView totalReinversiones, DataView proyeccionesTotal, DataView costosTotal)
        {
            this.proyecto = proyecto;
            this.invTotales = totalInversiones;

            signoMoneda = LocalContext.GetSignoMoneda(proyecto.CodProyecto);
            ObtieneProvincia();
            ObtieneCanton();
            ObtieneDistrito();

            reinvTotales = SetToDictionary(totalReinversiones);
            proyeccionesTotales = SetToDictionary(proyeccionesTotal);
            costosTotales = SetToDictionary(costosTotal);
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

        public bool CrearReporte()
        {
            string htmlString = LlenarPlantillaProyecto();
            string destinoReporte = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report");

            string reportPdfPath = destinoReporte + "Sample.pdf";
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
