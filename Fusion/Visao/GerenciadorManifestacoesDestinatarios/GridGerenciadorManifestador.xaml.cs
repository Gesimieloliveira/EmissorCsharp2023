using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Helpers;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public partial class GridGerenciadorManifestador
    {
        private readonly GridGerenciadorManifestadorModel _model;

        public GridGerenciadorManifestador(GridGerenciadorManifestadorModel model)
        {
            _model = model;
            DataContext = model;
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            ClickOpcoesHandler(sender, e);
        }

        private void ClickOpcoesHandler(object sender, RoutedEventArgs e)
        {
            new ManifestoOpcoes(_model.NfeResumidaSelecionada).ShowDialog();
            _model.AtualizarGrid();
        }

        private void ClickCopyChave(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string chave)
            {
                Clipboard.SetText(chave);
            }
        }


        private void GridGerenciadorManifestador_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.AtualizarInformacoes();
            _model.AtualizarGrid();
        }

        private void Distribuicao_OnClick(object sender, RoutedEventArgs e)
        {
            new EfetuarDistribuicao(new EfetuarDistribuicaoModel()).ShowDialog();
            _model.AtualizarGrid();
        }

        private void AplicarFiltro_OnClick(object sender, RoutedEventArgs e)
        {
            _model.AtualizarGrid();
        }
    }
}
