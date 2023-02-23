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
    public class GridCteEntrada : GridPadraoModel<NfCteEntradaDto>
    {
        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var id = new DataGridTextColumn { Header = "Codigo", Binding = new Binding("Id") { StringFormat = "D11" }, Width = 100 };
            colunas.Add(id);

            var nomeEmpresa = new DataGridTextColumn { Header = "Tomador/Empresa", Binding = new Binding("NomeEmpresa"), Width = 350 };
            colunas.Add(nomeEmpresa);

            var cnpjEmpresa = new DataGridTextColumn { Header = "Cnpj", Binding = new Binding("CnpjEmpresa"), Width = 110 };
            colunas.Add(cnpjEmpresa);

            var serie = new DataGridTextColumn { Header = "Série", Binding = new Binding("Serie") {StringFormat = "D3"}, Width = 60 };
            colunas.Add(serie);

            var numero = new DataGridTextColumn { Header = "Número", Binding = new Binding("Numero") {StringFormat = "D10"}, Width = 90 };
            colunas.Add(numero);

            var subSerie = new DataGridTextColumn { Header = "Subsérie", Binding = new Binding("Subserie") {StringFormat = "D3"}, Width = 100 };
            colunas.Add(subSerie);

            var cfop = new DataGridTextColumn { Header = "Cfop", Binding = new Binding("Cfop"), Width = 60 };
            colunas.Add(cfop);

            var emissaoEm = new DataGridTextColumn { Header = "Emissão Em", Binding = new Binding("EmissaoEm"), Width = 200 };
            colunas.Add(emissaoEm);

            var utilizacaoEm = new DataGridTextColumn { Header = "Utilização Em", Binding = new Binding("UtilizacaoEm"), Width = 200 };
            colunas.Add(utilizacaoEm);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            EfetuaPesquisa(texto);
        }

        public override Window JanelaNovo()
        {
            return new LancamentoCteEntradaForm(new LancamentoCteEntradaFormModel(new NfCteEntrada()));
        }

        public override Window JanelaAlterar()
        {
            var nfCteEntrada = BuscarNfCteEntradaPorId();

            return new LancamentoCteEntradaForm(new LancamentoCteEntradaFormModel(nfCteEntrada));
        }

        private NfCteEntrada BuscarNfCteEntradaPorId()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioNfCteEntrada(sessao).GetPeloId(Selecionado.Id);
            }
        }

        public override void PopularLista()
        {
            EfetuaPesquisa(string.Empty);
        }

        private void EfetuaPesquisa(string texto)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                Lista = new RepositorioNfCteEntrada(sessao).Buscar(texto);
            }
        }
    }
}