using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ControlarNfces
{
    public partial class ResolverNfceFaturamentosForm
    {
        private readonly ResolverNfceFaturamentosFormModel _model;

        public ResolverNfceFaturamentosForm(ResolverNfceFaturamentosFormModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = _model;
        }

        private async void EnviarTodasNfce_Click(object sender, RoutedEventArgs e)
        {
            if (_model.NovaDataEmissao != null)
            {
                if (!DialogBox.MostraConfirmacao(
                    $"Você está alterando a data de emissão de todas NFC-e\n para {_model.NovaDataEmissao}\n" +
                    $"AVISO IMPORTANTE - As CHAVES DAS NFC-E SERÃO ALTERADAS !!!\n" +
                    $"Deseja realmente realizar essa operação?",
                    MessageBoxImage.Question))
                    return;
            }

            await RunTaskWithProgress(() => _model.EnviarTodasNFCe());

            Close();
        }
    }
}
