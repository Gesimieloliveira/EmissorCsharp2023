using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionCore.Excecoes.Sessao;
using FusionCore.FusionPdv.ModeloEcf;
using FusionCore.FusionPdv.Sessao;
using FusionCore.FusionPdv.Setup.BD;
using FusionCore.Helpers.Exe;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionCore.Setup;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionPdv.Acbr;
using FusionPdv.Acbr.Paf;
using FusionPdv.Ecf;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Visao.ConexaoBancoDados;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Configuracao;
using ConfiguracaoInicialView = FusionPdv.Visao.ConfiguracaoInicial.ConfiguracaoInicial;

namespace FusionPdv.Visao.Splash
{
    public sealed class SplashPdvModel : ViewModel
    {
        private bool _ativaBotaoConfigurarLicenca;

        public string TextoNotificacao
        {
            get { return GetValue() ?? "Aguarde inicializando..."; }
            set { SetValue(value); }
        }

        public bool AtivaBotaoConexao
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool AtivaBotaoFechar
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool AtivaProgressBar
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool AtivaBotaoAtualizaDados
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool AtivaBotaoEcf
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public ICommand ConfiguraConexaoCommand => GetSimpleCommand(ConfiguraConexaoAction);
        public ICommand FecharCommand => GetSimpleCommand(FecharAction);
        public ICommand AtualizarVersaoCommand => GetSimpleCommand(AtualizarVersaoAction);
        public ICommand ConfiguracaoEcfCommand => GetSimpleCommand(ConfiguracaoEcfAction);
        public ICommand AtivarConfigurarLicencaCommand => GetSimpleCommand(ConfigurarServidorLicencaAction);

        public bool AtivaBotaoConfigurarLicenca
        {
            get { return _ativaBotaoConfigurarLicenca; }
            set
            {
                if (value == _ativaBotaoConfigurarLicenca) return;
                _ativaBotaoConfigurarLicenca = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler ConcluiuCarregamento;

        private void OnConcluiuCarregamento()
        {
            TextoNotificacao = "Tudo pronto para iniciar!";
            Application.Current.Dispatcher.Invoke(() => { ConcluiuCarregamento?.Invoke(this, EventArgs.Empty); });
        }

        public Task InicializaSistemaAsync()
        {
            return Task.Run(() => InicializaSystema());
        }

        private void InicializaSystema()
        {
            AtivaBotaoConexao = false;
            AtivaBotaoAtualizaDados = false;
            AtivaBotaoFechar = false;
            AtivaBotaoEcf = false;
            AtivaBotaoConfigurarLicenca = false;

            AtivaProgressBar = true;
            TextoNotificacao = "Estou preparando o Fusion para você!";

            try
            {
                IniciarLicenciamento();
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

        private void IniciarLicenciamento()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou alocando um acesso para o PDV.";

            try
            {
                ArquivoConexaoLicenciamento.CriaArquivo();

                var solicitacao = SolicitacaoUso.Factory(TipoSistema.FusionPdv);

                // TODO 1612 - LICENCIAMENTO: Concede acesso aos modulos
                SessaoSistema.AcessoConcedido = new AcessoConcedido(solicitacao)
                {
                    PossuiFusionCTe = true,
                    PossuiFusionCteOs = true,
                    PossuiFusionGestor = true,
                    PossuiFusionMdfe = true,
                    PossuiFusionStarter = true,
                    UltimaChecagem = DateTime.Now
                };
            }
            catch (Exception e)
            {
                TextoNotificacao = e.Message;
                AtivaBotaoConfigurarLicenca = true;
                return;
            }

            ChecarConfiguracaoBD();
        }

        private void ChecarConfiguracaoBD()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou checando as configurações de Conexão";

            var setup = new ConexaoSetup();

            if (!setup.ExisteArquivoConexao)
            {
                TextoNotificacao = "Percebi que ainda não configurou a conexão. Vamos configurar?";
                AtivaBotaoConexao = true;
                return;
            }

            var conexao = setup.LerArquivoConexao();
            var cfg = conexao.ConexaoPdv.ToCfg();

            var databaseUtility = new DatabaseUtility(cfg);

            var teste = databaseUtility.TesteConexao();

            if (!teste.IsValido)
            {
                TextoNotificacao = "Falha ao conectar com SQL: " + teste.DetalheFalha;
                AtivaBotaoConexao = true;
                return;
            }

            try
            {
                if (!databaseUtility.DatabaseExiste(cfg.BancoDados))
                {
                    TextoNotificacao = "Restaurando banco de dados FusionPdv";

                    var autoRestore = new DatabaseAutoRestore(databaseUtility, new AutoRestorePath());
                    autoRestore.RestaurarPdv(cfg.BancoDados);
                    Thread.Sleep(4000);
                }
            }
            catch (Exception e)
            {
                TextoNotificacao = e.Message;
                AtivaBotaoConexao = true;
                return;
            }

            InicialzarConexoes();
        }

        private void InicialzarConexoes()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou testando a configuração de conexão com o Banco de Dados";

            try
            {
                GerenciaSessao.GerenciaSessaoInicializar();
            }
            catch (Exception e)
            {
                TextoNotificacao = "Ops. Ocorreu um erro de conexão: " + e.Message;
                AtivaBotaoConexao = true;
                return;
            }

            VerificarVersaoBancoDados();
        }

        private void VerificarVersaoBancoDados()
        {
            PrepararAcbr();
        }

        private void PrepararAcbr()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Quase lá! Preparando componentes de comunicação";

            try
            {
                AcbrFactory.ObterAcbrEcf();
                AcbrFactory.ObterAcbrPaf();
            }
            catch (Exception e)
            {
                TextoNotificacao = "Erro ao carregar componentes: " + e.Message;
                return;
            }

            GerarArquivoAuxiliar();
        }

