using System;
using System.Collections.Generic;
using System.IO;
using Fusion.Sessao;
using Fusion.Visao.Produto.OpcoesBusca.GridPicker;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.Produto
{
    public class ProdutoGridPickerModel : GridPickerModel
    {
        private readonly ITabelaPreco _tabelaPreco;
        private ProdutoDTO _produtoBase;

        public ProdutoGridPickerModel(ITabelaPreco tabelaPreco = null)
        {
            _tabelaPreco = tabelaPreco;
            TipoBuscas.Add(new BuscaPadraoProdutoGridPicker());
            TipoBuscas.Add(new BuscaProdutoGridPickerCodigoBarrasAlias());
            BuscaSelecionada = TipoBuscas[0];
            IsTemTipoBuscas = true;

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            var isPermissaoInserirOuAlterar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PRODUTO_INSERIR_ALTERAR);

            HabilitaBotaoNovo = isPermissaoInserirOuAlterar;
            HabilitaBotaoEditar = isPermissaoInserirOuAlterar;
        }

        private void PreecherListaCom(IEnumerable<ProdutoGridPicker> produtos)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (produtos == null)
            {
                throw new InvalidDataException("Nenhum produto para listar");
            }

            produtos.ForEach(produto =>
            {
                produto.PrecoOriginal = produto.PrecoVenda;
            });

            AtualizaPrecoComTabelaPreco(produtos);

            produtos.ForEach(AddProdutoLista);
        }

        private void AtualizaPrecoComTabelaPreco(IEnumerable<ProdutoGridPicker> produtos)
        {
            if (_tabelaPreco == null) return;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioTabelaPreco = new RepositorioTabelaPreco(sessao);

                produtos.ForEach(produto =>
                {
                    produto = (ProdutoGridPicker) new AtualizaPrecoProdutoTabelaPreco(_tabelaPreco)
                        .CalculaProdutoComBaseTabelaPreco(repositorioTabelaPreco, produto);
                });
            }
        }

        private void AddProdutoLista(ProdutoGridPicker produtoGrid)
        {
            var gridPickerItem = new GridPickerItem
            {
                Titulo = produtoGrid.Nome,
                Subtitulo = produtoGrid.Referencia,
                Coluna1 = $"#ID: {produtoGrid.Id}",
                Coluna3 = $"Preço Venda: {produtoGrid.PrecoOriginal:C}",
                Coluna4 = $"Saldo Estoque: {produtoGrid.Estoque:N}",
                ItemReal = produtoGrid
            };

            if (_tabelaPreco != null)
                gridPickerItem.Coluna2 = $"Preço Tabela: {produtoGrid.PrecoVenda:C}";

            ItensLista.Add(gridPickerItem);
        }

        public override void AplicaPesquisa(string input)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                PreecherListaCom(BuscaSelecionada.Listar<ProdutoGridPicker>(input, sessao));
            }
        }

        protected override void OnNovoRegistro()
        {
            var viewModel = new ProdutoFormModel(_produtoBase);
            viewModel.RegistroSalvo += RegistroSalvoHandler;

            new ProdutoForm(viewModel).ShowDialog();
        }

        protected override void OnPickItem(IGridPickerItem item)
        {
            try
            {
                CarregaProdutoReal(item);

                OnPickItemEvent(new GridPickerEventArgs(item));
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private static void CarregaProdutoReal(IGridPickerItem item)
        {
            if (item.ItemReal.GetType() != typeof(ProdutoGridPicker))
            {
                return;
            }

            var produtoGrid = item.GetItemReal<ProdutoGridPicker>();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                var produto = repositorio.GetPeloId(produtoGrid.Id);

                item.ItemReal = produto;
            }
        }

        private void RegistroSalvoHandler(object sender, ProdutoDTO e)
        {
            OnPickItem(new GridPickerItem {ItemReal = e});
        }

        protected override void OnEditarRegistro(IGridPickerItem item)
        {
            CarregaProdutoReal(item);

            var produto = item.ItemReal as ProdutoDTO;

            var viewModel = new ProdutoFormModel(produto.Id);
            viewModel.RegistroSalvo += RegistroSalvoHandler;

            new ProdutoForm(viewModel).ShowDialog();
        }

        public void UsaProdutoBase(ProdutoDTO produto)
        {
            _produtoBase = produto;
        }
    }
}