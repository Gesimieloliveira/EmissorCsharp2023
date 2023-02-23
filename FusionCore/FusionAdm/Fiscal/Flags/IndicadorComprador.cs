using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum IndicadorComprador
    {
        [Description("0 - Não se aplica")]
        NaoSeAplica = 0,

        [Description("1 - Operação presencial")]
        Presencial = 1,

        [Description("2 - Operação internet (não presencial)")]
        Internet = 2,

        [Description("3 - Operação tele-atendimento (não presencial)")]
        TeleAtendimento = 3,

        [Description("4 - NFC-e em operação com entrega domicílio")]
        EntregaDomicilio = 4,

        [Description("9 - Não presencial (outros)")]
        Outros = 9
    }
}