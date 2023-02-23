using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Financeiro.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoLancamento
    {
        [Description("Juros")]
        Juro = 0,
        [Description("Desconto")]
        Desconto = 1,
        [Description("Pagamento")]
        Pagamento = 2,
        [Description("Ajuste Para Mais")]
        AjusteParaMais = 4,
        [Description("Ajuste Para Menos")]
        AjusteParaMenos = 5
    }
}