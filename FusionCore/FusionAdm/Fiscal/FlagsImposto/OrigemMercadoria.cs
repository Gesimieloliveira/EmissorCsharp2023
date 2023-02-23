using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Fiscal.FlagsImposto
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum OrigemMercadoria
    {
        [Description("0 - Nacional")]
        Nacional = 0,

        [Description("1 - Estrangeira Importação Direta")]
        EstrangeiraImportacaoDireta = 1,

        [Description("2 - Estrangeira Adquirida no Mercado Interno")]
        EstrangeiraInterna = 2,

        [Description("3 - Nacional Importação Superior 40%")]
        NacionalImportacaoSuperior40 = 3,

        [Description("4 - Nacional Confirmidade Básicas")]
        NacionalConformidadeBasicas = 4,

        [Description("5 - Nacional Importação Inferior 40%")]
        NacionalImportacaoInferior40 = 5,

        [Description("6 - Estrangeira Importação Direta Sem Similar")]
        EstrangeiraImportacaoDiretaSemSimilar = 6,

        [Description("7 - Estrangeira Interna Sem Similar")]
        EstrangeiraInternaSemSimilar = 7,

        [Description("8 - Nacional Importação Superior 70%")]
        NacionalImportacaoSuperior70 = 8
    }
}