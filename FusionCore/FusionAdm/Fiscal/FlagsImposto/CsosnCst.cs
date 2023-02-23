using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.FlagsImposto
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum CsosnCst
    {
        [Description("101 - Tributada pelo Simples Nacional com permissão de crédito")] CST101 = 101,
        [Description("102 - Tributada pelo Simples Nacional sem permissão de crédito")] CST102 = 102,
        [Description("103 - Isenção do ICMS no Simples Nacional para faixa de receita bruta")] CST103 = 103,
        [Description("201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS substituição tributária")] CST201 = 201,
        [Description("202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS substituição tributária")] CST202 = 202,
        [Description("203 - Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária")] CST203 = 203,
        [Description("300 - Imune")] CST300 = 300,
        [Description("400 - Não tributada pelo Simples Nacional")] CST400 = 400,
        [Description("500 - ICMS cobrado anteriormente por substituição tributária")] CST500 = 500,
        [Description("900 - Outros")] CST900 = 900
    }
}