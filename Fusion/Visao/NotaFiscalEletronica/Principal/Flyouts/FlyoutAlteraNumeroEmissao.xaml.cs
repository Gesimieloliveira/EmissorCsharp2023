using System.Windows;
using Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts
{
    public partial class FlyoutAlteraNumeroEmissao
    {
        private FlyoutAlteraNumeroEmissaoModel GetModel => DataContext as FlyoutAlteraNumeroEmissaoModel;

        public FlyoutAlteraNumeroEmissao()
        {
            InitializeComponent();
        }

        private void ClickSalvarHandler(object sender, RoutedEventArgs e)
        {
            GetModel.SalvarAlteracao();
        }
    }
}