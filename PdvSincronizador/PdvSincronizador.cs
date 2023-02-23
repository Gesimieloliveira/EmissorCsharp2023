using System;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Sincronizador;
using FusionCore.PdvSincronizador.Sync;
using FusionSincronizador.Core;

namespace FusionSincronizador
{
    public partial class FusionPdvSincronizador : ServiceBase
    {
        public FusionPdvSincronizador()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timerSync = new Timer(GetIntervaloSyncronizacao());
            Log.Path = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                timerSync.Elapsed += Sincronizacoes;
                timerSync.Start();

                Log.Registrar("Serviço conseguiu ser inicializado");
            }
            catch (Exception e)
            {
                new GravaStatusPdvSyncronizador("0").Executar();
                Log.Registrar("Falha ao inicializar o serviço");
                Log.Registrar(e.Message);
            }
        }

        private static double GetIntervaloSyncronizacao()
        {
            const int timePadrao = 15000;

            try
            {
                var arquivoCfg = Path.Combine(DiretorioAssembly.GetPastaConfig(), "pdvsync-elapsed.cfg");

                if (!File.Exists(arquivoCfg))
                {
                    File.Create(arquivoCfg).Close();
                    File.WriteAllText(arquivoCfg, timePadrao.ToString(), Encoding.ASCII);
                    return timePadrao;
                }

                var content = File.ReadAllText(arquivoCfg);
                return int.Parse(content.Trim(), 0);
            }
            catch (Exception ex)
            {
                return timePadrao;
            }
        }

        private static void Sincronizacoes(object sender, ElapsedEventArgs e)
        {
            var timer = (Timer)sender;

            try
            {
                timer.Stop();

                Sincronizador.Instancia.SincronizarTudo();
                new GravaStatusPdvSyncronizador("1").Executar();
            }
            catch (Exception ex)
            {
                new GravaStatusPdvSyncronizador("1").Executar();

                Log.Registrar("Falha ao sincronizar");
                Log.Registrar("Exception: " + ex.Message);
                Log.Registrar("Inner Exception: " + ex.InnerException?.Message);
            }
            finally
            {
                timer.Start();
            }
        }

        protected override void OnStop()
        {
            Log.Registrar("Serviço foi parado");
        }

        public void CallOnStart(string[] args)
        {
            OnStart(args);
        }

        public void CallOnStop()
        {
            OnStop();
        }
    }
}