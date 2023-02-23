using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionCore.Excecoes.Sessao;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.CertificadosDigitais;
using FusionCore.FusionNfce.ConfiguracaoSat;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.NfceSincronizador.Sync;
using FusionCore.NfceSincronizador.Sync.Start;
using FusionCore.Repositorio.FusionNfce;
using FusionNfce.Core;
using FusionNfce.Visao.BarraDeProgresso;
using FusionNfce.Visao.Conexao;
using FusionNfce.Visao.ConfiguracaoTerminal;
using FusionNfce.Visao.ConfiguraCertificado;
using FusionNfce.Visao.Principal;
using FusionWPF.Base.Utils.Dialogs;
using OpenAC.Net.Core.Extensions;

namespace FusionNfce.Visao.Login
{
    public partial class LoginForm
    {
        private readonly LoginFormModel _formModel;

        public LoginForm()
        {
            _formModel = new LoginFormModel();
            DataContext = _formModel;
            InitializeComponent();
        }

        private void Login_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F11:
                    new ConexaoForm().ShowDialog();
                    break;
                case Key.F5:
                    Sync_OnClick(sender, e);
                    break;
            }
        }

        private void Conexao_OnClick(object sender, RoutedEventArgs e)
        {
            new ConexaoForm().ShowDialog();
        }

        private void BtEntrar_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _formModel.EfetuarLogin();

                if (SessaoSistemaNfce.TipoEmissao != TipoEmissao.Normal)
                {
                    DialogBox.MostraInformacao("O sistema está configurado para modo contingência => " +
                                               SessaoSistemaNfce.TipoEmissao.GetDescription() + "\n"
                                               + "Justificativa => " + SessaoSistemaNfce.Contingencia.Motivo +
                                               "\n" +
                                               "Data da entrada em contingência => " +
                                               SessaoSistemaNfce.Contingencia.EntrouEm);
                }

                new VendaCaixa().Show();
                Close();
            }
            catch (NaoExisteCertificadoDigitalException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                new CertificadoDigitalForm(new CertificadoDigitalFormModel()).ShowDialog();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private async void LoginForm_OnContentRendered(object sender, EventArgs e)
        {
#if DEBUG
            _formModel.Login = "admin";
            _formModel.Senha = "agil4";
#endif

            var configuracao = _formModel.BuscaConfiguracao();

            if (configuracao == null)
            {
                await Task.Run(() => OnContentRendered(sender, e));
            }

            if (configuracao == null)
            {
                configuracao = _formModel.BuscaConfiguracao();
            }

            if (configuracao == null)
            {
                return;
            }

            if (configuracao.EmissorFiscal.FlagSat)
            {
                ConfigurarSat(configuracao);
            }

            _formModel.InicializaComboBoxEmpresa();
        }

        private void ConfigurarSat(ConfiguracaoTerminalNfce configuracao)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var repositorio = new RepositorioConfiguracaoSatFiscalNfce(sessao);
                var cfgTerminal = repositorio.BuscarConfiguracao() ?? new ConfiguracaoSatFiscal();

                cfgTerminal.Id = 1;
                cfgTerminal.Ativo = true;
                cfgTerminal.AssociadoAssinatura = true;
                cfgTerminal.CodificacaoArquivoXml = configuracao.EmissorFiscal.EmissorFiscalSat.CodificacaoArquivoXml;
                cfgTerminal.CodigoAssociacao = configuracao.EmissorFiscal.EmissorFiscalSat.CodigoAcossiacao;
                cfgTerminal.CodigoAtivacao = configuracao.EmissorFiscal.EmissorFiscalSat.CodigoAtivacao;
                cfgTerminal.FabricanteModelo = configuracao.EmissorFiscal.EmissorFiscalSat.Fabricante;

                repositorio.Salvar(cfgTerminal);
            }
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);

                LoginFormModel.TentaBuscarConfiguracaoDoServidor();

                var configuracao = _formModel.BuscaConfiguracao();

                if (configuracao == null)
                {
                    throw new InvalidOperationException("Para continuar preciso que configure este Terminal");
                }

                Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
            }
            catch (KeyNotFoundException ex)
            {
                Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                Dispatcher.Invoke(() =>
                {
                    DialogBox.MostraInformacao(ex.Message);

                    new ConexaoForm().ShowDialog();
                    GerenciaSessaoNfce.GerenciaSessaoInicializaTodasConexoes();
                });
                OnContentRendered(sender, e);
            }
            catch (ConexaoInvalidaException ex)
            {
                Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                Dispatcher.Invoke(() =>
                {
                    DialogBox.MostraInformacao(ex.Message);

                    new ConexaoForm().ShowDialog();
                    GerenciaSessaoNfce.GerenciaSessaoInicializaTodasConexoes();
                });
                OnContentRendered(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                DialogBox.MostraInformacao(ex.Message);
                Dispatcher.Invoke(() =>
                {
                    new ConfiguracaoTerminalForm().ShowDialog();
                });
            }
        }

        private async void Sync_OnClick(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();

            await Task.Run(() =>
            {
                try
                {
                    ControlaServicoSincronizador.Parar();
                    GerenciaSessaoNfce.GerenciaSessaoInicializaTodasConexoes();

                    if (VinculoTerminalFacade.SemViculoComServidor())
                    {
                        throw new InvalidOperationException("Terminal não possui vinculo com o servidor!");
                    }

                    new SincronizadorStart().Start();
                }
                catch (KeyNotFoundException ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraInformacao(ex.Message);
                        new ConexaoForm().ShowDialog();
                    });
                }
                catch (ConexaoInvalidaException ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso(ex.Message);
                        new ConexaoForm().ShowDialog();
                    });
                }
                catch (InvalidOperationException ex)
                {
                    Dispatcher.Invoke(() => DialogBox.MostraAviso(ex.Message));
                }
                finally
                {
                    ControlaServicoSincronizador.Iniciar();
                }
            });

            ProgressBarAgil4.CloseProgressBar();
        }
    }
}