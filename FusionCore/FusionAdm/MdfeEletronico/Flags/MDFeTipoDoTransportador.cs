using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum MDFeTipoDoTransportador
    {
        [Description("Não informado")]
        NaoInformado = 0,
        ETC = 1,
        TAC = 2,
        CTC = 3
    }
}