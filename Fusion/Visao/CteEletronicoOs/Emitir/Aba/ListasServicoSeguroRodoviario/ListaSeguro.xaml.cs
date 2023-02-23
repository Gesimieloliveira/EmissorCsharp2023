using System.Windows;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.ListasServicoSeguroRodoviario
{
    public partial class ListaSeguro
    {
        public ListaSeguro()
        {
            InitializeComponent();
        }

        private void Excluir_OnClick(object sender, RoutedEventArgs e)
        {
            ((AbaServicoSeguroRodoOsModel) DataContext)?.ExcluirSeguro();
        }
    }
}
