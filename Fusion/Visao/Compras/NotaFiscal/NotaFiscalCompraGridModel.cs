using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public class NotaFiscalCompraGridModel : GridPadraoModel<NotaFiscalCompraGrid>
    {
        public NotaFiscalCompraGridModel()
        {
            LabelPesquisaRapida = "Pesquisa rápida por Número ou Codigo/ID";
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var bTotalItens = new Binding(nameof(NotaFiscalCompraGrid.TotalItens)) {StringFormat = "N2"};
            var bValorTotal = new Binding(nameof(NotaFiscalCompraGrid.ValorTotal)) {StringFormat = "N2"};
            var bValorTotalBcIcms = new Binding(nameof(NotaFiscalCompraGrid.TotalBcIcms)) { StringFormat = "N2" };
            var valorTotalIcms = new Binding(nameof(NotaFiscalCompraGrid.ValorTotalIcms)) { StringFormat = "N2" };
            var bValorTotalBcIcmsSt = new Binding(nameof(NotaFiscalCompraGrid.TotalBcIcmsSt)) { StringFormat = "N2" };
            var valorTotalIcmsSt = new Binding(nameof(NotaFiscalCompraGrid.ValorTotalIcmsSt)) { StringFormat = "N2" };
            var valorTotalIpi = new Binding(nameof(NotaFiscalCompraGrid.ValorTotalIpi)) { StringFormat = "N2" };
            var valorTotalFrete = new Binding(nameof(NotaFiscalCompraGrid.ValorTotalFrete)) { StringFormat = "N2" };
            var valorTotalSeguro = new Binding(nameof(NotaFiscalCompraGrid.ValorTotalSeguro)) { StringFormat = "N2" };
            var valorTotalDesconto = new Binding(nameof(NotaFiscalCompraGrid.ValorTotalDesconto)) { StringFormat = "N2" };
            var valorTotalOutros = new Binding(nameof(NotaFiscalCompraGrid.ValorTotalOutros)) { StringFormat = "N2" };

            return new ObservableCollection<DataGridColumn>
            {
                CriaColuna("Emissão Em", nameof(NotaFiscalCompraGrid.EmissaoEm), 150),
                CriaColuna("Numero", nameof(NotaFiscalCompraGrid.Numero), 100),
                CriaColuna("Série", nameof(NotaFiscalCompraGrid.Serie), 100),
                CriaColuna("Fornecedor", nameof(NotaFiscalCompraGrid.NomeFornecedor), 200),
                CriaColuna("Empresa", nameof(NotaFiscalCompraGrid.NomeEmpresa), 200),
                CriaColunaDinheiroReal("Total Itens", bTotalItens, 100),
                CriaColunaDinheiroReal("Total", bValorTotal, 100),
                CriaColunaDinheiroReal("Total Bc Icms", bValorTotalBcIcms, 100),
                CriaColunaDinheiroReal("Total Icms", valorTotalIcms, 100),
                CriaColunaDinheiroReal("Total Bc Icms St", bValorTotalBcIcmsSt, 115),
                CriaColunaDinheiroReal("Total Icms St", valorTotalIcmsSt, 100),
                CriaColunaDinheiroReal("Total Ipi", valorTotalIpi, 100),
                CriaColunaDinheiroReal("Total Frete", valorTotalFrete, 100),
                CriaColunaDinheiroReal("Total Seguro", valorTotalSeguro, 100),
                CriaColunaDinheiroReal("Total Desconto", valorTotalDesconto, 115),
                CriaColunaDinheiroReal("Total Outros", valorTotalOutros, 100),
                CriaColuna("Entrada Em", nameof(NotaFiscalCompraGrid.EntradaEm), 150),
                CriaColuna("Chave", nameof(NotaFiscalCompraGrid.Chave), 310),
                CriaColuna("Codigo/ID", new Binding(nameof(NotaFiscalCompraGrid.Id)) {StringFormat = "D11"}, 100)
            };
        }

        public override void AplicarPesquisa(string texto)
        {
            GetData(texto);
        }

        private void GetData(string filtroRapido = null)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNotaFiscalCompra(sessao);
                var compras = repositorio.BuscarParaGrid(filtroRapido);

                Lista = new ObservableCollection<NotaFiscalCompraGrid>(compras);
            }
        }

        public override Window JanelaNovo()
        {
            return new NotaFiscalCompraView();
        }

        public override Window JanelaAlterar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNotaFiscalCompra(sessao);
                var compra = repositorio.GetPeloId(Selecionado.Id);

                return new NotaFiscalCompraView(compra);
            }
        }

        public override void PopularLista()
        {
            AplicarPesquisa(UltimoTextoPesquisado);
        }
    }
}
