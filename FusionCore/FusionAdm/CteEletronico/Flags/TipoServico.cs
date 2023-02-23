using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoServico
    {
        [Description("Normal")]
        Normal = 0,

        [Description("Subcontratação")]
        Subcontratacao = 1
        //Redespacho = 2,
        //RedespachoIntermediario = 3,
        //ServicoVinculadoAMultiModal = 4
    }
}