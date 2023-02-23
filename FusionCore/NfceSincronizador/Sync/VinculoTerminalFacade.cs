using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;

namespace FusionCore.NfceSincronizador.Sync
{
    public static class VinculoTerminalFacade
    {
        public static bool SemViculoComServidor()
        {
            using (var sessaonfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorioConfiguracao = new RepositorioConfiguracaoTerminalNfce(sessaonfce);
                var cfg = repositorioConfiguracao.GetPeloId(1);

                if (cfg == null) return true;

                using (var sessaoServer = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao())
                {
                    var repositorioTerminal = new RepositorioTerminalOffline(sessaoServer);
                    var terminal = repositorioTerminal.ConfiguracaoNfce(cfg.BindTerminal);

                    return terminal == null;
                }
            }
        }
    }
}