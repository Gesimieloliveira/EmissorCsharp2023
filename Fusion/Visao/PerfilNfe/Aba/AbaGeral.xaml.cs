using System.Windows;
using Fusion.Visao.EmissorFiscalEletronico;

namespace Fusion.Visao.PerfilNfe.Aba
{
    public partial class AbaGeral
    {
        public AbaGeral()
        {
            InitializeComponent();
        }

        private void OnClickAddEmissorFiscal(object sender, RoutedEventArgs e)
        {
            new EmissorFiscalForm(EmissorFiscalForm.CriaModel()).ShowDialog();

            var model = DataContext as PerfilNfeFormModel;

            model?.CarregaListasParaComboBox();
        }
    }
}