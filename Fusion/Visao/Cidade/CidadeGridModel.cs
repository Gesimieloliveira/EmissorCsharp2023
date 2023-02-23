using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Cidade;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Helpers.Hidratacao;

namespace Fusion.Visao.Cidade
{
    public class CidadeGridModel : GridPadraoModel<CidadeDTO>
    {
        public CidadeGridModel()
        {
            MostraBotaoNovo = false;
            EditaRegistro = false;
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var nome = new DataGridTextColumn {Header = "Nome da cidade", Binding = new Binding("Nome")};
            colunas.Add(nome);

            var codigoIbge = new DataGridTextColumn
            {
                Header = "Código ibge",
                Binding = new Binding("CodigoIbge"),
                Width = 100
            };

            colunas.Add(codigoIbge);

            var siglaUf = new DataGridTextColumn {Header = "Uf", Binding = new Binding("SiglaUf"), Width = 100};
            colunas.Add(siglaUf);

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
                var repositorio = new RepositorioComun<CidadeDTO>(sessao);
                Lista = repositorio.Busca(new BuscaRapidaDeCidades(texto));
            }
        }

        public override Window JanelaNovo()
        {
            return null;
        }

        public override Window JanelaAlterar()
        {
            return null;
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<CidadeDTO>(sessao);
                Lista = repositorio.Busca(new TodasCidades());
            }
        }
    }
}