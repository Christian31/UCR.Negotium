using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCR.Negotium.Domain;
using UCR.Negotium.DataAccess;

namespace DataTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RequerimientoInversion requerimiento = new RequerimientoInversion();
            RequerimientoInversionData requerimientoData = new RequerimientoInversionData();
            requerimiento.Cantidad = 5;
            requerimiento.CostoUnitario = 200;
            requerimiento.Depreciable = true;
            requerimiento.DescripcionRequerimiento = "Soy una descripcion";
            requerimiento.UnidadMedida.CodUnidad = 2;
            requerimiento.VidaUtil = 10;
            requerimientoData.InsertarRequerimientosInvesion(requerimiento, 2);
        }
    }
}
