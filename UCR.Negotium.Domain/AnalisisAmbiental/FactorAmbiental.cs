namespace UCR.Negotium.Domain
{
    public class FactorAmbiental
    {
        public int CodFactorAmbiental { get; set; }
        public string NombreFactor { get; set; }
        public int CodProyecto { get; set; }
        public int CodCondicionAfectada { get; set; }
        public int CodElementoAmbiental { get; set; }
        public int CodSigno { get; set; }
        public int CodIntensidad { get; set; }
        public int CodExtension { get; set; }
        public int CodMomento { get; set; }
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
    }
}
