using System;
using System.Windows;
using System.Windows.Input;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.Produto
{
    
    public partial class ConsultarProduto
    {

        private readonly ConsultarProdutoModel _consultarProdutoModel;

        public ConsultarProduto()
        {
            InitializeComponent();
            _consultarProdutoModel = new ConsultarProdutoModel();

            DataContext = _consultarProdutoModel;

            TBConsulta.Focus();
        }

        private void TBConsulta_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarProdutoPorNome();
            }
        }

        private void BuscarProdutoPorNome()
        {
            try
            {
                _consultarProdutoModel.ConsultarProdutoPorNome();
                TBConsulta.Focus();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ConsultarProduto_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    BuscarProdutoPorNome();
                    break;
                case Key.F2:
                    Close();
                    break;
                case Key.Escape:
                    _consultarProdutoModel.ProdutoSelecionado = null;
                    Close();
                    break;
            }
        }

        private void BtBuscarProdutoPorNome_Click(object sender, RoutedEventArgs e)
        {
            BuscarProdutoPorNome();
        }

        private void TBConsulta_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsDown || e.Key != Key.Down) return;
            _consultarProdutoModel.ConsultarProdutoPorNome();
            LbListaDeProdutos.Focus();
            _consultarProdutoModel.ProdutoSelecionado = _consultarProdutoModel.PrimeiroItemDaLista;
        }

        public ProdutoDt Retorno()
        {
            return _consultarProdutoModel.ProdutoSelecionado;
        }

        private void LbListaDeProdutos_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Close();
        }

        private void BtConfirmar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtCancelar_OnClick(object sender, RoutedEventArgs e)
        {
            _consultarProdutoModel.ProdutoSelecionado = null;
            Close();
        }

        private void DoubleClickEditarEnderecoHandler(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
