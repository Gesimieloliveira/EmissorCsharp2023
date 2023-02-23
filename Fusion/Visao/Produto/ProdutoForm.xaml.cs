using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.ProdutoGrupo;
using Fusion.Visao.ProdutoLocalizacoes;
using Fusion.Visao.ProdutoUnidade;
using Fusion.Visao.TabelasPrecos;
using Fusion.Visao.Tributacoes.Regras;
using FusionCore.FusionAdm.Produtos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Regras;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.Produto
{
    public partial class ProdutoForm
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();

        internal ProdutoForm(ProdutoFormModel viewModel)
        {
            DataContext = viewModel;
            ViewModel.CloseRequest += (sender, args) => Close();

            InitializeComponent();
        }

        private ProdutoFormModel ViewModel => DataContext as ProdutoFormModel;

        private void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Inicializar();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.SalvarModel();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickAddGrupo(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = new ProdutoGrupoForm(new ProdutoGrupoDTO());
                form.ShowDialog();
                ViewModel.AtualizarListaGrupo();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickAddUnidade(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = new ProdutoUnidadeForm(new ProdutoUnidadeDTO());
                form.ShowDialog();
                ViewModel.AtualizarListaUnidade();
                ViewModel.AtualizarListaUnidadeTributavel();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickAddLocalizacao(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = new ProdutoLocalizacaoForm(new ProdutoLocalizacaoFormModel(new ProdutoLocalizacao()));
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClickNovaRegraHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.NovaRegraTributacao();
        }

        private void DoubleClickRegraEstadoHandler(object sender, MouseButtonEventArgs e)
        {
            ViewModel.EditarRegaEstadoSelecionada();
        }

        private void ClickNovoCodigoBarraHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.AbrirFlyoutNovoAlias();
        }

        private void TentaBuscarNcm_OnKeyDown(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                ViewModel.LoadNcm();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void ClickClonarHandler(object sender, RoutedEventArgs e)
        {
            var clone = ViewModel.ClonaProduto();
            var viewModel = new ProdutoFormModel(clone);

            Dispatcher.BeginInvoke(new Action(() => { }));
            new ProdutoForm(viewModel).ShowDialog();
            Close();
        }

        private void TabControlChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            var tabItem = e.AddedItems.FirstOrNull() as TabItem;

            if (Equals(tabItem?.Tag, "historico_compra"))
            {
                ViewModel.CarregarHistoricoCompras();
            }
        }

        private void CriarRegraClickHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new RegraTributacaoSaidaContexto(_sessaoManager);
            var view = new RegraTributacaoSaidaView(contexto);

            contexto.SalvoSucesso += (o, regra) =>
            {
                ViewModel.IcmsModel.CarregaRegrasTributacao();
                ViewModel.IcmsModel.RegraSelecionada = RegraTributacaoSaidaSlim.From(regra);
            };

            view.ShowDialog();
        }

        private void DeletarAliasClickHandler(object sender, RoutedEventArgs e)
        {
            var confirmacao = DialogBox.MostraConfirmacao("Deseja continuar a exclusão desse código?");

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                ViewModel.DeletarAlias();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void RowCodigoDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            ViewModel.FlyoutCodigoBarraModel = new FlyoutCodigoBarraModel { IsOpen = true };
            ViewModel.FlyoutCodigoBarraModel.Edicao(ViewModel.ProdutoAliasSelecionado);

            ViewModel.FlyoutCodigoBarraModel.Confirmou += (o, alias) =>
            {
                ViewModel.SalvarCodigoEditado(alias);
                ViewModel.FlyoutCodigoBarraModel = null;
            };
        }

        private void RowDoubleClickTabelaPrecoSelecionadaHandler(object sender, MouseButtonEventArgs e)
        {

            var tabela = ViewModel.BuscarTabelaSelecionada();
            var model = new TabelaPrecoFormularioModel(tabela);
            new TabelaPrecoFormulario(model).ShowDialog();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                ViewModel.LoadTabelaPreco(sessao);
            }

        }
    }
}
