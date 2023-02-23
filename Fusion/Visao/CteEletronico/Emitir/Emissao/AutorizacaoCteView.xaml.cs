using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FusionCore.FusionAdm.CteEletronico.Emissao;

namespace Fusion.Visao.CteEletronico.Emitir.Emissao
{
    public partial class AutorizacaoCteView
    {
        private readonly AutorizacaoCteModel _viewModel;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);
        public event EventHandler FecharCteForm;
        private bool _forcaProximoNumero;

        public AutorizacaoCteView(Cte autorizar)
        {
            _viewModel = new AutorizacaoCteModel(autorizar);
            InitializeComponent();
            DataContext = _viewModel;
        }

        public Task<AutorizacaoModelResposta> AutorizaAsync()
        {
            Dispatcher.BeginInvoke(new Func<bool?>(ShowDialog));
            return TaskAutorizaAsync();
        }

        private async Task<AutorizacaoModelResposta> TaskAutorizaAsync()
        {
            return await Task.Run(() =>
            {
                AutorizacaoModelResposta resposta;

                while (true)
                {
                    resposta = _viewModel.EmiteNotaFiscal(_forcaProximoNumero);

                    _forcaProximoNumero = false;
                    _waitHandle.WaitOne();

                    if (_forcaProximoNumero == false) break;
                }

                return resposta;
            });
        }

        private void OnClickAutorizaComProximoNumero(object sender, RoutedEventArgs e)
        {
            _forcaProximoNumero = true;
            _waitHandle.Set();
        }

        protected override void OnClosed(EventArgs e)
        {
            _waitHandle.Set();
            base.OnClosed(e);

            if (_viewModel.GetStatusEmissao() == CteEmissaoStatus.Pendente)
            {
                OnFecharCteForm();
            }
        }

        protected virtual void OnFecharCteForm()
        {
            FecharCteForm?.Invoke(this, EventArgs.Empty);
        }
    }
}