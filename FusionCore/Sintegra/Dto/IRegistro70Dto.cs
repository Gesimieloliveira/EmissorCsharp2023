using System;

namespace FusionCore.Sintegra.Dto
{
    public interface IRegistro70Dto
    {
        string GetDocumentoUnico();
        string GetInscricaoEstadual();
        DateTime GetEmissaoRecebimento();
        string GetUf();
        int GetModelo();
        string GetSerie();
        string GetSubSerie();
        int GetNumero();
        int GetCfop();
        decimal GetValorTotal();
        decimal GetBaseCalculoIcms();
        decimal GetValorIcms();
        decimal? GetValorOutras();
        string GetSituacaoNotaFiscal();
        int GetCifFob();
    }
}