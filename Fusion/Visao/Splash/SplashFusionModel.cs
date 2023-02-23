using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fusion.Factories;
using Fusion.Licenciamento;
using Fusion.Sessao;
using Fusion.Visao.Licenciamento;
using FusionCore.Debug;
using FusionCore.Excecoes.Sessao;
using FusionCore.FusionAdm.Csrt;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Exe;
using FusionCore.Helpers.Log;
using FusionCore.MigracaoFluente;
using FusionCore.Relatorios;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Cfop;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionCore.Setup;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Configuracao;
using log4net;

namespace Fusion.Visao.Splash
{
    public sealed class SplashFusionModel : ViewModel
    {
        private readonly ILog _log = FusionLog.GetLogger(MethodBase.GetCurrentMethod());
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly ConfiguradorConexao _configuradorConexao = new ConfiguradorConexao();
        private bool _ativaBotaoConfigurarLicenca;
        private IMigracao _migradorAdm;
        private IMigracao _migradorRelatorios;

        public string TextoNotificacao
        {
            get => GetValue() ?? "Aguarde inicializando...";
            set => SetValue(value);
        }

        public bool AtivaBotaoConexao
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);

#if DEBUG
                PropriedadeAlterada(nameof(BotaoConexaoDevelopIsVisible));
#endif
            }
        }

        public bool BotaoConexaoDevelopIsVisible
            => BuildMode.IsHomologacao && AtivaBotaoConexao;

        public bool AtivaBotaoFechar
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool AtivaProgressBar
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool AtivaBotaoAtualizaDados
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool AtivaBotaoPainelLicencas
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool AtivaBotaoConfigurarLicenca
        {
            get => _ativaBotaoConfigurarLicenca;
            set
            {
                if (value == _ativaBotaoConfigurarLicenca) return;
                _ativaBotaoConfigurarLicenca = value;
                PropriedadeAlterada();
            }
        }

        public bool AtivaBotaoRevalidarLicenca
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand ConfiguraConexaoCommand => GetSimpleCommand(ConfiguraConexaoAction);
        public ICommand FecharCommand => GetSimpleCommand(FecharAction);
        public ICommand AtualizarVersaoCommand => GetSimpleCommand(AtualizarVersaoAction);
        public ICommand LicenciamentoCommand => GetSimpleCommand(LicenciamentoAction);
        public ICommand AtivarConfigurarLicencaCommand => GetSimpleCommand(ConfigurarServidorLicencaAction);
        public ICommand RevalidarLicencaCommand => GetSimpleCommand(RevalidarLicencaAction);
        public event EventHandler ConcluiuCarregamento;

        private void OnConcluiuCarregamento()
        {
            ChecadorLicenca.Instancia.IniciarChecagem(_sessaoSistema.AcessoConcedido);
            TextoNotificacao = "Tudo pronto para iniciar!";

            Application.Current.Dispatcher.Invoke(() => { ConcluiuCarregamento?.Invoke(this, EventArgs.Empty); });
        }

        public async Task InicializaSistemaAsync()
        {
            await Task.Run(() => InicializaSistema());
        }

        private void InicializaSistema()
        {
            AtivarAguarde();
            IniciaFluxoDeChecagem();
        }

        private void AtivarAguarde()
        {
            AtivaProgressBar = true;
            AtivaBotaoConexao = false;
            AtivaBotaoAtualizaDados = false;
            AtivaBotaoFechar = false;
            AtivaBotaoPainelLicencas = false;
            AtivaBotaoConfigurarLicenca = false;
            AtivaBotaoRevalidarLicenca = false;
        }

        private void IniciaFluxoDeChecagem()
        {
            try
            {
                AlocarLicencaParaUso();
            }
            catch (ConexaoInvalidaException e)
            {
                TextoNotificacao = e.Message;
                DialogBox.MostraErro(e.Message, e);
            }
            catch (Exception e)
            {
                TextoNotificacao = e.Message;
            }
            finally
            {
                AtivaBotaoFechar = true;
                AtivaProgressBar = false;
            }
        }

        private void AlocarLicencaParaUso()
        {
            _log.Info("Iniciando alocação da licença...");

            AtivaProgressBar = true;
            TextoNotificacao = "Alocando licença para uso...";

            try
            {
                // TODO 1612 - LICENCIAMENTO: Concede acesso aos módulos

                if (_sessaoSistema.AcessoConcedido == null)
                {
                    ArquivoConexaoLicenciamento.CriaArquivo();

                    var solicitacao = SolicitacaoUso.Factory(TipoSistema.FusionAdm);

                    _sessaoSistema.AcessoConcedido = new AcessoConcedido(solicitacao)
                    {
                        PossuiFusionGestor = true,
                        PossuiFusionStarter = true,
                        PossuiFusionCTe = true,
                        PossuiFusionCteOs = true,
                        PossuiFusionMdfe = true,
                        UltimaChecagem = DateTime.Now
                    };
                }
            }
            catch (Exception e)
            {
                _log.Error(e);

                TextoNotificacao = $"Não consegui conectar no Servidor de Licenças! / Detalhes: {e.Message}";
                AtivaBotaoConfigurarLicenca = true;
                return;
            }

            InicializarConexaoBancoDados();
        }

        private void InicializarConexaoBancoDados()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou conectando com o banco de dados...";

            if (!_configuradorConexao.ArquivoExiste())
            {
                TextoNotificacao = "Preciso que configure a conexão com o banco de dados!";
                AtivaBotaoConexao = true;
                return;
            }

            var conexao = _configuradorConexao.LerArquivo();
            var databaseUtility = new DatabaseUtility(conexao.ToCfg());
            var testeConexao = databaseUtility.TesteConexao();

            if (!testeConexao.IsValido)
            {
                TextoNotificacao = $"Problema de conexão: {testeConexao.DetalheFalha}";
                AtivaBotaoConexao = true;
                return;
            }

            var paths = new AutoRestorePath();

            if (!databaseUtility.DatabaseExiste(conexao.BancoDados))
            {
                var autoRestore = new DatabaseAutoRestore(databaseUtility, paths);

                try
                {
                    TextoNotificacao = "Restaurando banco de dados FusionAdm...";
                    autoRestore.RestaurarAdm(conexao.BancoDados);
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                    AtivaBotaoConexao = true;
                    throw;
                }
            }

            if (!databaseUtility.DatabaseExiste("FusionRelatorio"))
            {
                var autoRestore = new DatabaseAutoRestore(databaseUtility, paths);
                autoRestore.RestaurarRelatorio("FusionRelatorio");
            }

            TestarConfiguracoesBancoDados();
        }

        private void TestarConfiguracoesBancoDados()
        {
            try
            {
                using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
                {
                    sessao.Transaction.Begin(IsolationLevel.ReadCommitted);
                }
            }
            catch (Exception e)
            {
                throw new ConexaoInvalidaException("Não foi possível conectar no banco de dados!", e);
            }

            VerficaVersaoBancoDados();
        }

        private void VerficaVersaoBancoDados()
        {
            TextoNotificacao = "Estou verificando a versão do banco de dados...";

            MigracaoFacade.ThrowExcepionSeVersaoAntiga(_sessaoSistema.SessaoManager);

            var cfgConexaoAdm = SessaoHelperFactory.GetConexaoCfg();

            _migradorAdm = MigracaoFactory.CriaMigrador(cfgConexaoAdm, MigracaoTag.Adm);
            _migradorRelatorios = MigracaoFactory.CriaMigrador(CriaIConexaoCfg.CriaIConexaoCfgRelatorio(cfgConexaoAdm), MigracaoTag.Relatorio);

            if (_migradorAdm.PrecisaAtualizar || _migradorRelatorios.PrecisaAtualizar)
            {
                TextoNotificacao = "Seu banco de dados está desatualizado. Clique em atualizar para continuar! :)";
                AtivaBotaoAtualizaDados = true;
                AtivaBotaoConexao = true;
                return;
            }

            VerificarSeTemResponsavelTecnico();
        }

        private void VerificarSeTemResponsavelTecnico()
        {
            try
            {
                var responsaveisTecnicos = new ObterCsrf().LerResponsaveisTecnicos();

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioResponsavelTecnico(sessao);

                    if (responsaveisTecnicos.Count == 0)
                    {
                        var repositorioTerminalOffline = new RepositorioTerminalOffline(sessao);
                        var terminaisDesligado = repositorioTerminalOffline.TodosTerminaisSomenteComId();
                        var repositorioSincronizacaoPendente = new RepositorioSincronizacaoPendente(sessao);

                        foreach (var terminalOffline in terminaisDesligado)
                        {
                            repositorioSincronizacaoPendente.AdicionaResponsavelTecnico(terminalOffline);
                        }
                    }

                    repositorio.DeletarTudo();
                    foreach (var responsavelTecnico in responsaveisTecnicos)
                    {
                        repositorio.Persistir(ResponsavelTecnico.Instancia(responsavelTecnico, sessao));
                    }

                    transacao.Commit();
                }
            }
            catch (CsrtException e)
            {
                _log.Error(e);
            }

            ConcluirCarregamento();
        }

        private void ConfiguraConexaoAction(object obj)
        {
            var contexto = new ConfigurarConexaoViewModel(_configuradorConexao);

            var view = new ConfigurarConexaoView(contexto);
            var viewResult = view.ShowDialog();

            if (viewResult != true)
            {
                return;
            }

            InicializaSistemaAsync();
        }

        private static void FecharAction(object obj)
        {
            Application.Current.Shutdown();
        }

        private async void AtualizarVersaoAction(object obj)
        {
            var resultado = DialogBox.MostraConfirmacao("Deseja atualizar a base de dados?");

            if (resultado != MessageBoxResult.Yes)
            {
                return;
            }

            await Task.Run(() =>
            {
                try
                {
                    TextoNotificacao = "Atualizando banco de dados...";
                    AtivarAguarde();

                    _migradorAdm.Migracao();
                    _migradorRelatorios.Migracao();
                }
                catch (Exception e)
                {
                    TextoNotificacao = "Falha ao atualizar o banco de dados!";
                    AtivaBotaoFechar = true;

                    Application.Current.Dispatcher.Invoke(() => { DialogBox.MostraErro(TextoNotificacao, e); });

                    return;
                }

                InicializaSistemaAsync();
            });
        }

        private void LicenciamentoAction(object obj)
        {
            new PainelLicencas().ShowDialog();
            InicializaSistemaAsync();
        }

        private void ConfigurarServidorLicencaAction(object obj)
        {
            new ConexaoServidorLicenca().ShowDialog();
            InicializaSistemaAsync();
        }

        private void RevalidarLicencaAction(object obj)
        {
            // TODO 1612 - LICENCIAMENTO: Revalidação de licenca
        }

        private void ConcluirCarregamento()
        {
            OnConcluiuCarregamento();
        }

        public void UsarMesmaConexaoDoDevelop()
        {
            var diretorioConfig = DiretorioAssembly.GetPastaConfig();
            var diretorioDevelop = diretorioConfig.Replace($"{FusionProjetoHelper.NomeBranch}", "develop");
            var conexaoDevelop = Path.Combine(diretorioDevelop, "Conexao.xml");

            if (!File.Exists(conexaoDevelop))
            {
                throw new InvalidOperationException($"Conexão para o branch develop não esiste em: {conexaoDevelop}");
            }

            File.Copy(conexaoDevelop, Path.Combine(diretorioConfig, "Conexao.xml"), true);
        }
    }
}