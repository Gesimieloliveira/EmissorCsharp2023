using System;
using FusionCore.Vendas.Autorizadores.Nfce;

namespace FusionCore.Cupom.Nfce
{
    public interface IFiltroCupomFiscalDto
    {
        DateTime? EmitidasIgualOuApos { get; }
        int? NumeroIgual { get; }
        int? CodigoIdIgualA { get; }
        string NomeEmpresaContenha { get; }
        string NomeClienteContenha { get; }
        TipoEmissaoCupomFiscal? TipoEmissao { get; }
        SituacaoFiscal? SituacaoFiscal { get; }
    }
}