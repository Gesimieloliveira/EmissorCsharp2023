using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.NfceSincronizador.Financeiro
{
    public class ServicoReceberTipoDocumento
    {
        private readonly SessaoSyncFactory _sessaoFactory;

        public ServicoReceberTipoDocumento(SessaoSyncFactory sessaoFactory)
        {
            _sessaoFactory = sessaoFactory;
        }

        public void ReceberDados()
        {
            using (var sessaoServidor = _sessaoFactory.CriarSessaoServidor())
            using (var sessaoLocal = _sessaoFactory.CriarSessaoLocal())
            {
                var terminalId = ConfiguracaoTerminalFacade.ObtemNumeroTerminal(sessaoLocal);
                var repositorioSincronizacaoPendente = new RepositorioSincronizacaoPendente(sessaoServidor);

                var pedentes = repositorioSincronizacaoPendente.BuscaTodosParaSincronizacao(EntidadeSincronizavel.TipoDocumento, terminalId);

                foreach (var itemPendente in pedentes)
                {
                    var registro = sessaoServidor.Get<TipoDocumento>(short.Parse(itemPendente.Referencia));

                    sessaoLocal.SaveOrUpdate(registro);
                    sessaoLocal.Flush();

                    sessaoServidor.Delete(itemPendente);
                    sessaoServidor.Flush();
                }
            }
        }
    }
}