using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Fiscal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtCancelamentoAdm
    {
        public static NfceCancelamentoAdm ToAdm(this NfceCancelamento cancelamento, NfceAdm nfceAdm)
        {
            if (cancelamento == null) return null;

            var cancelamentoAdm = new NfceCancelamentoAdm
            {
                Nfce = nfceAdm,
                Justificativa = cancelamento.Justificativa,
                Ambiente = cancelamento.Ambiente,
                DocumentoUnico = cancelamento.DocumentoUnico,
                Protocolo = cancelamento.Protocolo,
                Chave = cancelamento.Chave,
                OcorreuEm = cancelamento.OcorreuEm,
                TipoEvento = cancelamento.TipoEvento,
                CodigoUf = cancelamento.CodigoUf,
                NfceId = 0,
                StatusRetorno = cancelamento.StatusRetorno,
                VersaoAplicacao = cancelamento.VersaoAplicacao
            };


            return cancelamentoAdm;
        }
    }
}