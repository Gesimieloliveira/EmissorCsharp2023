using System.Windows;
using FusionWPF.Base.Utils.Dialogs;
using CentroDeCusto = FusionCore.FusionAdm.Financeiro.CentroCusto;

namespace Fusion.Visao.CentroCusto
{
    public partial class CentroCustoForm
    {
        private readonly CentroCustoFormModel _centroCustoFormModel;

        public CentroCustoForm(CentroDeCusto centroCusto)
        {
            _centroCustoFormModel = new CentroCustoFormModel(centroCusto);
            DataContext = _centroCustoFormModel;
            InitializeComponent();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs routedEventArgs)
        {
            _centroCustoFormModel.Salvar();
            DialogBox.MostraMensagemSalvouComSucesso();
            Close();
        }

        private void OnClickFechar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            _centroCustoFormModel.Deletar();
            Close();
        }
    }
}
