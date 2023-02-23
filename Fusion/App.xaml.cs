using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows;
using System.Windows.Threading;
using Fusion.FastReport.Relatorios;
using Fusion.Licenciamento;
using Fusion.Sessao;
using Fusion.Storage;
using Fusion.WhiteLabel;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Culture;
using FusionCore.Helpers.Log;
using FusionCore.Helpers.Network;
using FusionCore.Permissoes;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion
{
    public partial class App
    {
        private static readonly ChecadorLicenca ChecadorLicenca = ChecadorLicenca.Instancia;
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CultureInfoHelper.DefineDefaultCultureInfo();
            DependenciaStorageFusion.Referenciar();
            FrSettings.ConfigurarIdiomaBrasil();

            if (UnicoAberto.IsAppAlreadyRunning(Process.GetCurrentProcess()))
            {
                DialogBox.MostraAviso("Fusion", $"{MarcaWhiteLabel.NomeSoftware} já está em excecução!");
                Process.GetCurrentProcess().Kill();
                return;
            }

            NetworkHelper.DefineCertificateValidation();
            FusionLog.ConfiguraLog();

            DispatcherUnhandledException += UnhandledExcpetionHandler;
            ChecadorLicenca.ChecagemErro += ErroChecagemLicencaHandler;
        }

        private static void ErroChecagemLicencaHandler(object sender, Exception e)
        {
            if (Current.Dispatcher == null)
            {
                Current.Shutdown();
                return;
            }

            const string titulo = "Licenciamento";
            var mensagem = "(FusionApi): Falha ao checar a Licença no Servidor!";

            if (e is FaultException fex)
            {
                mensagem += $"\n{fex.Message}";
            }

            Current.Dispatcher.Invoke(() =>
            {
                DialogBox.MostraAviso(titulo, mensagem);
                Current.Shutdown();
            });
        }

        private static void UnhandledExcpetionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            e.Dispatcher.Invoke(() =>
            {
                switch (e.Exception)
                {
                    case InvalidOperationException _:
                    case PermissaoException _:

                        DialogBox.MostraAviso(e.Exception.Message);

                        return;

                    default:
                        DialogBox.MostraErro(e.Exception.Message, e.Exception);
                        break;
                }
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                ChecadorLicenca.ParaChecagem();
            }
            catch (Exception)
            {
                //ignore
            }
            finally
            {
                base.OnExit(e);
            }
        }
    }
}