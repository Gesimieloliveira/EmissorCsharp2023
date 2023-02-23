using FusionCore.FusionAdm.PedidoVenda.Repositorio;
using FusionCore.Helpers.Maquina;
using FusionCore.Impressoras;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.PedidoVenda.Preferencias
{
    public class PedidoVendaPreferenciaFacade
    {
        private readonly ISessaoManager _sessaoManager;

        public PedidoVendaPreferenciaFacade()
        {
            _sessaoManager = new SessaoManagerAdm();
        }

        public bool PossuiPreferenciaParaMaquina()
        {
            var mkSha1 = IdMaquinaProvider.Computa();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPedidoVendaPreferencia(sessao);
                return repositorio.PossuiPreferenciaParaMaquina(mkSha1);
            }
        }

        public PedidoVendaPreferencia GetPreferenciaDaMaquina()
        {
            var mkSha1 = IdMaquinaProvider.Computa();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPedidoVendaPreferencia(sessao);
                var preferencia = repositorio.GetPeloIdentificadorMaquina(mkSha1);

                return preferencia;
            }
        }

        public void Salvar(PedidoVendaPreferencia faturamentoPreferencia)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                new RepositorioPedidoVendaPreferencia(sessao).Salvar(faturamentoPreferencia);

                transacao.Commit();
            }
        }

        public void SalvarPadrao()
        {
            var impressoraPadrao = Impressora.Padrao;
            Salvar(new PedidoVendaPreferencia(IdMaquinaProvider.Computa())
            {
                Impressora = impressoraPadrao
            });
        }
    }
}