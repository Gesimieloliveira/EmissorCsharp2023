using System.Windows;
using System.Windows.Input;

namespace Fusion.Visao.Fiscal.Estadual
{
    public partial class AliquotaInternaForm
    {
        private readonly AliquotaInternaFormModel _model;

        public AliquotaInternaForm()
        {
            _model = new AliquotaInternaFormModel();
            InitializeComponent();
            DataContext = _model;
        }

        private void NoCliqueEditarAliquotaInterna(object sender, RoutedEventArgs e)
        {
            AlterarAliquotaInterna();
        }

        private void DuploCliqueDataGridRow(object sender, MouseButtonEventArgs e)
        {
            AlterarAliquotaInterna();
        }

        public void AlterarAliquotaInterna()
        {
            new EditarAliquotaInternaForm(_model.AliquotaInternaSelecionada).ShowDialog();
            _model.AtualizarListagem();
        }
    }
}
