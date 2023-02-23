using System.ComponentModel;

namespace FusionCore.MigracaoFluente
{
    public enum MigracaoTag
    {
        [Description("FA")]
        Adm = 0,

        [Description("FN")]
        Nfce = 1,

        [Description("RR")]
        Relatorio = 2
    }
}