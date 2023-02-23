using System.Windows.Input;

namespace Fusion.Visao.Compras.Importacao.Contents
{
    public partial class ImportacaoChave
    {
        public ImportacaoChave()
        {
            InitializeComponent();
        }

        private ImportacaoCompraViewModel GetModel => DataContext as ImportacaoCompraViewModel;

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                GetModel?.ConfirmarImportacaoChaveCommand.Execute(sender);
            }
        }
    }
}