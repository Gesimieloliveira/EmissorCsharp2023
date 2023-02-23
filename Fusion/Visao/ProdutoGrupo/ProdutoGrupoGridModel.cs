using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ProdutoGrupo;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.ProdutoGrupo
{
    public class ProdutoGrupoGridModel : GridPadraoModel<ProdutoGrupoDTO>
    {
        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();


            var nome = new DataGridTextColumn {Header = "Nome", Binding = new Binding("Nome")};
            colunas.Add(nome);

            var id = new DataGridTextColumn
            {
                Header = "Código",
                Binding = new Binding("Id")
                {
                    StringFormat = "D4"
                },
                Width = 80
            };
            colunas.Add(id);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            using (var repositorio = new RepositorioComun<ProdutoGrupoDTO>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                var lista = repositorio.Busca(new TodosGrupos());

                if (texto.IsNotNullOrEmpty())
                {
                    var listaFiltrada = lista.Where(g => g.Id.ToString() == texto || g.Nome.Contains(texto.ToUpper())).ToList();
                    Lista = listaFiltrada;
                    return;
                }
                
                Lista = lista;
            }
        }

        public override Window JanelaNovo()
        {
            return new ProdutoGrupoForm(new ProdutoGrupoDTO());
        }

        public override Window JanelaAlterar()
        {
            return new ProdutoGrupoForm(Selecionado);
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioGrupoProduto(sessao);
                var todosGrupos = repositorio.BuscarTodosOrdenado(x => x.Nome);

                Lista = todosGrupos;
            }
        }
    }
}