using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.FlagsImposto
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum CofinsCst
    {
        [Description("01 - Operação Tributável com Alíquota Básica")] CST01 = 01,
        [Description("02 - Operação Tributável com Alíquota Diferenciada")] CST02 = 02,
        [Description("03 - Operação Tributável com Alíquota por Unidade de Medida de Produto")] CST03 = 03,
        [Description("04 - Operação Tributável Monofásica - Revenda a Alíquota Zero")] CST04 = 04,
        [Description("05 - Operação Tributável por Substituição Tributária")] CST05 = 05,
        [Description("06 - Operação Tributável a Alíquota Zero")] CST06 = 06,
        [Description("07 - Operação Isenta da Constribuição")] CST07 = 07,
        [Description("08 - Operação sem Icidência da Contribuição")] CST08 = 08,
        [Description("09 - Operação com Suspensã da Contribuição")] CST09 = 09,
        [Description("49 - Outras Operações de Saída")] CST49 = 49,
        [Description("50 - Operação com Direito a Crédito - Vinc Exclusiv a Rec Trib no Merc Int")] CST50 = 50,
        [Description("51 - Operação com Direito a Crédito - Vinc Exclusiv a Rec Não Trib no Merc Int")] CST51 = 51,
        [Description("52 - Operação com Direito a Crédito - Vinc Exclusiv a Rec de Exp")] CST52 = 52,
        [Description("53 - Operação com Direito a Crédito - Vinc a Rec Trib e Não-Trib no Merc Int")] CST53 = 53,
        [Description("54 - Operação com Direito a Crédito - Vinc a Rec Trib no Merc Int e de Exp")] CST54 = 54,
        [Description("55 - Operação com Direito a Crédito - Vinc a Rec Não-Trib no Mer Int e de Exp")] CST55 = 55,
        [Description("56 - Operação com Direito a Crédito - Vinc a Rec Trib e Não-Trib no Merc Int e de Exp")] CST56 = 56,
        [Description("60 - Crédito Presumido - Operação de Aqu Vinc Exclusiv a Rec Trib no Merc Int")] CST60 = 60,
        [Description("61 - Crédito Presumido - Operação de Aqu Vinc Exclusiv a Rec Não-Trib no Merc Int")] CST61 = 61,
        [Description("62 - Crédito Presumido - Operação de Aqu Vinc Exclusiv a Rec de Exp")] CST62 = 62,
        [Description("63 - Crédito Presumido - Operação de Aqu Vinc a Rec Trib e Não-Trib no Merc Int")] CST63 = 63,
        [Description("64 - Crédito Presumido - Operação de Aqu Vinc a Rec Trib no Merc Int e de Exp")] CST64 = 64,
        [Description("65 - Crédito Presumido - Operação de Aqu Vinc a Rec Não-Trib no Merc Int e de Exp")] CST65 = 65,
        [Description("66 - Crédito Presumido - Operação de Aqu Vinc a Rec Trib e Não-Trib no Merc Int e de Exp")] CST66 = 66,
        [Description("67 - Crédito Presumido - Outras Operações")] CST67 = 67,
        [Description("70 - Operação de Aquisição sem Direito a Crédito")] CST70 = 70,
        [Description("71 - Operação de Aquisição com Isenção")] CST71 = 71,
        [Description("72 - Operação de Aquisição com Suspensão")] CST72 = 72,
        [Description("73 - Operação de Aquisição a Alíquota Zero")] CST73 = 73,
        [Description("74 - Operação de Aquisição sem Incidência da Contribuição")] CST74 = 74,
        [Description("75 - Operação de Aquisição por Substituição Tributária")] CST75 = 75,
        [Description("98 - Outras Operações de Entrada")] CST98 = 98,
        [Description("99 - Outras Operações")] CST99 = 99
    }
}