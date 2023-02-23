using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.EntradaOutras
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ModeloDocumentoOutro
    {
        [Description("Nota Fiscal/Conta de Energia Elétrica, modelo 6 (código 06)")]
        ContaEnergiaEletrica = 06,

        [Description("Nota Fiscal de Serviço de Comunicação, modelo 21 (código 21)")]
        Comunicacao = 21,

        [Description("Nota Fiscal de Serviços de Telecomunicações, modelo 22 (código 22)")]
        Telecomunicacao = 22
    }
}