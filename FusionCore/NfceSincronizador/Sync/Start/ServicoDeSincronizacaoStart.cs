using System;
using FusionCore.Excecoes.Sessao;
using FusionCore.Extencoes;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Helpers.Log;
using FusionCore.NfceSincronizador.ControleAutorizacao;
using FusionCore.NfceSincronizador.ControleCaixa;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Base.Helper;

namespace FusionCore.NfceSincronizador.Sync.Start
{
    public static class ServicoDeSincronizacaoStart
    {
        private static readonly IRegistrarLog Log;

        static ServicoDeSincronizacaoStart()
        {
            Log = RegistarLog.Istancia;
        }

        public static void CriaConexoes()
        {
            TrataException(GerenciaSessaoNfce.GerenciaSessaoInicializaTodasConexoes);
        }

        public static void Sincronizar()
        {
            TrataException(ExecutaSincronizacao);
        }

        private static void ExecutaSincronizacao()
        {
            EntidadeSincronizavel.EstadoUf.Sincronizar();
            EntidadeSincronizavel.Cidade.Sincronizar();
            EntidadeSincronizavel.Pessoa.Sincronizar();
            EntidadeSincronizavel.Usuario.Sincronizar();
            EntidadeSincronizavel.Empresa.Sincronizar();
            EntidadeSincronizavel.Cfop.Sincronizar();
            EntidadeSincronizavel.RegraTributacaoSaida.Sincronizar();
            EntidadeSincronizavel.ProdutoUnidade.Sincronizar();
            EntidadeSincronizavel.Produto.Sincronizar();
            EntidadeSincronizavel.ConfiguracaoEmail.Sincronizar();
            EntidadeSincronizavel.Nfce.Sincronizar();
            EntidadeSincronizavel.EmissorFiscalNfce.Sincronizar();
            EntidadeSincronizavel.TerminalOffline.Sincronizar();
            EntidadeSincronizavel.ProdutoEstoqueEvento.Sincronizar();
            EntidadeSincronizavel.Ibpt.Sincronizar();
            EntidadeSincronizavel.TipoDocumento.Sincronizar();
            EntidadeSincronizavel.ConfiguracaoFrenteCaixa.Sincronizar();
            EntidadeSincronizavel.ConfiguracaoEstoque.Sincronizar();
            EntidadeSincronizavel.Pos.Sincronizar();
            EntidadeSincronizavel.Inutilizacao.Sincronizar();
            EntidadeSincronizavel.Balanca.Sincronizar();
            EntidadeSincronizavel.ResponsavelTecnico.Sincronizar();
            EntidadeSincronizavel.Papel.Sincronizar();
            EntidadeSincronizavel.TabelaPreco.Sincronizar();

            SincronizarConfiguracoes();
            SincronizarCaixa();
            SincronizarEventosOperacaoAutorizada();
        }

        public static void TrataException(Action executaAcao)
        {
            try
            {
                executaAcao.Invoke();
            }
            catch (ConexaoInvalidaException ex)
            {
                RegistrarLog(ex);
                ThrowException(ex);
            }
            catch (InvalidOperationException ex)
            {
                RegistrarLog(ex);
                ThrowException(ex);
            }
            catch (RepositorioExeption ex)
            {
                RegistrarLog(ex);
                ThrowException(ex);
            }
            catch (SessaoHelperException ex)
            {
                RegistrarLog(ex);
                ThrowException(ex);
            }
            catch (Exception ex)
            {
                RegistrarLog(ex);
                ThrowException(ex);
            }
        }

        private static void ThrowException(Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

        private static void RegistrarLog(Exception ex)
        {
            Log.RegistrarException(ex);
        }

        private static void SincronizarCaixa()
        {
            var servico = new ServicoEnviarDadosDoCaixaParaServidor(new SessaoSyncFactory());

            servico.EnviarLancamentosAvulsos();
            servico.EnviarCaixasIndividuais();
        }

        private static void SincronizarConfiguracoes()
        {
            new ServicoReceberConfiguracaoCaixa(new SessaoSyncFactory())
                .ReceberConfiguracao();
        }

        private static void SincronizarEventosOperacaoAutorizada()
        {
            new ServicoEnviarEventoOperacaoAutorizadaParaServidor(new SessaoSyncFactory())
                .EnviarEventos();
        }
    }
}