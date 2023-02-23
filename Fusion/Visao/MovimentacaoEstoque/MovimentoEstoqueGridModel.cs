using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Estoque.Movimentacoes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.MovimentacaoEstoque
{
    public class MovimentoEstoqueGridModel : GridPadraoModel<MovimentoEstoque>
    {

        public MovimentoEstoqueGridModel()
        {
            LabelPesquisaRapida = "Busca por ID e Descrição";
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>
            {
                CriaColuna("ID", new Binding("Id") {StringFormat = "D11"}, 100),
                CriaColuna("Descrição", new Binding("Descricao"), DataGridLength.Auto),
                CriaColuna("Tipo Evento", new Binding("TipoEvento")),
                CriaColuna("Total P.Compra", new Binding("PrecoCompraTotal") {StringFormat = "N2"}),
                CriaColuna("Total P.Venda", new Binding("PrecoVendaTotal") {StringFormat = "N2"}),
                CriaColuna("Cadastrado em", new Binding("CadastradoEm"))
            };

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            using (var repositorio = new RepositorioMovimentoEstoque(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                if (string.IsNullOrEmpty(texto))
                {
                    Lista = repositorio.BuscaTodos();
                }

                Lista = repositorio.BuscaRapida(texto);
            }
        }

        public override Window JanelaNovo()
        {
            return new MovimentoEstoqueForm(new MovimentoEstoqueFormModel());
        }

        public override Window JanelaAlterar()
        {
            return new MovimentoEstoqueForm(new MovimentoEstoqueFormModel(Selecionado.Id));
        }

        public override void PopularLista()
        {
            AplicarPesquisa(string.Empty);
        }
    }
}