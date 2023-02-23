using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum ModalidadeFrete
    {
        [Description("Por conta do emitente")] PorContaEmitente = 0,
        [Description("Por conta do destinatário/remente")] PorContaDestintario = 1,
        [Description("Por conta de terceiros")] PorContaTerceiros = 2,
        [Description("Sem cobrança de frete")] SemFrete = 9
    }
}