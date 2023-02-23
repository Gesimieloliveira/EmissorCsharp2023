using System;
using System.ComponentModel;
using System.Timers;
using FusionCore.Servicos.Core.Servicos;
using FusionCore.Sessao;

namespace Fusion.Background.App
{
    public class Worker : BackgroundWorker
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private Timer _timerEnviaNfceOffline;
        private Timer _timerExportacaoXml;


        protected override void OnDoWork(DoWorkEventArgs e)
        {
            _timerExportacaoXml = new Timer(GetIntervaloArranqueExportacaoXml());
            _timerExportacaoXml.Elapsed += ServicoExportacaoXml;
            _timerExportacaoXml.Start();

            _timerEnviaNfceOffline = new Timer(GetIntervaloEnviarNfceOffline());
            _timerEnviaNfceOffline.Elapsed += ServicoEnviarNfceOffline;
            _timerEnviaNfceOffline.Start();
        }

        private void ServicoExportacaoXml(object sender, ElapsedEventArgs e)
        {
            _timerExportacaoXml.Stop();
            _timerExportacaoXml.Interval = GetIntervaloSequentes();

            try
            {
                InicializarSessaoManager();
                FazerExportacoes();
            }
            catch (Exception ex)
            {
                _timerExportacaoXml.Interval = GetIntervaloArranqueExportacaoXml();
                Console.WriteLine($@"Error: {ex.Message}");
            }
            finally
            {
                _sessaoManager.FecharFactory();
                _timerExportacaoXml.Start();
            }
        }

        private void FazerExportacoes()
        {
            var servicoExportacao = new ServicoExportacao(_sessaoManager);
            servicoExportacao.FazerExportacoes();
        }

        private void ServicoEnviarNfceOffline(object sender, ElapsedEventArgs e)
        {
            _timerEnviaNfceOffline.Stop();
            _timerEnviaNfceOffline.Interval = GetIntervaloEnviarNfceOffline();

            try
            {
                InicializarSessaoManager();
                ExecutarEnvioNfceOffline();
            }
            catch (Exception ex)
            {
                _timerEnviaNfceOffline.Interval = GetIntervaloEnviarNfceOffline();
                Console.WriteLine($@"Error: {ex.Message}");
            }
            finally
            {
                _sessaoManager.FecharFactory();
                _timerEnviaNfceOffline.Start();
            }
        }

        private void ExecutarEnvioNfceOffline()
        {
            var enviarNfcesOffline = new EnviarNfcesOffline(_sessaoManager);
            enviarNfcesOffline.Enviar();
        }

        private void InicializarSessaoManager()
        {
            if (_sessaoManager.FactoryIsOpen)
            {
                return;
            }

            _sessaoManager.CarregarFactory();
        }

        private static int GetIntervaloArranqueExportacaoXml()
        {
#if DEBUG
            return 5000;
#else
            return 60000;
#endif
        }

        private int GetIntervaloEnviarNfceOffline()
        {
#if DEBUG
            return 60000;
#else
            return 300000;
#endif            
        }

        private static int GetIntervaloSequentes()
        {
#if DEBUG
            return 15000;
#else
            return 600000;
#endif
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _timerEnviaNfceOffline?.Stop();
            _timerExportacaoXml?.Stop();
        }
    }
}