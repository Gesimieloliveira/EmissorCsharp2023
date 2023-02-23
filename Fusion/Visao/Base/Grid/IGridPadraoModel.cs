using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Base.Grid
{
    public interface IGridPadraoModel
    {
        string LabelPesquisaRapida { get; set; }
        bool EditaRegistro { get; set; }
        bool MostraBotaoNovo { get; set; }
        bool MostraPesquisaRapida { get; set; }
        bool MostraBotaoOpcoes { get; set; }
        string UltimoTextoPesquisado { get; set; }
        bool AutoReload { get; }
        ObservableCollection<DataGridColumn> ColunasDaGrid();
        void AplicarPesquisa(string texto);
        Window JanelaNovo();
        ChildWindow JanelaFiltro();
        Window JanelaAlterar();
        Window JanelaOpcoes();
        void PopularLista();
        void Loaded();
    }
}