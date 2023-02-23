using System;
using System.Windows;
using System.Windows.Input;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Utils;
using NHibernate.Util;

namespace FusionNfce.Visao.ConsultaProduto
{
    public partial class ConsultaProdutosView
    {
        public ConsultaProdutosView(UltimaBuscaEfetuadaDoDia ultimaBuscaEfetuadaDoDia, TabelaPrecoNfce tabelaPrecoNfce)
        {
            InitializeComponent();
            Contexto = new ConsultaProdutosContexto(new SessaoManagerNfce(), ultimaBuscaEfetuadaDoDia, tabelaPrecoNfce);
            RegistrarAtalho(Key.F6, AcaoFocarTextBoxBusca);
            RegistrarAtalho(Key.Escape, Close);
        }

        public ConsultaProdutosContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
        }

        private void AcaoFocarTextBoxBusca()
        {
            TextBoxPesquisa.Focus();
            TextBoxPesquisa.SelectAll();
        }

        private void TextBoxBuscaKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && Contexto.Produtos.Any())
            {
                e.Handled = true;

                GridProdutos.Focus();
                GridProdutos.SelectedItem = Contexto.Produtos.First();
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
            Contexto.TextoPesquisa = TextBoxPesquisa.Text;
            Contexto.CarregarDadosDosProdutos();

            if (Contexto.Produtos.Any())
            {
                GridProdutos.ScrollIntoView(Contexto.Produtos.First());
            }
        }

        private void TextBoxBuscaClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoExecutarBusca();
        }

        private void DataGridRowKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AcaoSelecionarProduto();
            }
        }

        private void AcaoSelecionarProduto()
        {
            try
            {
                Contexto.Selecionar();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void DataGridRowDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            AcaoSelecionarProduto();
        }

        private void ConsultaProdutosView_OnContentRendered(object sender, EventArgs e)
        {
            AcaoFocarTextBoxBusca();
        }
    }
}