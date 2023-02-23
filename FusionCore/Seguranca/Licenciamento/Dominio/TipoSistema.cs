using System.ComponentModel;
using System.Runtime.Serialization;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    [DataContract]
    [TypeConverter(typeof (EnumDescriptionConverter))]
    public enum TipoSistema
    {
        [EnumMember]
        [Description("Fusion Administrador")]
        FusionAdm,

        [EnumMember]
        [Description("Fusion PDV")]
        FusionPdv,

        [EnumMember]
        [Description("Fusion NFC-E")]
        FusionNFCE
    }
}