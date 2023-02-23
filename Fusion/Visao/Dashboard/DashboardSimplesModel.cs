using System.Threading.Tasks;
using System.Timers;
using Fusion.Sessao;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Dashboard
{
    public sealed class DashboardSimplesModel : ViewModel
    {
        private readonly  SessaoManagerAdm _sessaoManager = new SessaoManagerAdm();
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private Timer _timerAutoRefresh;

        public DashboardSimplesModel()
        {
            AniversarianteContexto = new AniversarianteContexto(_sessaoManager);
            TotalizadoresContexto = new TotalizadoresContexto(_sessaoManager);
            DashContasPagarContexto = new DashContasPagarContexto(_sessaoManager, _sessaoSistema);
        }

        public AniversarianteContexto AniversarianteContexto
        {
            get => GetValue<AniversarianteContexto>();
            set => SetValue(value);
        }

        public TotalizadoresContexto TotalizadoresContexto
        {
            get => GetValue<TotalizadoresContexto>();
            set => SetValue(value);
        }

        public DashContasPagarContexto DashContasPagarContexto
        {
            get => GetValue<DashContasPagarContexto>();
            set => SetValue(value);
        }

        public void RefreshAsync()
        {
            Task.Run(() =>
            {
                RefreshAll();
            });
        }

        private void RefreshAll()
        {
            AniversarianteContexto.Refresh();
            TotalizadoresContexto.Refresh();

            DashContasPagarContexto.ComTotalizacao(TotalizadoresContexto);
            DashContasPagarContexto.Refresh();
        }

        public void IniciaAutoRefresh()
        {
            _timerAutoRefresh?.Stop();

            _timerAutoRefresh = new Timer(300000); //300000 = 5 minutos
            _timerAutoRefresh.Elapsed += AutoRefreshElapsedAsync;
            _timerAutoRefresh.Start();
        }

        private void AutoRefreshElapsedAsync(object sender, ElapsedEventArgs e)
        {
            _timerAutoRefresh.Stop();

            try
            {
                if (_sessaoSistema.UsuarioLogado != null)
                {
                    RefreshAll();
                }
            }
            finally
            {
                if (_sessaoSistema.UsuarioLogado != null)
                {
                    _timerAutoRefresh.Start();
                }
            }
        }
    }
}