using System;

namespace FusionCore.Sintegra.Dto
{
    public interface IRegistro50Dto
    {
        string GetDocumentoUnico();
        string GetInscricaoEstadual();
        DateTime GetEmissaoRecebimento();
        string GetUf();
        int GetModelo();
        string GetSerie();
        int GetNumero();
        int GetCfop();
        string GetEmitente();
        decimal GetValorTotal();
        decimal GetBaseCalculoIcms();
        decimal GetValorIcms();
        decimal? GetValorOutras();
        decimal GetAliquotaIcms();
        string GetSituacaoNotaFiscal();
        decimal GetValorSt();
    }
}