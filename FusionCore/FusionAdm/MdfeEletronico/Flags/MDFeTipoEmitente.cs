using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum MDFeTipoEmitente
    {
        [Description("1 - Prestador de Serviço de Transporte")]
        PrestadorServicoDeTransporte = 1,

        [Description("2 - Transportador de Carga Própria")]
        TransportadorDeCargaPropria = 2
    }
}