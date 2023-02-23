using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ModeloNotaFiscal
    {
        [Description("NF Modelo 01/1A e Avulsa")]
        NFModelo011AeAvulsa = 1,
        [Description("NF de Produtor")]
        NFdeProdutor = 4
    }
}