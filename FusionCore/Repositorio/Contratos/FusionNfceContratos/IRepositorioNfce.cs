using System.Collections.Generic;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Pagamento;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioNfce : IRepositorio<Nfce, int>
    {
        void Salvar(Nfce nfce);
        void SalvarNfceEnvioLote(NfceEnvioLote envioLote);
        void SalvarItemESincronizar(NfceItem item);
        void SalvarItem(NfceItem item);
        void SalvarEmissao(NfceEmissao emissao);
        void SalvarDestinatario(NfceDestinatario destinatario);
        void SalvarEmitente(NfceEmitente emitente);
        void SalvarCancelamento(NfceCancelamento cancelamento);
        void SalvarPagamento(FormaPagamentoNfce formaPagamentoNfce);
        IList<Nfce> NfceEmitidaOffline();
        IList<Nfce> BuscaNfceOfflineComErrosNaEmissao();
        void SalvarHistorico(NfceEmissaoHistorico emissaoHistoricoFinalizado);
        int QuantidadeDeNFCeOffiline();
        NfceEmissaoHistorico BuscarHistoricoPelaChaveDeAcesso(string chaveAcesso);
        IEnumerable<NfceEmissaoHistorico> BuscarHistoricoEmissao(Nfce nfce);
        void SalvarESincronizar(Nfce nfce);
        string ObterXmlAutorizado(int nfceId);
        string BuscarUltimoXmlAssinado(int nfceId);
    }
}