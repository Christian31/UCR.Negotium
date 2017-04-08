using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Utils
{
    public class GenerarReporte
    {
        private Proyecto proyecto;

        public GenerarReporte(Proyecto proyecto)
        {
            ObjetoInteresData objetoInteresData = new ObjetoInteresData();
            ProponenteData proponenteData = new ProponenteData();
            this.proyecto = proyecto;
            ObtieneProvincia();
            ObtieneCanton();
            ObtieneDistrito();
            this.proyecto.ObjetoInteres = objetoInteresData.GetObjetoInteres(proyecto.CodProyecto);
            this.proyecto.Proponente = proponenteData.GetProponente(proyecto.CodProyecto);
            ObtieneTipoOrganizacion();
        }

        private void ObtieneProvincia()
        {
            ProvinciaData provinciaData = new ProvinciaData();
            DataTable dtProvincia = provinciaData.GetProvincias();
            Provincia provincia = new Provincia();
            foreach (DataRow fila in dtProvincia.Rows)
            {
                if (proyecto.Provincia.CodProvincia == Int32.Parse(fila["cod_provincia"].ToString()))
                {
                    proyecto.Provincia.NombreProvincia = fila["nombre_provincia"].ToString();
                }//if
            }//foreach
        }

        private void ObtieneCanton()
        {
            CantonData cantonData = new CantonData();
            DataTable dtcanton = cantonData.GetCantonesPorProvincia(proyecto.Provincia.CodProvincia);
            Canton canton = new Canton();
            foreach (DataRow fila in dtcanton.Rows)
            {
                if (proyecto.Canton.CodCanton == Int32.Parse(fila["cod_canton"].ToString()))
                {
                    proyecto.Canton.NombreCanton = fila["nombre_canton"].ToString();
                }//if
            }//foreach
        }

        private void ObtieneDistrito()
        {
            DistritoData distritoData = new DistritoData();
            DataTable dtdistrito = distritoData.GetDistritosPorCanton(proyecto.Canton.CodCanton);
            Distrito distrito = new Distrito();
            foreach (DataRow fila in dtdistrito.Rows)
            {
                if (proyecto.Distrito.CodDistrito == Int32.Parse(fila["cod_distrito"].ToString()))
                {
                    proyecto.Distrito.NombreDistrito = fila["nombre_distrito"].ToString();
                }//if
            }//foreach
        }

        private void ObtieneTipoOrganizacion()
        {
            TipoOrganizacionData tipoOrganizacionData = new TipoOrganizacionData();
            DataTable dtTipoOrganizacion = tipoOrganizacionData.GetTiposDeOrganizacion();
            TipoOrganizacion tipoOrganizacion = new TipoOrganizacion();
            foreach (DataRow fila in dtTipoOrganizacion.Rows)
            {
                if (proyecto.Proponente.Organizacion.Tipo.CodTipo == Int32.Parse(fila["cod_tipo"].ToString()))
                {
                    proyecto.Proponente.Organizacion.Tipo.Descripcion = fila["descripcion"].ToString();
                }
            }
        }

        public bool CrearReporte()
        {
            string htmlString = LlenarPlantillaProyecto();

            string destinoReporte = @"C:\Users\Christian\Source\Repos\UCR.Negotium\UCR.Negotium.WindowsUI\Report\";

            string pdf_page_size = "5";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;

            int webPageWidth = 1200;
            int webPageHeight = 5000;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            //converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(htmlString);

            // save pdf document
            doc.Save(destinoReporte + "Sample.pdf");

            // close pdf document
            doc.Close();

            System.Diagnostics.Process.Start(destinoReporte + "Sample.pdf");
            return true;
        }

        private string LlenarPlantillaProyecto()
        {
            string templatePath = @"C:\Users\Christian\Source\Repos\UCR.Negotium\UCR.Negotium.WindowsUI\Report\template.html";
            string reportPath = @"C:\Users\Christian\Source\Repos\UCR.Negotium\UCR.Negotium.WindowsUI\Report\sample.html";

            File.Copy(templatePath, reportPath, true);

            StreamReader objReader;
            objReader = new StreamReader(reportPath);
            string bodyReport = objReader.ReadToEnd();
            objReader.Close();

            bodyReport = bodyReport.Replace(NOMBREPROYECTO, proyecto.NombreProyecto);
            bodyReport = bodyReport.Replace(ANOPROYECTO, proyecto.AnoInicial.ToString());
            bodyReport = bodyReport.Replace(HORIZONTEEVALUACION, proyecto.HorizonteEvaluacionEnAnos.ToString() + " Años");
            bodyReport = bodyReport.Replace(PAGAIMPUESTOS, proyecto.PagaImpuesto ? "Si" : "No");
            bodyReport = bodyReport.Replace(PORCENTAJEIMPUESTOS, proyecto.PorcentajeImpuesto.ToString() + " %");
            bodyReport = bodyReport.Replace(TIPOPROYECTO, proyecto.ConIngresos ? "Con Ingresos" : "Sin Ingresos");
            bodyReport = bodyReport.Replace(BIENOSERVICIO, proyecto.ObjetoInteres.DescripcionObjetoInteres);
            bodyReport = bodyReport.Replace(UNIDADMEDIDABIENOSERVICIO, proyecto.ObjetoInteres.UnidadMedida.NombreUnidad);
            bodyReport = bodyReport.Replace(CANTON, proyecto.Canton.NombreCanton);
            bodyReport = bodyReport.Replace(PROVINCIA, proyecto.Provincia.NombreProvincia);
            bodyReport = bodyReport.Replace(DISTRITO, proyecto.Distrito.NombreDistrito);
            bodyReport = bodyReport.Replace(DIRECCIONEXACTA, proyecto.DireccionExacta);
            bodyReport = bodyReport.Replace(RESUMENEJECUTIVO, proyecto.ResumenEjecutivo);
            bodyReport = bodyReport.Replace(NOMBREREPRESENTANTE, proyecto.Proponente.Nombre + " " + proyecto.Proponente.Apellidos);
            bodyReport = bodyReport.Replace(CEDULAREPRESENTANTE, proyecto.Proponente.NumIdentificacion);
            bodyReport = bodyReport.Replace(TELEFONOREPRESENTANTE, proyecto.Proponente.Telefono);
            bodyReport = bodyReport.Replace(PUESTOREPRESENTANTE, proyecto.Proponente.PuestoEnOrganizacion);
            bodyReport = bodyReport.Replace(GENEROREPRESENTANTE, proyecto.Proponente.Genero.Equals(0) ? "Masculino" : "Femenino");
            bodyReport = bodyReport.Replace(INDIVIDUALREPRESENTANTE, proyecto.Proponente.EsRepresentanteIndividual.Equals(1) ? "Si" : "No");
            bodyReport = bodyReport.Replace(TIPOORGANIZACION, proyecto.Proponente.Organizacion.Tipo.Descripcion);
            bodyReport = bodyReport.Replace(NOMBREORGANIZACION, proyecto.Proponente.Organizacion.NombreOrganizacion);
            bodyReport = bodyReport.Replace(CEDULAORGANIZACION, proyecto.Proponente.Organizacion.CedulaJuridica);
            bodyReport = bodyReport.Replace(TELEFONOORGANIZACION, proyecto.Proponente.Organizacion.Telefono);
            bodyReport = bodyReport.Replace(POBLACIONCARACTERIZACION, proyecto.DescripcionPoblacionBeneficiaria);
            bodyReport = bodyReport.Replace(SERVICIOCARACTERIZACION, proyecto.CaraterizacionDelBienServicio);
            bodyReport = bodyReport.Replace(DESCRIPCIONCARACTERIZACION, proyecto.DescripcionSostenibilidadDelProyecto);
            bodyReport = bodyReport.Replace(JUSTIFICATIONCARACTERIZACION, proyecto.JustificacionDeMercado);

            StreamWriter objWriter = new StreamWriter(reportPath);
            objWriter.Write(bodyReport);
            objWriter.Close();

            return reportPath;
        }

        #region Constantes
        private const string NOMBREPROYECTO = "{{NOMBREPROYECTO}}";
        private const string ANOPROYECTO = "{{ANOINICIAL}}";
        private const string HORIZONTEEVALUACION = "{{HORIZONTEEVALUACION}}";
        private const string PAGAIMPUESTOS = "{{PAGAIMPUESTOS}}";
        private const string PORCENTAJEIMPUESTOS = "{{PORCENTAJEIMPUESTO}}";
        private const string TIPOPROYECTO = "{{TIPOPROYECTO}}";
        private const string BIENOSERVICIO = "{{BIENOSERVICIO}}";
        private const string UNIDADMEDIDABIENOSERVICIO = "{{UNIDADMEDIDA}}";
        private const string CANTON = "{{CANTON}}";
        private const string PROVINCIA = "{{PROVINCIA}}";
        private const string DISTRITO = "{{DISTRITO}}";
        private const string DIRECCIONEXACTA = "{{DIRECCIONEXACTA}}";
        private const string RESUMENEJECUTIVO = "{{RESUMENEJECUTIVO}}";

        private const string NOMBREREPRESENTANTE = "{{NOMBREREPRESENTANTE}}";
        private const string CEDULAREPRESENTANTE = "{{CEDULAREPRESENTANTE}}";
        private const string TELEFONOREPRESENTANTE = "{{TELEFONOREPRESENTANTE}}";
        private const string PUESTOREPRESENTANTE = "{{PUESTOREPRESENTANTE}}";
        private const string GENEROREPRESENTANTE = "{{GENEROREPRESENTANTE}}";
        private const string INDIVIDUALREPRESENTANTE = "{{INDIVIDUALREPRESENTANTE}}";
        private const string TIPOORGANIZACION = "{{TIPOORGANIZACION}}";
        private const string NOMBREORGANIZACION = "{{NOMBREORGANIZACION}}";
        private const string CEDULAORGANIZACION = "{{CEDULAORGANIZACION}}";
        private const string TELEFONOORGANIZACION = "{{TELEFONOORGANIZACION}}";

        private const string POBLACIONCARACTERIZACION = "{{POBLACIONCARACTERIZACION}}";
        private const string SERVICIOCARACTERIZACION = "{{SERVICIOCARACTERIZACION}}";
        private const string DESCRIPCIONCARACTERIZACION = "{{DESCRIPCIONCARACTERIZACION}}";
        private const string JUSTIFICATIONCARACTERIZACION = "{{JUSTIFICATIONCARACTERIZACION}}";
        #endregion
    }
}
