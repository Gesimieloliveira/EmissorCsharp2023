using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using Fusion.Visao.Pessoa.SubFormularios;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.Veiculos
{
    public class VeiculoGridModel : GridPadraoModel<Veiculo>
    {
        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var tipoProprietario = new DataGridTextColumn
            {
                Header = "Tipo",
                Binding = new Binding(nameof(Veiculo.TipoProprietario)),
                Width = 120
            };

            colunas.Add(tipoProprietario);

            var descricao = new DataGridTextColumn {Header = "Descrição", Binding = new Binding("Descricao")};
            colunas.Add(descricao);

            var placa = new DataGridTextColumn {Header = "Placa", Binding = new Binding("Placa"), Width = 100};
            colunas.Add(placa);

            var siglaUf = new DataGridTextColumn {Header = "UF", Binding = new Binding("SiglaUf"), Width = 50};
            colunas.Add(siglaUf);

            var id = new DataGridTextColumn {Header = "Codigo/ID", Binding = new Binding("Id") {StringFormat = "D11"}, Width = 100};
            colunas.Add(id);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioVeiculo(sessao);
                Lista = repositorio.BuscaTodosParaGrid(texto);
            }
        }

        public override Window JanelaNovo()
        {
            return new VeiculoForm(new VeiculoFormModel());
        }

        public override Window JanelaAlterar()
        {
            return new VeiculoForm(new VeiculoFormModel(Selecionado));
        }

        public override void PopularLista()
        {
            AplicarPesquisa(UltimoTextoPesquisado);
        }
    }
}