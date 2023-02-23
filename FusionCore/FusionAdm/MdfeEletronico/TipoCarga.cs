using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoCarga
    {
        [Description("Nenhuma")]
        Nenhuma = 00,

        [Description("Granel sólido")]
        GranelSolido = 01,

        [Description("Granel líquido")]
        GranelLiquido = 02,

        [Description("Frigorificada")]
        Frigorificada = 03,

        [Description("Conteinerizada")]
        Conteinerizada = 04,

        [Description("Carga Geral")]
        CargaGeral = 05,

        [Description("Neogranel")]
        Neogranel = 06,

        [Description("Perigosa (granel sólido)")]
        PerigosaGranelSolido = 07,

        [Description("Perigosa (granel líquido)")]
        PerigosaGranelLiquido = 08,

        [Description("Perigosa (carga frigorificada)")]
        PerigosaCargaFrigorificada = 09,

        [Description("Perigosa (conteinerizada)")]
        PerigosaConteinerizada = 10,

        [Description("Perigosa (carga geral)")]
        PerigosaCargaGeral = 11
    }
}