using System;
using System.Windows;
using System.Windows.Input;
using FusionCore.DI;
using FusionCore.Sessao;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public partial class GridLancamentosCaixaControl
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly IControleCaixaProvider _caixaProvider;
        public event EventHandler MexeuEmLancamento;

        public GridLancamentosCaixaControl(ISessaoManager sessaoManager, IControleCaixaProvider caixaProvider)
        {
            _sessaoManager = sessaoManager;
            _caixaProvider = caixaProvider;
            InitializeComponent();

            Contexto = new GridLancamentosCaixaContexto(_sessaoManager);
        }

        public GridLancamentosCaixaContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.ListarItems();

            DataContext = Contexto;
        }

        private void NovoClickHandler(object sender, RoutedEventArgs e)
        {
            var view = new LancamentoNoCaixaView(_sessaoManager, _caixaProvider);
            var result = view.ShowDialog();

            if (result != true) return;

            Contexto.ListarItems();
            OnMexeuEmLancamento();
        }

        private void DoubleClickRowHandler(object sender, MouseButtonEventArgs e)
        {
            var view = new LancamentoNoCaixaView(_sessaoManager, _caixaProvider);

            view.Contexto.PrepararEdicao(Contexto.ItemSelecionado);

            if (view.ShowDialog() != true) return;

            Contexto.ListarItems();
            OnMexeuEmLancamento();
        }

        protected virtual void OnMexeuEmLancamento()
        {
            MexeuEmLancamento?.Invoke(this, EventArgs.Empty);
        }
    }
}