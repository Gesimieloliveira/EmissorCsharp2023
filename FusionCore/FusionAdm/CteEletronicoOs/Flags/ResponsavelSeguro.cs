using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronicoOs.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ResponsavelSeguro
    {
        [Description("Nenhum")]
        Nenhum = 0,

        [Description("4 - Emitente do CT-e")]
        Emitente = 4,

        [Description("5 - Tomador de Serviço")]
        TomadorDeServico = 5
    }
}