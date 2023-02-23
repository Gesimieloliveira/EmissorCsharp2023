using System.Data;
using FusionCore.Configuracoes;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.NfceSincronizador.ControleCaixa
{
    public class ServicoReceberConfiguracaoCaixa
    {
        private readonly SessaoSyncFactory _sessaoFactory;

        public ServicoReceberConfiguracaoCaixa(SessaoSyncFactory sessaoFactory)
        {
            _sessaoFactory = sessaoFactory;
        }

        public void ReceberConfiguracao()
        {
            using (var sessaoServidor = _sessaoFactory.CriarSessaoServidor(IsolationLevel.ReadCommitted))
            using (var sessaoLocal = _sessaoFactory.CriarSessaoLocal(IsolationLevel.ReadCommitted))
            {
                var terminalId = ConfiguracaoTerminalFacade.ObtemNumeroTerminal(sessaoLocal);
                var repositorio = new RepositorioSincronizacaoPendente(sessaoServidor);

                var pendentes = repositorio.BuscaTodosParaSincronizacao(EntidadeSincronizavel.ConfiguracaoCaixa, terminalId);

                foreach (var p in pendentes)
                {
                    var unicaCfg = sessaoServidor.QueryOver<ConfiguracaoControleDeCaixa>().SingleOrDefault();

                    sessaoLocal.SaveOrUpdate(unicaCfg);
                    sessaoLocal.Flush();

                    sessaoServidor.Delete(p);
                    sessaoServidor.Flush();
                }

                sessaoLocal.Transaction.Commit();
                sessaoServidor.Transaction.Commit();
            }
        }
    }
}