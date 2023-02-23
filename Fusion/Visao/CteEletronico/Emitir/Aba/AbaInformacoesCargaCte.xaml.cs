using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;

namespace Fusion.Visao.CteEletronico.Emitir.Aba
{
    public partial class AbaInformacoesCargaCte
    {
        public AbaInformacoesCargaCte()
        {
            InitializeComponent();
        }

        private AbaInformacoesCargaCteModel ViewModel => DataContext as AbaInformacoesCargaCteModel;

        private void OnClickBotaoAnterior(object sender, RoutedEventArgs e)
        {
            ViewModel?.Anterior();
        }

        private void ClickAdicionarCargaHandler(object sender, RoutedEventArgs e)
        {
            ViewModel?.OnAdicionarInformacaoCargaCall();
        }

        private void ClickAdicionarVeiculoHandler(object sender, RoutedEventArgs e)
        {
            ViewModel?.OnAdicionarVeiculoNovoCall();
        }

        private void EmitirCteOnClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.EmiteCte();
        }
    }
}