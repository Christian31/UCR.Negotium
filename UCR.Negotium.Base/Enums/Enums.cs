using System.ComponentModel.DataAnnotations;

namespace UCR.Negotium.Base.Enums
{
    #region CondicionAfectada
    public enum CondicionAfectada
    {
        [Display(Name = "Físicas y Químicas")]
        FisicasYQuimicas,

        [Display(Name = "Paisaje")]
        Paisaje,

        [Display(Name = "Biotopos")]
        Biotopos,

        [Display(Name = "Económico y Social")]
        EconomicoYSocial,

        [Display(Name = "Histórico y Cultural")]
        HistoricoYCultural,
    }
    #endregion

    #region ElementoAmbientalCondicion
    public enum ElementoAmbientalCond1
    {
        [Display(Name = "Agua")]
        Agua,

        [Display(Name = "Aire")]
        Aire,

        [Display(Name = "Suelos")]
        Suelos
    }

    public enum ElementoAmbientalCond2
    {
        [Display(Name = "Calidad Visual")]
        CalidadVisual
    }

    public enum ElementoAmbientalCond3
    {
        [Display(Name = "Flora / Fauna")]
        FloraFauna
    }

    public enum ElementoAmbientalCond4
    {
        [Display(Name = "Social")]
        Social,

        [Display(Name = "Económico")]
        Economico
    }

    public enum ElementoAmbientalCond5
    {
        [Display(Name = "Patrimonio")]
        Patrimonio
    }
    #endregion

    #region EvaluacionImpactos
    public enum Signo
    {
        [Display(Name = "Impacto Beneficioso (+1)")]
        ImpactoBeneficioso = 1,

        [Display(Name = "Impacto Perjudicial (-1)")]
        ImpactoPerjudicial = -1
    }

    public enum Intensidad
    {
        [Display(Name = "Baja (1)")]
        Baja = 1,

        [Display(Name = "Media (2)")]
        Media = 2,

        [Display(Name = "Alta (4)")]
        Alta = 4,

        [Display(Name = "Muy Alta (8)")]
        MuyAlta = 8,

        [Display(Name = "Total (12)")]
        Total = 12
    }

    public enum Extension
    {
        [Display(Name = "Puntual (1)")]
        Puntual = 1,

        [Display(Name = "Parcial (2)")]
        Parcial = 2,

        [Display(Name = "Extenso (4)")]
        Extenso = 4,

        [Display(Name = "Total (8)")]
        Total = 8
    }

    public enum Momento
    {
        [Display(Name = "Largo Plazo (1)")]
        LargoPlazo = 1,

        [Display(Name = "Mediano Plazo (2)")]
        MedianoPlazo = 2,

        [Display(Name = "Inmediato (4)")]
        Inmediato = 4
    }

    public enum Persistencia
    {
        [Display(Name = "Fugaz (1)")]
        Fugaz = 1,

        [Display(Name = "Temporal (2)")]
        Temporal = 2,

        [Display(Name = "Permanente (4)")]
        Permanente = 4
    }

    public enum Reversibilidad
    {
        [Display(Name = "Corto Plazo (1)")]
        CortoPlazo = 1,

        [Display(Name = "Mediano Plazo (2)")]
        MedianoPlazo = 2,

        [Display(Name = "Irreversible (4)")]
        Irreversible = 4
    }

    public enum Sinergia
    {
        [Display(Name = "Sin Sinergismo (Simple) (1)")]
        SinSinergismo = 1,

        [Display(Name = "Sinérgico (2)")]
        Sinergico = 2,

        [Display(Name = "Muy Sinérgico (4)")]
        MuySinergico = 4
    }

    public enum Acumulacion
    {
        [Display(Name = "Simple (1)")]
        Simple = 1,

        [Display(Name = "Acumulativo (4)")]
        Acumulativo = 4,
    }

    public enum Efecto
    {
        [Display(Name = "Indirecto (Secundario) (1)")]
        Indirecto = 1,

        [Display(Name = "Directo (4)")]
        Directo = 4,
    }

    public enum Periodicidad
    {
        [Display(Name = "Irregular, Esporádico y Dicontinuo (1)")]
        Irregular = 1,

        [Display(Name = "Periódico (2)")]
        Periodico = 2,

        [Display(Name = "Continuo (4)")]
        Continuo = 4
    }

    public enum Recuperabilidad
    {
        [Display(Name = "Recuperable Inmediato (1)")]
        RecuperableInmediato = 1,

        [Display(Name = "Recuperable Mediano Plazo (2)")]
        RecuperableMedianoPlazo = 2,

        [Display(Name = "Recuperable Parcialmente, Mitigable y/o Compensable (4)")]
        RecuperableParcialmente = 4,

        [Display(Name = "Irrecuperable (8)")]
        Irrecuperable = 8
    }
    #endregion

    #region Clasificacion
    public enum Clasificacion
    {
        [Display(Name = "Irrelevante")]
        Irrelevante,

        [Display(Name = "Moderado")]
        Moderado,

        [Display(Name = "Severo")]
        Severo,

        [Display(Name = "Crítico")]
        Critico
    }
    #endregion
}
