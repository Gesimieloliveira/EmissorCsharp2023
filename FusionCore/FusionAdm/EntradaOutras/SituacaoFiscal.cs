using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.EntradaOutras
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum SituacaoFiscal
    {
        [Description("Documento Fiscal Normal")]
        Normal = 1,

        [Description("Documento Fiscal Cancelado")]
        Cancelado = 2,

        [Description("Lançamento Extemporâneo de Documento Fiscal Normal")]
        ExtemporaneoNormal = 3,

        [Description("Lançamento Extemporâneo de Documento Fiscal Cancelado")]
        ExtemporaneoCancelado = 4,

        [Description("Documento com USO DENEGADO - exclusivamente para uso dos emitentes de Nota Fiscal Eletrônica - Modelo 55, Conhecimento de Transporte Eletrônico, Modelo 57 e Conhecimento de Transporte Eletrônico para Outros Serviços, modelo 67.")]
        Denegada = 5,

        [Description("Documento com USO inutilizado - exclusivamente para uso dos emitentes de Nota Fiscal Eletrônica - Modelo 55, Conhecimento de Transporte Eletrônico, Modelo 57 e Conhecimento de Transporte Eletrônico para Outros Serviços, modelo 67.")]
        Inutilizado = 6
    }
}