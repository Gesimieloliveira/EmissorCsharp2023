using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Helpers;
using Fusion.Sessao;
using Fusion.Visao.ExportacaoBalanca;
using Fusion.Visao.ExportacaoBuscaRapida;
using FusionCore.Papeis.Enums;

namespace Fusion.Visao.Produto
{
    public partial class ProdutoGridControl
    {
        private ProdutoGridModel GridModel => DataContext as ProdutoGridModel;

        public ProdutoGridControl()
        {
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                SessaoSistema.Instancia.UsuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.CADASTRO_PRODUTO_LISTAR);

                GridModel?.Inicializar();
                FiltroNomeProduto.Focus();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private void DoubleClickRowHandler(object sender, MouseButtonEventArgs e)
        {
            GridModel.AlteraSelecionado();
        }

        private void ClickNovoHandler(object sender, RoutedEventArgs e)
        {
            GridModel.NovoProduto();
        }

        private void ClickOpcoesHandler(object sender, RoutedEventArgs e)
        {
            GridModel.OpcoesProduto();
        }

        private void ExportacaoBalancaClickHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new ExportacaoBalancaContexto();
            var view = new ExportacaoBalancaView(contexto);

            view.ShowDialog();
        }

        private void AplicarFiltroManipulador(object sender, RoutedEventArgs e)
        {
            GridModel.AplicaPesquisa();
        }

        private void ExportacaoBuscaRapidaClickHandler(object sender, RoutedEventArgs e)
        {
            var model = new ExportacaoBuscaRapidaFormModel();
            var form = new ExportacaoBuscaRapidaForm(model);

            form.ShowDialog();
        }
    }
}
