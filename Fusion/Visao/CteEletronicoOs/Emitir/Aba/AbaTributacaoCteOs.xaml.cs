using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba
{
    public partial class AbaTributacaoCteOs
    {
        public AbaTributacaoCteOs()
        {
            InitializeComponent();
        }

        private AbaCTeOsTributacaoModel ViewModel => DataContext as AbaCTeOsTributacaoModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
        }
    }
}
