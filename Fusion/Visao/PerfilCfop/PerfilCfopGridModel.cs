using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.PerfilCfop;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.PerfilCfop
{
    public class PerfilCfopGridModel : GridPadraoModel<PerfilCfopDTO>
    {
        private IList<PerfilCfopDTO> _cache;

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var id = new DataGridTextColumn { Header = "Codigo/ID", Binding = new Binding("Id") {StringFormat = "D11"}, Width = 100 };
            colunas.Add(id);

            var codigo = new DataGridTextColumn { Header = "Código", Binding = new Binding("Codigo"), Width = 150};
            colunas.Add(codigo);

            var descricao = new DataGridTextColumn { Header = "Descrição", Binding = new Binding("Descricao") };
            colunas.Add(descricao);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                Lista = _cache.ToList();
                return;
            }

            var listaFiltrada = _cache.Where(g => g.Descricao.Contains(texto) || g.Codigo == texto).ToList();
            Lista = listaFiltrada;
        }

        public override Window JanelaNovo()
        {
            return new PerfilCfopForm(new PerfilCfopDTO {Ativo = true});
        }

        public override Window JanelaAlterar()
        {
            return new PerfilCfopForm(Selecionado);
        }

        public override void PopularLista()
        {
            using (var repositorio = new RepositorioComun<PerfilCfopDTO>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                var lista = repositorio.Busca(new TodosPerfilCfop());
                _cache = lista;
                Lista = _cache;
            }
        }
    }
}
