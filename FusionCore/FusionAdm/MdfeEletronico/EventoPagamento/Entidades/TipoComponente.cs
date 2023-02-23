using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoComponente
    {
        [Description("Vale Pedágio")]
        ValePedagio = 01,

        [Description("Impostos, taxas e contribuições")]
        ImpostosTaxasContribuicoes = 02,

        [Description("Despesas (bancárias, meios de pagamento, outras)")]
        DespesasBancariasMeiosPagamentoOutras = 03,

        [Description("Outros")]
        Outros = 99
    }
}