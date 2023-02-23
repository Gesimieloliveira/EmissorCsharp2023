using System.Windows;
using FusionCore.Tributacoes.Estadual;

namespace Fusion.Visao.Fiscal.Estadual
{
    public partial class EditarAliquotaInternaForm
    {
        private readonly EditarAliquotaInternaFormModel _model;

        public EditarAliquotaInternaForm(AliquotaInterna aliquotaInterna)
        {
            _model = new EditarAliquotaInternaFormModel(aliquotaInterna);
            _model.Fechar += (sender, args) => Close();
            InitializeComponent();
            DataContext = _model;
        }

        private void NoCliqueSalvar(object sender, RoutedEventArgs e)
        {
            _model.Salvar();
        }
    }
}
