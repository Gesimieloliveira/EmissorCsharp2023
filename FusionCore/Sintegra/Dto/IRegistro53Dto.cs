using System;

namespace FusionCore.Sintegra.Dto
{
    public interface IRegistro53Dto
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
        decimal GetBaseCalculoIcmsSt();
        decimal GetIcmsRetido();
        decimal GetDespesasAcessorias();
        string GetSituacaoNotaFiscal();
        string GetCodigoAntecipacao();
    }
}