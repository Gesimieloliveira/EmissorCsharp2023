using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Culture;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionPdv.Acbr;
using FusionPdv.Ecf;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfoHelper.DefineDefaultCultureInfo();

            if (UnicoAberto.IsAppAlreadyRunning(Process.GetCurrentProcess()))
            {
                DialogBox.MostraAviso("Fusion PDV", "Fusion PDV já está em excecução!");
                Process.GetCurrentProcess().Kill();
                return;
            }

            DispatcherUnhandledException += UnhandledExcpetionHandler;

            // ChecadorLicencaOffline.Instancia.AcessoInvalido += AcessoInvalidoHandler;
            // ChecadorLicencaOffline.Instancia.AcessoValido += AcessoValidoHandler;

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            FinalizarLicenciamento();
            FinalizarAcbrFramework();
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

        private static void FinalizarAcbrFramework()
        {
            try
            {
                AcbrFactory.FecharTodosSemLancarErro();
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private void AcessoValidoHandler(object sender, AcessoConcedido e)
        {
            SessaoSistema.AcessoConcedido = e;
        }

        private void AcessoInvalidoHandler(object sender, Exception e)
        {
            try
            {
                var estadoEcf = SessaoSistema.EstadoEcf is EstadoEcfFiscal
                    ? (EstadoEcfFiscal) SessaoSistema.EstadoEcf
                    : EstadoEcfFiscal.Livre;

                if (estadoEcf != EstadoEcfFiscal.Livre)
                {
                    SessaoSistema.AcessoConcedido = null;
                    SessaoSistema.MensagemErroAcesso = e.Message;
                    return;
                }

                Dispatcher.Invoke(() =>
                {
                    DialogBox.MostraAviso("Licencimento", e.Message);
                    Shutdown();
                });
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private static void UnhandledExcpetionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            DialogBox.MostraErro("Aconteceu um erro não esperado", e.Exception);
        }
    }
}