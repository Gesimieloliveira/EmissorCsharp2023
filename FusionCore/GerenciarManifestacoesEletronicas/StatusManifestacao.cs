using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum StatusManifestacao
    {
        [Description("Desconhecida")]
        Desconhecida,

        [Description("Ciência da Operação")]
        CienciaOperacao,

        [Description("Confirmação da Operação")]
        ConfirmacaoOperacao,

        [Description("Desconhecimento da Operação")]
        DesconhecimentoOperacao,

        [Description("Operação Não Realizada")]
        OperacaoNaoRealizada,

        [Description("Download XML")]
        DownloadXml
    }
}