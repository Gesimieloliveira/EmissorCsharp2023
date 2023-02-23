using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum IndicadorPagamento
    {
        [Description("Pagamento à Vista")]
        PagamentoAVista = 0,

        [Description("Pagamento à Prazo")]
        PagamentoAPrazo = 1
    }
}