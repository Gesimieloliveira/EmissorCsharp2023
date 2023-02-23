using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TerminalOffline;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Visao.Terminal
{
    public class TerminalOfflineGridModel : GridPadraoModel<TerminalOffline>
    {
        private IList<TerminalOffline> _terminalOfflines; 

        public TerminalOfflineGridModel()
        {
            LabelPesquisaRapida = "Pesquisa com descrição/id";
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var id = new DataGridTextColumn { Header = "Id", Binding = new Binding("Id"), Width = 100 };
            colunas.Add(id);

            var descricao = new DataGridTextColumn { Header = "Descricao", Binding = new Binding("Descricao") };
            colunas.Add(descricao);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            texto = texto ?? string.Empty;
            Lista = _terminalOfflines.Where(t => t.Descricao.Contains(texto) || t.Id.ToString().Equals(texto)).ToList();
        }

        public override Window JanelaNovo()
        {
            return new TerminalOfflineForm(new TerminalOffline
            {
                Ativo = true
            });
        }

        public override Window JanelaAlterar()
        {
            return new TerminalOfflineForm(Selecionado);
        }

        public override void PopularLista()
        {
            var repositorio = new RepositorioTerminalOffline(SessaoHelperFactory.AbrirSessaoAdm());
            using (repositorio)
            {
                var todos = repositorio.BuscaTodos();

                _terminalOfflines = new List<TerminalOffline>(todos);
                Lista = todos;
            }
        }
    }
}
