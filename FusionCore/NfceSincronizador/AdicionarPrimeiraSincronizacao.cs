using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.NfceSincronizador
{
    public class AdicionarPrimeiraSincronizacao
    {
        private readonly RepositorioSincronizacaoPendente _repositorioSincronizacaoPendente;

        public AdicionarPrimeiraSincronizacao(RepositorioSincronizacaoPendente repositorioSincronizacaoPendente)
        {
            _repositorioSincronizacaoPendente = repositorioSincronizacaoPendente;
        }

        public void Executar(byte idTerminal)
        {
            _repositorioSincronizacaoPendente.AdicionaTodosUsuariosNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodosEstadosUfNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodasCidadesNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodasEmpresasNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodosCfopDaNfceNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodosEmissorFiscalDaNfceNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodasUnidadesMedidasProdutoDaNfceNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaRegraTributacaoSaida(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodosProdutoDaNfceNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodasPessoasDaNfceNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaConfiguracaoDeEmailNaNfceNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaIbptNaNfceNaPrimeiraSync(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTipoDocumento(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaConfiguracaoFrenteCaixa(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaConfiguracaoEstoque(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaConfiguracaoBalanca(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaResponsavelTecnico(idTerminal);
            _repositorioSincronizacaoPendente.AdicionaTodasTabelasDePrecoNaPrimeiraSync(idTerminal);
        }
    }
}