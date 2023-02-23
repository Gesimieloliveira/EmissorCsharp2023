using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.EntradaOutras;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sintegra.Dto;

namespace Fusion.Visao.Lancamentos
{
    public class GridLancamentoOutroModel : GridPadraoModel<NfOutroDto>
    {
        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var id = new DataGridTextColumn { Header = "Codigo", Binding = new Binding("Id") {StringFormat = "D11"}, Width = 100};
            colunas.Add(id);

            var nomeFornecedor = new DataGridTextColumn { Header = "Fornecedor", Binding = new Binding("NomeFornecedor"), Width = 350};
            colunas.Add(nomeFornecedor);

            var serie = new DataGridTextColumn { Header = "Série", Binding = new Binding("Serie") { StringFormat = "D3" }, Width = 55};
            colunas.Add(serie);

            var numero = new DataGridTextColumn { Header = "Número", Binding = new Binding("Numero") { StringFormat = "D10" }, Width = 77};
            colunas.Add(numero);

            var emissaoEm = new DataGridTextColumn { Header = "Emissão Em", Binding = new Binding("EmissaoEm"), Width = 116};
            colunas.Add(emissaoEm);

            var recebimentoEm = new DataGridTextColumn { Header = "Recebimento Em", Binding = new Binding("RecebimentoEm"), Width = 123};
            colunas.Add(recebimentoEm);

            var modeloDocumento = new DataGridTextColumn { Header = "Modelo Documento", Binding = new Binding("ModeloDocumentoOutro") };
            colunas.Add(modeloDocumento);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            EfetuaPesquisa(texto);
        }

        private void EfetuaPesquisa(string texto)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                Lista = new RepositorioNfOutro(sessao).Buscar(texto);
            }
        }

        public override Window JanelaNovo()
        {
            return new LancamentoOutroForm(new LancamentoOutroFormModel(new NfOutro()));
        }

        public override Window JanelaAlterar()
        {

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new LancamentoOutroForm(new LancamentoOutroFormModel(new RepositorioNfOutro(sessao).BuscarPorIdLazy(Selecionado.Id)));
            }
        }

        public override void PopularLista()
        {
            EfetuaPesquisa(string.Empty);
        }
    }
}