        private void GerarArquivoAuxiliar()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Estou checando o Arquivo Auxiliar obrigatório pelo PAF-ECF";

            try
            {
                if (!CriadorArquivoAuxiliar.ArquivoExiste)
                {
                    var ecf = new ObterEcfEmUso().Buscar();

                    if (ecf != null)
                    {
                        ecf.EmUso = 0;

                        using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                        {
                            var repositorio = new EcfRepositorio(sessao);
                            repositorio.Salvar(ecf);
                        }
                    }

                    TextoNotificacao = "Arquivo Auxiliar não existe. Configure-o para continuarmos!";
                    AtivaBotaoEcf = true;
                    return;
                }
            }
            catch (Exception e)
            {
                TextoNotificacao = "Erro ao checar Arquivo Auxiliar:" + e.Message;
                return;
            }

            GerarArquivoPafMd5();
        }

        private void GerarArquivoPafMd5()
        {
            TextoNotificacao = "Mais um pouco! Estou gerando o MD5 obrigatório pelo PAF-ECF";

            try
            {
                var md5 = new GerarMd5();
                md5.Executar();

                new AtualizarMd5(md5.Md5Final).Executar();
            }
            catch (ArquivoAuxiliarInvalidoException ex)
            {
                RestaurarArquivoAuxiliar();
                TextoNotificacao = "Erro ao gerar Arquivo MD5: " + ex.Message;
                AtivaBotaoFechar = true;
                return;
            }
            catch (Exception e)
            {
                TextoNotificacao = "Erro ao gerar Arquivo MD5:" + e.Message;
            }

            ValidarConfiguracoesDoEcf();
        }

        private void ValidarConfiguracoesDoEcf()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Quease pronto! Estou checando o seu ECF";

            try
            {
                new EcfConfiguracao().ValidarConfiguracaoDoEcf();
            }
            catch (ConexaoInvalidaException e)
            {
                TextoNotificacao = "Houve um erro de conexão com o banco de dados" + e.Message;
                AtivaBotaoConexao = true;
                return;
            }
            catch (Exception e)
            {
                TextoNotificacao = e.Message;
                AtivaBotaoEcf = true;
                return;
            }

            InicializarEcf();
        }

        private void InicializarEcf()
        {
            AtivaProgressBar = true;
            TextoNotificacao = "Vamos la! Estou iniciando seu ECF!";

            try
            {
                var ecf = new ObterEcfEmUso().Buscar();
                Dispositivo.Modelo = ecf.ModeloAcbr;
                Dispositivo.Porta = ecf.Porta;
                Dispositivo.ControlePorta = ecf.ControlePorta;
                Dispositivo.Velocidade = int.Parse(ecf.Velocidade);
                Dispositivo.ModeloCompleto = ecf.Modelo;

                var modelos = new ListaModeloEcf().ObterModelosEcf();
                var modelo = modelos.First(e => e.ObterModeloEcf.ToString() == ecf.Modelo).Instancia;

                SessaoEcf.EcfFiscal = CriaEcfFiscal.ObterEcfFiscal(modelo) ?? SessaoEcf.EcfFiscal;
                SessaoEcf.EcfFiscal.Ativar();
            }
            catch (ArquivoAuxiliarInvalidoException)
            {
                RestaurarArquivoAuxiliar();
                InicializaSistemaAsync();
            }
            catch (Exception e)
            {
                TextoNotificacao = "Erro ao iniciar ECF: " + e.Message;
                AtivaBotaoEcf = true;
                return;
            }

            FinalizarFluxoInicializacao();
        }

        private static void RestaurarArquivoAuxiliar()
        {
            try
            {
                new FazerBackupArquivoAuxiliar("2").RestaurarArquivoAuxiliar();
            }
            catch (Exception)
            {
                try
                {
                    new FazerBackupArquivoAuxiliar("1").RestaurarArquivoAuxiliar();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message, ex);
                }
            }
        }

        private void FinalizarFluxoInicializacao()
        {
            Application.Current.Dispatcher.Invoke(OnConcluiuCarregamento);
        }

        private void ConfiguraConexaoAction(object obj)
        {
            new TelaConexaoPdv().ShowDialog();
            InicializaSistemaAsync();
        }

        private static void FecharAction(object obj)
        {
            Application.Current.Shutdown();
        }

        private void AtualizarVersaoAction(object obj)
        {
            var resultado = DialogBox.MostraConfirmacao("Deseja atualizar a base de dados?");

            if (resultado != MessageBoxResult.Yes) return;

            InicializaSistemaAsync();
        }

        private void ConfiguracaoEcfAction(object obj)
        {
            var configuracaoInicial = new ConfiguracaoInicialView();
            configuracaoInicial.ShowDialog();

            InicializaSistemaAsync();
        }

        private void ConfigurarServidorLicencaAction(object obj)
        {
            var dialog = new ConexaoServidorLicenca();
            dialog.ShowDialog();

            InicializaSistemaAsync();
        }
    }
}