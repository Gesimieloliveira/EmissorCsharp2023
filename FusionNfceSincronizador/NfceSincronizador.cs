using System;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Helpers.Log;
using FusionCore.Helpers.Sincronizador;
using FusionCore.NfceSincronizador.Sync;
using FusionCore.NfceSincronizador.Sync.Start;
using FusionCore.Repositorio.FusionNfce;
using Timer = System.Timers.Timer;

namespace FusionNfceSincronizador
{
    public partial class FusionNfceSincronizador : ServiceBase
    {
        private static readonly IRegistrarLog Log = RegistarLog.Istancia;
        private int _intervaloDaSincronizacao = 15000;

        public FusionNfceSincronizador()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.Registrar("Serviço iniciado");

            var timerInicializar = new Timer(3000);
            timerInicializar.Elapsed += InicializarSessoes;
            timerInicializar.Start();
        }

        private void InicializarSessoes(object sender, ElapsedEventArgs e)
        {
            var timer = (Timer) sender;

            try
            {
                timer.Stop();
                ServicoDeSincronizacaoStart.CriaConexoes();

                var timerSync = new Timer(_intervaloDaSincronizacao);
                timerSync.Elapsed += EfetuaSincronizacao;
                timerSync.Start();
            }
            catch (Exception ex)
            {
                Log.Registrar("Erro ao inicializar as sessoes: " + ex.Message);
                Log.Registrar($"Nova tentativa em {timer.Interval/1000} segundos");
                timer.Start();
            }
        }

        private void EfetuaSincronizacao(object sender, ElapsedEventArgs e)
        {
            var timer = (Timer) sender;
            timer.Stop();

            try
            {
                if (VinculoTerminalFacade.SemViculoComServidor())
                {
                    throw new InvalidOperationException("Terminal não possui vinculo com o servidor!");
                }

                Log.Registrar("Serviço iniciou uma sincronização");

                ServicoDeSincronizacaoStart.TrataException(() => { AdicionaIntervaloSincronizacao(timer); });
                ServicoDeSincronizacaoStart.Sincronizar();
                new GravaStatusPdvSyncronizador("1").Executar();

                Log.Registrar("Serviço concluiu com exito a sincronização");
            }
            catch (Exception ex)
            {
                new GravaStatusPdvSyncronizador("0").Executar();

                Log.Registrar("Erro ao tentar sincronizar. Em 60 segundos será executado um novo teste!");
                Log.Registrar(ex.Message);

                Thread.Sleep(60000);
                OnStart(null);

                return;
            }

            timer.Start();
        }

        private void AdicionaIntervaloSincronizacao(Timer timer)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioConfiguracaoTerminalNfce(sessao);

                var intervalo = repositorio.ObterIntervaloSincronizacao();

                _intervaloDaSincronizacao = intervalo == 0 ? 15000 : intervalo;

                timer.Interval = _intervaloDaSincronizacao;
            }
        }

        protected override void OnStop()
        {
            Log.Registrar("Serviço foi parado");
        }

        public void CallOnStop()
        {
            OnStop();
        }

        public void CallOnStart(string[] args)
        {
            OnStart(args);
        }
    }
}