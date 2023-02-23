
using System.Windows;

namespace Fusion.Visao.MdfeEletronico.NaoEncerrados
{
    public partial class ConsultaNaoEncerradosForm
    {
        private ConsultaNaoEncerradosFormModel _model;

        public ConsultaNaoEncerradosForm()
        {
            _model = new ConsultaNaoEncerradosFormModel();
            DataContext = _model;
            InitializeComponent();
        }

        private void OnClickEncerrarMdfe(object sender, RoutedEventArgs e)
        {
            _model.EncerrarMDFeSelecionado();
        }
    }
}
