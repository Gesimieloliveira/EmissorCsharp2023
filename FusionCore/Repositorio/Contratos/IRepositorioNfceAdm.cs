using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Nfce.SatFiscal;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioNfceAdm : IRepositorio<NfceAdm, int>
    {
        void Salvar(NfceAdm nfce);
        void SalvarItem(NfceItemAdm itemAdm);
        void SalvarEmissao(NfceEmissaoAdm emissaoAdm);
        void SalvarDestinatario(NfceDestinatarioAdm nfceDestinatarioAdm);
        void SalvarEmitente(NfceEmitenteAdm emitenteAdm);
        void SalvarCancelamento(NfceCancelamentoAdm cancelamentoAdm);
        void SalvarFormaPagamento(FormaPagamentoNfceAdm pagamentoAdm);
        void SalvarNfceHistoricoAdm(NfceEmissaoHistoricoAdm historicoAdm);
        NfceItemAdm BuscarItemPorId(int itemNfceIdRemoto);
        NfceEmissaoAdm BuscarEmissaoPeloId(int idRemotoEmissao);
        NfceEmitenteAdm BuscaEmitentePeloId(int idRemotoEmitente);
        NfceDestinatarioAdm BuscarDestinatarioPeloId(int destinatarioIdRemoto);
        NfceCancelamentoAdm BuscarCancelamentoPeloId(int cancelamentoIdRemoto);
        CancelamentoSatAdm BuscaCancelamentoSatPeloId(int cancelamentoSatIdRemoto);
        string BaixarXmlAutorizado(int nfceId);
        string UltimoXmlAssinado(int nfceId);
    }
}