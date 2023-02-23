using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum MDFeResponsavelSeguro
    {
        [Description("Emitente")]
        Emitente = 1,

        [Description("Responsável pela contratação do serviço de transporte (contratante)")]
        ContratanteServicoTransporte = 2
    }
}