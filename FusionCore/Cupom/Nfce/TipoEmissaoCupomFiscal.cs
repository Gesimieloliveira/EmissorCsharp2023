using System;
using System.ComponentModel;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Cupom.Nfce
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoEmissaoCupomFiscal
    {
        [Description("Normal")]
        Normal = 1,

        [Description("Contingência")]
        Contingencia = 2
    }

    public static class ExtTipoEmissao
    {
        public static TipoEmissao ToEmissaoFiscal(this TipoEmissaoCupomFiscal emissaoCupomFiscal)
        {
            switch (emissaoCupomFiscal)
            {
                case TipoEmissaoCupomFiscal.Normal:
                    return TipoEmissao.Normal;
                case TipoEmissaoCupomFiscal.Contingencia:
                    return TipoEmissao.ContigenciaOfflineNFCe;
                default:
                    throw new ArgumentOutOfRangeException(nameof(emissaoCupomFiscal), emissaoCupomFiscal, null);
            }
        }
    }

    public static class ExtStatusNfce
    {
        public static Status ToStatus(this SituacaoFiscal situacaoFiscal)
        {
            switch (situacaoFiscal)
            {
                case SituacaoFiscal.Aberta:
                    return Status.Aberta;

                case SituacaoFiscal.Cancelado:
                    return Status.Cancelada;

                case SituacaoFiscal.Autorizada:
                    return Status.Transmitida;

                case SituacaoFiscal.RejeicaoOffline:
                    return Status.Aberta;

                case SituacaoFiscal.AutorizadaSemInternet:
                    return Status.PendenteOffline;

                case SituacaoFiscal.AutorizadaDenegada:
                    return Status.Transmitida;

                case SituacaoFiscal.Rejeicao:
                    return Status.Aberta;

                default:
                    throw new ArgumentOutOfRangeException(nameof(situacaoFiscal), situacaoFiscal, null);
            }

            throw new ArgumentNullException("Status não existente");
        }
    }
}