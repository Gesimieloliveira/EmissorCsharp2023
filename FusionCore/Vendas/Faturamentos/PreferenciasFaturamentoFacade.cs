using FusionCore.Helpers.Maquina;
using FusionCore.Sessao;
using FusionCore.Vendas.Repositorio;

namespace FusionCore.Vendas.Faturamentos
{
    public class PreferenciasFaturamentoFacade
    {
        private readonly ISessaoManager _sessaoManager;

        public PreferenciasFaturamentoFacade(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public bool PossuiPreferenciaParaMaquina()
        {
            var mkSha1 = IdMaquinaProvider.Computa();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPreferencia(sessao);
                return repositorio.PossuiPreferenciaParaMaquina(mkSha1);
            }
        }

        public FaturamentoPreferencia GetPreferenciaDaMaquina()
        {
            var mkSha1 = IdMaquinaProvider.Computa();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPreferencia(sessao);
                var preferencia = repositorio.GetPeloIdentificadorMaquina(mkSha1);

                return preferencia;
            }
        }

        public void Salva(FaturamentoPreferencia faturamentoPreferencia)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                new RepositorioPreferencia(sessao).Salva(faturamentoPreferencia);
            }
        }
    }
}