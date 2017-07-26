using System;
using UCR.Negotium.Base.Enums;

namespace UCR.Negotium.Domain
{
    public class FactorAmbiental
    {
        public int CodFactorAmbiental { get; set; }
        public string NombreFactor { get; set; }
        public int CodProyecto { get; set; }
        public int CodCondicionAfectada { get; set; }
        public int CodElementoAmbiental { get; set; }
        public bool Signo { get; set; } //true= impacto beneficioso, false= impacto perjudicial
        public int CodIntensidad { get; set; }
        public int CodExtension { get; set; }
        public bool ExtensionCritico { get; set; }
        public int CodMomento { get; set; }
        public bool MomentoCritico { get; set; }
        public int CodPersistencia { get; set; }
        public int CodReversibilidad { get; set; }
        public int CodSinergia { get; set; }
        public int CodAcumulacion { get; set; }
        public int CodEfecto { get; set; }
        public int CodPeriodicidad { get; set; }
        public int CodRecuperabilidad { get; set; }

        public int CodClasificacion { get; set; }

        public FactorAmbiental()
        {

        }

        public string NombreSigno
        {
            get
            {
                return Signo ? "Beneficioso" : "Perjudicial";
            }
        }

        public string NombreCondicionAfectada
        {
            get
            {
                CondicionAfectada cond = CondicionAfectada.Biotopos;
                Enum.TryParse(CodCondicionAfectada.ToString(), out cond);
                return EnumsHelper<CondicionAfectada>.GetDisplayValue(cond);
            }
        }

        public string NombreElementoAmbiental
        {
            get
            {
                switch (CodCondicionAfectada)
                {
                    case 0:
                        ElementoAmbientalCond1 elemAmbiental1 = ElementoAmbientalCond1.Agua;
                        Enum.TryParse(CodElementoAmbiental.ToString(), out elemAmbiental1);
                        return EnumsHelper<ElementoAmbientalCond1>.GetDisplayValue(elemAmbiental1);
                    case 1:
                        ElementoAmbientalCond2 elemAmbiental2 = ElementoAmbientalCond2.CalidadVisual;
                        Enum.TryParse(CodElementoAmbiental.ToString(), out elemAmbiental2);
                        return EnumsHelper<ElementoAmbientalCond2>.GetDisplayValue(elemAmbiental2);
                    case 2:
                        ElementoAmbientalCond3 elemAmbiental3 = ElementoAmbientalCond3.FloraFauna;
                        Enum.TryParse(CodElementoAmbiental.ToString(), out elemAmbiental3);
                        return EnumsHelper<ElementoAmbientalCond3>.GetDisplayValue(elemAmbiental3);
                    case 3:
                        ElementoAmbientalCond4 elemAmbiental4 = ElementoAmbientalCond4.Economico;
                        Enum.TryParse(CodElementoAmbiental.ToString(), out elemAmbiental4);
                        return EnumsHelper<ElementoAmbientalCond4>.GetDisplayValue(elemAmbiental4);
                    case 4:
                        ElementoAmbientalCond5 elemAmbiental5 = ElementoAmbientalCond5.Patrimonio;
                        Enum.TryParse(CodElementoAmbiental.ToString(), out elemAmbiental5);
                        return EnumsHelper<ElementoAmbientalCond5>.GetDisplayValue(elemAmbiental5);
                    default:
                        ElementoAmbientalCond1 elemAmbiental = ElementoAmbientalCond1.Agua;
                        Enum.TryParse(CodElementoAmbiental.ToString(), out elemAmbiental);
                        return EnumsHelper<ElementoAmbientalCond1>.GetDisplayValue(elemAmbiental);
                }
            }
        }

        public string NombreClasificacion
        {
            get
            {
                Clasificacion clas = Clasificacion.Critico;
                Enum.TryParse(CodClasificacion.ToString(), out clas);
                return EnumsHelper<Clasificacion>.GetDisplayValue(clas);
            }
        }
    }
}
