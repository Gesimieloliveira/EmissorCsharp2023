using System;

namespace FusionCore.Sintegra.Dto
{
    public interface IRegistro61Dto
    {
        DateTime ObterEmissaoNoDiaEm();
        int ObterModelo();
        string ObterSerie();
        string ObterSubserie();
        string ObterNumeroInicialDia();
        string ObterNumeroFinalDia();
        decimal ObterValorTotalDiario();
        decimal ObterBaseCalculoIcmsDiario();
        decimal ObterValorIcmsTotalDiario();
        decimal ObterValorAmparadoPorInsencaoOuNaoIncidencia();
        decimal ObterValorOutrasDiario();
        decimal ObterAliquotaIcms();
    }
}