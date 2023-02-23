using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ProdutoUnidade;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.ProdutoUnidade
{
    public class ProdutoUnidadeGridModel : GridPadraoModel<ProdutoUnidadeDTO>
    {
        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var sigla = new DataGridTextColumn {Header = "Sigla", Binding = new Binding(nameof(ProdutoUnidadeDTO.Sigla)), Width = 100};
            colunas.Add(sigla);

            var nome = new DataGridTextColumn {Header = "Nome", Binding = new Binding(nameof(ProdutoUnidadeDTO.Nome))};
            colunas.Add(nome);

            var fracionado = new DataGridTextColumn { Header = "Fracionado", Binding = new Binding(nameof(ProdutoUnidadeDTO.FrancionadoTexto))};
            colunas.Add(fracionado);

            var solicitaTotalPdv = new DataGridTextColumn { Header = "Solicitar Total (PDV)", Binding = new Binding(nameof(ProdutoUnidadeDTO.SolicitaTotalPdvTexto)) };
            colunas.Add(solicitaTotalPdv);

            var solicitarPeso = new DataGridTextColumn { Header = "Solicita Peso (FAT)", Binding = new Binding(nameof(ProdutoUnidadeDTO.SolicitarPesoTexto)) };
            colunas.Add(solicitarPeso);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            if (texto.IsNullOrEmpty())
            {
                PopularLista();
                return;
            }

            using (var repositorio = new RepositorioComun<ProdutoUnidadeDTO>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                Lista = repositorio.Busca(new BuscaRapidaDeUnidades(texto));
            }
        }

        public override Window JanelaNovo()
        {
            return new ProdutoUnidadeForm(new ProdutoUnidadeDTO());
        }

        public override Window JanelaAlterar()
        {
            return new ProdutoUnidadeForm(Selecionado);
        }

        public override void PopularLista()
        {
            using (var repositorio = new RepositorioComun<ProdutoUnidadeDTO>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                Lista = repositorio.Busca(new TodasUnidades());
            }
        }
    }
}
