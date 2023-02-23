using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.ProdutoLocalizacoes
{
    public class ProdutoLocalizacaoGridModel : GridPadraoModel<ProdutoLocalizacao>
    {
        public ProdutoLocalizacaoGridModel()
        {
            LabelPesquisaRapida = "Pesquisa por Codigo/ID ou Nome";
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var id = new DataGridTextColumn { Header = "Codigo/ID", Binding = new Binding("Id") {StringFormat = "D11"}, Width = 100 };
            colunas.Add(id);

            var nome = new DataGridTextColumn { Header = "Nome", Binding = new Binding("Nome") };
            colunas.Add(nome);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            if (texto.IsNullOrEmpty())
            {
                PopularLista();
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoLocalizacao(sessao);
                var lista = repositorio.BuscaRapida(texto);
                Lista = lista;
            }
        }

        public override Window JanelaNovo()
        {
            return new ProdutoLocalizacaoForm(new ProdutoLocalizacaoFormModel(new ProdutoLocalizacao()));
        }

        public override Window JanelaAlterar()
        {
            return new ProdutoLocalizacaoForm(new ProdutoLocalizacaoFormModel(Selecionado));
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoLocalizacao(sessao);
                var lista = repositorio.BuscaTodos();
                Lista = lista;
            }
        }
    }
}