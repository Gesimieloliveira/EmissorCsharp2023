using System;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.Produto
{
    public partial class ConsultarPrecoProduto
    {
        private readonly ConsultarPrecoProdutoModel _consultarPrecoProdutoModel;

        public ConsultarPrecoProduto()
        {
            InitializeComponent();
            _consultarPrecoProdutoModel = new ConsultarPrecoProdutoModel();
            DataContext = _consultarPrecoProdutoModel;

            TbCodigoBarra.Focus();
        }

        private void CodigoBarra_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarPorCodigoBarras();
            }
        }

        private void ConsultarPrecoProduto_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.F5:
                    BuscarPorCodigoBarras();
                    break;
                case Key.F6:
                    Close();
                    new ConsultarProduto().ShowDialog();
                    break;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BuscarPorCodigoBarras();
        }

        private void BuscarPorCodigoBarras()
        {
            try
            {
                _consultarPrecoProdutoModel.BuscarPorCodigoBarras();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message);
            }
            
            TbCodigoBarra.Focus();
        }

        private void TbBuscaPorNome_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
            new ConsultarProduto().ShowDialog();
        }
    }
}
