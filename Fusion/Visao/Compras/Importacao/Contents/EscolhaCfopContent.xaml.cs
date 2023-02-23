using System.Windows.Input;

namespace Fusion.Visao.Compras.Importacao.Contents
{
    public partial class EscolhaCfopContent
    {
        public EscolhaCfopContent()
        {
            InitializeComponent();
        }

        private ImportacaoCompraViewModel Contexto => DataContext as ImportacaoCompraViewModel;

        private void EscolhaCfopContent_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                Contexto?.ConfirmarRegrasCommand.Execute(this);
            }
        }
    }
}