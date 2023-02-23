using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoCarroceria
    {
        [Description("Não Aplicável")]
        NaoAplicavel,
        [Description("Aberta")]
        Abera,
        [Description("Fechada/Baú")]
        FechadaBau,
        [Description("Graneleira")]
        Graneleira,
        [Description("Porta Container")]
        PortaContainer,
        [Description("Sider")]
        Sider
    }
}