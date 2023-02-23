using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Tributacoes.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum RegimeTributario
    {
        [Description("Simples Nacional")]
        SimplesNacional = 1,

        [Description("Simples Nacional - com excesso de sublimite")]
        SimplesNacionalExcesso = 2,

        [Description("Regime Normal")]
        RegimeNormal = 3
    }
}