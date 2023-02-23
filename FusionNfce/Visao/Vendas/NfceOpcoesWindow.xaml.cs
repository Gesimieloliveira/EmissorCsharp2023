using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FusionCore.Helpers.Basico;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Utils;

namespace FusionNfce.Visao.Vendas
{
    public partial class NfceOpcoesWindow
    {
        public NfceOpcoesWindow(NfceOpcoesViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }

        private NfceOpcoesViewModel ViewModel => DataContext as NfceOpcoesViewModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            TbSearch.Focus();
            ViewModel.Inicializar();
        }

        private void CancelarNfceTransmitidaForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    OpcoesDoItem();
                    break;
                case Key.F3:
                    AplicarSearch();
                    break;
                case Key.Escape:
                    FecharTelaSemSelecionarItem();
                    break;
            }
        }

        private void ClickFiltrarHandler(object sender, RoutedEventArgs e)
        {
            AplicarSearch();
        }

        private void AplicarSearch()
        {
            ViewModel.EmissaoInicial = DateTimeHelper.Parse(DataInicio.Text);
            ViewModel.EmissaoFinal = DateTimeHelper.Parse(DataFim.Text);

            try
            {
                ViewModel.AplicarFiltro();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void LbListaDeProdutos_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    OpcoesDoItem();
                    break;
            }
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpcoesDoItem();
        }

        private void ClickOpcoesHandler(object sender, RoutedEventArgs e)
        {
            OpcoesDoItem();
        }

        private void OpcoesDoItem()
        {
            if (ViewModel.ItemSelecionado == null)
            {
                return;
            }

            try
            {
                ViewModel.CancelarNfce();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FecharTelaSemSelecionarItem()
        {
            ViewModel.ItemSelecionado = null;
            Close();
        }

        private void TextInputSearchHandler(object sender, TextCompositionEventArgs e)
        {
            ViewModel.EmissaoInicial = null;
            ViewModel.EmissaoFinal = null;
        }

        private void FiltroKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && ViewModel.Itens.Any())
            {
                LbListaDeProdutos.FocusFirstItem();
                e.Handled = true;
            }

            if (Equals(e.Source, TbSearch) && e.Key == Key.Enter)
            {
                AplicarSearch();
                e.Handled = true;
            }
        }
    }
}
