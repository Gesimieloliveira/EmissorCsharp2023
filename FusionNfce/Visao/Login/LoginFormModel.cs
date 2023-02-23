using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using FontAwesome.WPF;
using FusionCore.Excecoes.Sessao;
using FusionCore.FusionNfce.CertificadosDigitais;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Setup.Conexao;
using FusionCore.FusionNfce.Usuario;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Sincronizador;
using FusionCore.Helpers.Wmi;
using FusionCore.MigracaoFluente;
using FusionCore.NfceSincronizador.Sync.TerminaisOffline;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionNfce.DI.Providers;
using FusionNfce.Visao.Configuracao.Impressao;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Login
{
    public class LoginFormModel : ViewModel
    {
        private string _login;
        private string _senha;
        private DispatcherTimer _timerStatusServidor;
        private SolidColorBrush _corStatus;
        private FontAwesomeIcon _statusServidor;
        private ObservableCollection<EmissorComboBoxDTO> _empresas = new ObservableCollection<EmissorComboBoxDTO>();
        private EmissorComboBoxDTO _empresaSelecionada;

        public EmissorComboBoxDTO EmpresaSelecionada
        {
            get => _empresaSelecionada;
            set
            {
                if (Equals(value, _empresaSelecionada)) return;
                _empresaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EmissorComboBoxDTO> Empresas
        {
            get => _empresas;
            set
            {
                if (Equals(value, _empresas)) return;
                _empresas = value;
                PropriedadeAlterada();
            }
        }

        public LoginFormModel()
        {
            VerificaStatusServidor();
            IniciaTimerStatusServico();
            Versao = ResponsavelLegal.VersaoSistema;
        }

        public string Versao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                PropriedadeAlterada();
            }
        }

        public string Senha
        {
            get => _senha;
            set
            {
                _senha = value;
                PropriedadeAlterada();
            }
        }

        public void EfetuarLogin()
        {
            if (string.IsNullOrEmpty(Login)) throw new ArgumentException("Preciso que informe um usuário");
            if (string.IsNullOrEmpty(Senha)) throw new ArgumentException("Preciso que informe uma senha");

            UsuarioNfce usuarioLogado;

            if (NaoEstaNaUltimaVersaoParaSincronizacao())
                DialogBox.MostraAviso("Pode usar o terminal para vendas normalmente mas nada sera sincronizado até atualizar o mesmo");

            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioUsuarioNfce(sessao);

                var senha = Sha1Helper.Computar(_senha);
                usuarioLogado = repositorio.FazerLogin(Login, senha);

                if (usuarioLogado == null)
                {
                    throw new InvalidOperationException("Credenciais informadas são inválidas!");
                }
            }

            usuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.GERENCIAR_PDV);

            DefineEmissorEmUso();
            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                var facadeCertificadoDigital = new CertificadoDigitalNfceFacade();
                facadeCertificadoDigital.ExisteCertificadoDigital(SessaoSistemaNfce.Empresa());
            }
            SessaoSistemaNfce.Usuario = usuarioLogado;

            DeletaArquivoDeLogSeExistir();
            CarregarLogoNaSessao();
            CarregarImpressaoDiretaNaSessao();
            CarregarConfiguracaoBalanca();
            CarregarConfiguracaoTef();
            CarregarCertificadoDigital();

            RemoveArquivoDeLogNfceSincronizador();
        }

        private bool NaoEstaNaUltimaVersaoParaSincronizacao()
        {
            try
            {
                var conexao = new ManipulaConexao().LerArquivo();
                var migracao = MigracaoFactory.CriaMigrador(conexao.ConexaoAdm.ToCfg(), MigracaoTag.Adm);

                using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoServerNfce).AbrirSessao())
                {
                    new ChecarVersaoBdAdm(migracao, sessao).Checar();
                }

                return false;
            }
            catch (ConexaoInvalidaException)
            {
                DialogBox.MostraAviso("Não consegui me comunicar com o servidor.\nPode continuar vendendo normalmente, mas enquanto não \nconseguir me conectar com o servidor não consigo \nenviar as vendas para lá e nem receber atualizações.");
                return false;
            }
            catch
            {
                return true;
            }
        }

        private void CarregarCertificadoDigital()
        {
            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                var repositorioCertificadoDigital = new RepositorioCertificadoDigitalNfce(sessao);
                SessaoSistemaNfce.CertificadoDigital = repositorioCertificadoDigital.CarregarPorEmpresa(SessaoSistemaNfce.Empresa());
            }
        }

        private void DefineEmissorEmUso()
        {
            foreach (var nfceEmissorFiscal in SessaoSistemaNfce.Configuracao.EmissorFiscalLista)
            {
                using (var sessao = new SessaoManagerNfce().CriaSessao())
                using (var transacao = sessao.BeginTransaction())
                {
                    nfceEmissorFiscal.EmUso = nfceEmissorFiscal.Id == EmpresaSelecionada.Id;

                    new RepositorioEmissorFiscalNfce(sessao).Salva(nfceEmissorFiscal);

                    transacao.Commit();
                }
            }
        }

        private void CarregarConfiguracaoTef()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                SessaoSistemaNfce.ConfigTef = new RepositorioConfigTef(sessao).BuscarConfiguracaoTef();
            }
        }

        private void RemoveArquivoDeLogNfceSincronizador()
        {
            try
            {
                var localArquivoLog = $"{ManipulaArquivo.LocalAplicacao()}\\nfce-sincronizador.log";

                new ManipulaArquivo(localArquivoLog).DeletaSeExistir();
            }
            catch
            {
                
            }
        }

        private void CarregarConfiguracaoBalanca()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                SessaoSistemaNfce.ConfiguracoesBalanca = new RepositorioBalancaNfce(sessao).BuscarUnicaBalanca();
            }
        }

        private void CarregarImpressaoDiretaNaSessao()
        {
            var impressaoDiretaAtiva = new SalvarImpressaoDireta().Ler();
            SessaoSistemaNfce.ImpressaoDireta = impressaoDiretaAtiva;
        }

        private static void DeletaArquivoDeLogSeExistir()
        {
            var arquivo = new ManipulaArquivo(@"C:\SistemaFusion\FusionNfce\nfce-sincronizador.log");
            arquivo.DeletaSeExistir();
        }

        public ConfiguracaoTerminalNfce BuscaConfiguracao()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioConfiguracaoTerminalNfce(sessao);
                var repositorioNfce = new RepositorioNfce(sessao);

                var configuracao = repositorio.GetPeloId(1);
                var contingencia = repositorioNfce.BuscarContingenciaAtiva();

                SessaoSistemaNfce.Contingencia = contingencia;

                if (configuracao != null)
                {
                    SessaoSistemaNfce.Configuracao = configuracao;
                    SessaoSistemaNfce.CaixaProvider = new ControleCaixaProvider(configuracao.TerminalOfflineId);
                }

                return configuracao;
            }
        }

        public static void TentaBuscarConfiguracaoDoServidor()
        {
            var diskDrive = WmiHelper.GetDiskDriveZero();
            var hash = Md5Helper.Computar($"agil4@{diskDrive.SerialNumber}");

            new SincronizarTerminalOfflineConfiguracao(hash).RealizarSincronizacao();
        }

        private void CarregarLogoNaSessao()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioConfiguracaoFrenteCaixaNfce(sessao);

                SessaoSistemaNfce.ConfiguracaoFrenteCaixa = repositorio.BuscarUnico();
            }
        }

        private void IniciaTimerStatusServico()
        {
            _timerStatusServidor = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
            _timerStatusServidor.Dispatcher.Thread.IsBackground = true;
            _timerStatusServidor.Tick += DispatcherTimer_StatusServidor;
            _timerStatusServidor.Interval = new TimeSpan(0, 0, 0, 10, 0);
            _timerStatusServidor.Start();
        }

        private void DispatcherTimer_StatusServidor(object sender, EventArgs e)
        {
            VerificaStatusServidor();
        }

        private void VerificaStatusServidor()
        {
            new Thread(() =>
            {
                try
                {
                    new ConsultarStatusDoServico().Executar("FusionNfceSincronizador");
                    StatusServidor = FontAwesomeIcon.Database;
                    CorStatus = Brushes.LawnGreen;
                }
                catch (Exception)
                {
                    StatusServidor = FontAwesomeIcon.Database;
                    CorStatus = Brushes.DarkRed;
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        public SolidColorBrush CorStatus
        {
            get => _corStatus;
            set
            {
                if (Equals(value, _corStatus)) return;
                _corStatus = value;
                PropriedadeAlterada();
            }
        }

        public FontAwesomeIcon StatusServidor
        {
            get => _statusServidor;
            set
            {
                if (value == _statusServidor) return;
                _statusServidor = value;
                PropriedadeAlterada();
            }
        }

        public void InicializaComboBoxEmpresa()
        {
            var empresasNfce = SessaoSistemaNfce.Configuracao.EmissorFiscalLista.Select(nfceEmissorFiscal => 
                new EmissorComboBoxDTO
                {
                    Id = nfceEmissorFiscal.Id, 
                    EmUso = nfceEmissorFiscal.EmUso,
                    IsSat = nfceEmissorFiscal.FlagSat,
                    IsMfe = nfceEmissorFiscal.EmissorFiscalSat != null && nfceEmissorFiscal.EmissorFiscalSat.IsMFe,
                    IsNfce = nfceEmissorFiscal.FlagNfce,
                    EmpresaComboBox = new EmpresaComboBoxDTO
                    {
                        Nome = nfceEmissorFiscal.Empresa.NomeFantasia,
                        Id = nfceEmissorFiscal.Empresa.Id
                    }
                }).ToList();

            Empresas = new ObservableCollection<EmissorComboBoxDTO>(empresasNfce);

            var empresaNfce = empresasNfce.FirstOrDefault(x => x.EmUso == true) ?? empresasNfce.FirstOrDefault();

            EmpresaSelecionada = empresaNfce;
        }
    }
}