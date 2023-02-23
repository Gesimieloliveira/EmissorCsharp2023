using System;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.ConsultaProdutoRapida
{
    public partial class ConsultaProdutoRapidaForm
    {
        private readonly ConsultaProdutoRapidaFormModel _fromModel;

        public ConsultaProdutoRapidaForm(ConsultaProdutoRapidaFormModel consultaProdutoRapidaFormModel)
        {
            _fromModel = consultaProdutoRapidaFormModel;
            DataContext = _fromModel;
            InitializeComponent();

            RegistrarAtalho(Key.Escape, Close);
        }

        private void BuscarProduto_Click(object sender, RoutedEventArgs e)
        {
            AcaoBuscarProduto();
        }

        private void AcaoBuscarProduto()
        {
            try
            {
                if (string.IsNullOrEmpty(_fromModel.CodigoBarra))
                {
                    return;
                }

                _fromModel.BuscarProduto();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                FocarTextBoxBusca();
            }
        }

        private void FocarTextBoxBusca()
        {
            TextBoxCodigoBarra.Focus();
            TextBoxCodigoBarra.SelectAll();
        }

        private void CodigoBarra_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AcaoBuscarProduto();
            }
        }
    }
}
