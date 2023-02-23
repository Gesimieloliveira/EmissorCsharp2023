using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Utils;

namespace FusionNfce.Visao.ConsultaVendedores
{
    public partial class ConsultaVendedorForm
    {
        private readonly ConsultaVendedorFormModel _model;

        public ConsultaVendedorForm(ConsultaVendedorFormModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = model;
            RegistrarAtalho(Key.F6, AcaoFocarTextBoxBusca);
            RegistrarAtalho(Key.Escape, Close);
        }

        private void AcaoFocarTextBoxBusca()
        {
            TextBoxPesquisa.Focus();
            TextBoxPesquisa.SelectAll();
        }


        private void TextBoxBuscaClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoExecutarBusca();
        }

        private void TextBoxBuscaKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && _model.Vendedores.Any())
            {
                e.Handled = true;

                GridProdutos.Focus();
                GridProdutos.SelectedItem = _model.Vendedores.First();
                GridProdutos.FocusFirstItem();

                return;
            }

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AcaoExecutarBusca();
            }
        }

        private void AcaoExecutarBusca()
        {
            _model.TextoPesquisa = TextBoxPesquisa.Text;
            _model.CarregarDadosDosVendedores();

            if (_model.Vendedores.Any())
            {
                GridProdutos.ScrollIntoView(_model.Vendedores.First());
            }
        }

        private void DataGridRowKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AcaoSelecionarVendedor();
            }
        }

        private void AcaoSelecionarVendedor()
        {
            try
            {
                _model.Selecionar();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void DataGridRowDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            AcaoSelecionarVendedor();
        }

        private void ConsultaVendedorForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.CarregarDadosDosVendedores();
        }
    }
}
