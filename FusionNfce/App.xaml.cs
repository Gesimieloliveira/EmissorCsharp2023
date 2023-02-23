using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Fusion.FastReport.Relatorios;
using Fusion.Storage;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Culture;
using FusionCore.Helpers.Log;
using FusionCore.Helpers.Network;
using FusionCore.Permissoes;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionNfce.Storage;
using FusionNfce.Visao.BarraDeProgresso;
using FusionNfce.Visao.Principal;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfoHelper.DefineDefaultCultureInfo();
            DependenciaStorageFusion.Referenciar();
            DependenciaStorageNfce.Referenciar();
            FrSettings.ConfigurarIdiomaBrasil();

            if (UnicoAberto.IsAppAlreadyRunning(Process.GetCurrentProcess()))
            {
                DialogBox.MostraAviso("Fusion NFC-E", "Fusion NFC-E já está em excecução!");
                Shutdown();
                return;
            }

            FusionLog.ConfiguraLog();
            NetworkHelper.DefineCertificateValidation();

            DispatcherUnhandledException += UnhandledExcpetionHandler;

            // TODO 1612 - LICENCIAMENTO: Checagem de acesso invalido
            // ChecadorLicencaOffline.Instancia.AcessoInvalido += AcessoInvalidoHandler;
            // ChecadorLicencaOffline.Instancia.AcessoValido += AcessoValidoHandler;

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            FinalizarLicenciamento();
            Application.Current.Shutdown();
        }

        private static void FinalizarLicenciamento()
        {
            try
            {
                
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private static void UnhandledExcpetionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            
            Current.Dispatcher.Invoke(() =>
            {
                ProgressBarAgil4.CloseProgressBar();

                if (e.Exception is PermissaoException ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                    return;
                }

                DialogBox.MostraErro($"Erro inesperado: {e.Exception.Message}", e.Exception);
            });
        }

        private void AcessoValidoHandler(object sender, AcessoConcedido e)
        {
            SessaoSistemaNfce.AcessoConcedido = e;
        }

        private void AcessoInvalidoHandler(object sender, Exception e)
        {
            try
            {
                var statusVenda = SessaoSistemaNfce.StatusVenda is StatusVenda
                    ? (StatusVenda) SessaoSistemaNfce.StatusVenda
                    : StatusVenda.Livre;

                SessaoSistemaNfce.AcessoConcedido = null;
                SessaoSistemaNfce.MensagemErroAcesso = e.Message;

                if (statusVenda != StatusVenda.Livre)
                    return;

                Dispatcher.Invoke(() =>
                {
                    DialogBox.MostraAviso("Licenciamento", e.Message);
                    Shutdown();
                });
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}