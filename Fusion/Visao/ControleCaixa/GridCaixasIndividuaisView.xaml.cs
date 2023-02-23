using System;
using System.Windows;
using System.Windows.Input;
using Fusion.FastReport.Relatorios.Sistema.Caixa;
using Fusion.Helpers;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SharedViews.ControleCaixa;

namespace Fusion.Visao.ControleCaixa
{
    public partial class GridCaixasIndividuaisView
    {
        public GridCaixasIndividuaisView(GridCaixasIndividuaisContexto contexto)
        {
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
            Contexto = contexto;
        }

        public GridCaixasIndividuaisContexto Contexto { get; }
        public event EventHandler MexeuEmLancamento;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;

            Contexto.CarregarFiltros();
            CarregarDados();
        }

        public void CarregarDados()
        {
            Contexto.CarregarDadosCaixaLoja();
            Contexto.CarregarDadosCaixasIndividuais();

            MoveScrollParaUltimoItemExtrato();
        }

        private void MoveScrollParaUltimoItemExtrato()
        {
            LbExtratoContaCaixa.Items.MoveCurrentToLast();
            LbExtratoContaCaixa.ScrollIntoView(LbExtratoContaCaixa.Items.CurrentItem);
        }

        private void AbrirCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var contexto = new AberturaDeCaixaContexto(Contexto.CaixaProvider, Contexto.SessaoManager);
                var view = new AberturaDeCaixaView(contexto);

                if (view.ShowDialog() != true) return;

                CarregarDados();
                OnMexeuEmLancamento();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void AplicarBuscaClickHandler(object sender, RoutedEventArgs e)
        {
            CarregarDados();
        }

        private void RowDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            var caixaId = Contexto.ItemSelecionado.Id;

            var contexto = new ResumoCaixaIndividualContexto(
                Contexto.SessaoManager, 
                Contexto.CaixaProvider, 
                caixaId
            );

            var view = new ResumoCaixaIndividualView(contexto);

            view.ShowDialog();
            CarregarDados();
            OnMexeuEmLancamento();
        }

        private void ImprimirCaixasFechadosClickHandler(object sender, RoutedEventArgs e)
        {
            using (var r = new RListagemDeCaixasFechados(Contexto.SessaoManager))
            {
                r.Visualizar();
            }
        }

        private void NovoLancamentoClickHandler(object sender, RoutedEventArgs e)
        {
            var view = new LancamentoNoCaixaView(Contexto.SessaoManager, Contexto.CaixaProvider);

            var result = view.ShowDialog();

            if (result != true) return;

            CarregarDados();
            OnMexeuEmLancamento();
        }

        protected virtual void OnMexeuEmLancamento()
        {
            MexeuEmLancamento?.Invoke(this, EventArgs.Empty);
        }
    }
}