using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using FusionCore.FusionNfce.Fiscal;
using MahApps.Metro.Controls;

namespace FusionNfce.Visao.Autorizacao.Emissao
{
    public partial class AutorizacaoNfceView : MetroWindow
    {
        private readonly AutorizacaoNfceModel _viewModel;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        public AutorizacaoNfceView()
        {
            InitializeComponent();
        }

        public AutorizacaoNfceView(Nfce autorizar) : this()
        {
            _viewModel = new AutorizacaoNfceModel(autorizar);
            DataContext = _viewModel;
        }

        public Task<AutorizacaoModelResposta> AutorizaAsync()
        {
            KeyDown -= AutorizacaoNfceView_OnKeyDown;
            Dispatcher.BeginInvoke(new Func<bool?>(ShowDialog));
            return TaskAutorizaAsync();
        }

        private async Task<AutorizacaoModelResposta> TaskAutorizaAsync()
        {
            return await Task.Run(async () =>
            {
                Resposta = await _viewModel.EmiteNotaFiscalAsync();
                KeyDown += AutorizacaoNfceView_OnKeyDown;

                if (!Resposta.Sucesso)
                    _waitHandle.WaitOne();

                return Resposta;
            });
        }

        public AutorizacaoModelResposta Resposta { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            _waitHandle?.Set();
            base.OnClosed(e);
        }

        private void AutorizacaoNfceView_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}