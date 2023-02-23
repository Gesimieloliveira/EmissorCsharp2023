using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionCore.Excecoes.Sessao;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Setup.Conexao;
using FusionCore.Helpers.Exe;
using FusionCore.MigracaoFluente;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionCore.Setup;
using FusionLibrary.VisaoModel;
using FusionNfce.Visao.Conexao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Configuracao;
using IMigracao = FusionCore.MigracaoFluente.IMigracao;

namespace FusionNfce.Visao.Splash
{
    public sealed class SplashNfceModel : ViewModel
    {
        private IMigracao _migrador;
        private bool _ativaBotaoConfigurarLicenca;

        public string TextoNotificacao
        {
            get => GetValue() ?? "Aguarde inicializando...";
            set => SetValue(value);
        }

        public bool AtivaBotaoConexao
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

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

        public bool AtivaBotaoAtivarLicenca
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand ConfiguraConexaoCommand => GetSimpleCommand(ConfiguraConexaoAction);
        public ICommand FecharCommand => GetSimpleCommand(FecharAction);
        public ICommand AtualizarVersaoCommand => GetSimpleCommand(AtualizarVersaoAction);
        public ICommand AtivarConfigurarLicencaCommand => GetSimpleCommand(ConfigurarServidorLicencaAction);

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

        private void ConfigurarServidorLicencaAction(object obj)
        {
            var dialog = new ConexaoServidorLicenca();
            dialog.ShowDialog();

            InicializaSistemaAsync();
        }

        public event EventHandler ConcluiuCarregamento;

        private void OnConcluiuCarregamento()
        {
            TextoNotificacao = "Tudo pronto para iniciar!";

            Application.Current.Dispatcher.Invoke(() =>
            {
                ConcluiuCarregamento?.Invoke(this, EventArgs.Empty);
            });
        }

        public Task InicializaSistemaAsync()
        {
            return Task.Factory.StartNew(InicializaSystema);
        }

        private void InicializaSystema()
        {
            try
            {
                AtivaProgresso();
                InicializarLicenciamento();
            }
            catch (Exception e)
            {
                TextoNotificacao = $"Erro: {e.Message}";
            }
            finally
            {
                AtivaBotaoFechar = true;
                AtivaProgressBar = false;
            }
        }

        private void AtivaProgresso()
        {
            AtivaProgressBar = true;
            AtivaBotaoConexao = false;
            AtivaBotaoAtualizaDados = false;
            AtivaBotaoFechar = false;
            AtivaBotaoAtivarLicenca = false;
            AtivaBotaoConfigurarLicenca = false;
        }

        private void InicializarLicenciamento()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou checando o licenciamento";

            try
            {
                if (SessaoSistemaNfce.AcessoConcedido == null)
                {
                    ArquivoConexaoLicenciamento.CriaArquivo();

                    var solicitacao = SolicitacaoUso.Factory(TipoSistema.FusionNFCE);

                    //TODO 1612 - LICENCIAMENTO: Concede acesso aos módulos
                    SessaoSistemaNfce.AcessoConcedido = new AcessoConcedido(solicitacao)
                    {
                        PossuiFusionCteOs = true,
                        PossuiFusionGestor = true,
                        PossuiFusionMdfe = true,
                        PossuiFusionStarter = true,
                        UltimaChecagem = DateTime.Now,
                        PossuiFusionCTe = true
                    };
                }
            }
            catch (Exception e)
            {
                TextoNotificacao = e.Message;
                AtivaBotaoConfigurarLicenca = true;
                return;
            }

            InicializarBancoDeDados();
        }

        private void InicializarBancoDeDados()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou checando o banco de dados";

            var manipulador = new ManipulaConexao();

            if (!manipulador.ArquivoExiste())
            {
                TextoNotificacao = "A conexão ainda não está configurada. Vamos configura-la agora?";
                AtivaBotaoConexao = true;
                return;
            }

            var conexao = manipulador.LerArquivo();
            var nfce = conexao.ConexaoNfce.ToCfg();
            var databaseUtilitiy = new DatabaseUtility(nfce);

            var paths = new AutoRestorePath();

            var teste = databaseUtilitiy.TesteConexao();

            if (!teste.IsValido)
            {
                TextoNotificacao = "Falha ao testar conexao com Banco Dados: " + teste.DetalheFalha;
                AtivaBotaoConexao = true;
                return;
            }

            try
            {
                if (!databaseUtilitiy.DatabaseExiste(nfce.BancoDados))
                {
                    TextoNotificacao = "Restaurando banco de dados FusionNfce";

                    var autoRestore = new DatabaseAutoRestore(databaseUtilitiy, paths);
                    autoRestore.RestaurarNfce(nfce.BancoDados);

                    Thread.Sleep(4000);
                }
            }
            catch (Exception e)
            {
                TextoNotificacao = e.Message;
                AtivaBotaoConexao = true;
                return;
            }

            try
            {
                GerenciaSessaoNfce.GerenciaSessaoNfceInicializa();
            }
            catch (ConexaoInvalidaException e)
            {
                TextoNotificacao = "Falha conectar Banco Dados: " + e.Message;
                AtivaBotaoConexao = true;
                return;
            }

            try
            {
                GerenciaSessaoNfce.GerenciaSessaoInicializaTodasConexoes();
            }
            catch (Exception e)
            {
                //ignore
            }


            VerificarVersaoBancoDados();
        }

        private void VerificarVersaoBancoDados()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou verificando a versão do sistema";

            MigracaoFacade.ThrowExcepionSeVersaoAntiga(SessaoSistemaNfce.SessaoManager);

            try
            {
                _migrador = MigracaoFactory.CriaMigrador(SessaoNfce.Conexao, MigracaoTag.Nfce);

                if (_migrador.PrecisaAtualizar)
                {
                    TextoNotificacao = "Humm, preciso que atualize o banco de dados";
                    AtivaBotaoAtualizaDados = true;
                    return;
                }
            }
            catch (Exception e)
            {
                TextoNotificacao = "Erro ao verificar versão do BD: " + e.Message;
                return;
            }

            OnConcluiuCarregamento();
        }

        private void ConfiguraConexaoAction(object obj)
        {
            new ConexaoForm().ShowDialog();
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
                TextoNotificacao = "Atualizando banco de dados";
                AtivaProgresso();

                try
                {
                    _migrador.Migracao();
                }
                catch (Exception e)
                {
                    Application.Current.Dispatcher.Invoke(() => { DialogBox.MostraErro("Falha ao atualizar o banco de dados", e); });

                    return;
                }

                InicializaSistemaAsync();
            });
        }
    }
}