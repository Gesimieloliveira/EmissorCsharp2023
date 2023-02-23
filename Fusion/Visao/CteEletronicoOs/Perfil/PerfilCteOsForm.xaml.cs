using System;
using System.Windows;

namespace Fusion.Visao.CteEletronicoOs.Perfil
{
    public partial class PerfilCteOsForm
    {
        private readonly PerfilCteOsFormModel _model;

        public PerfilCteOsForm(PerfilCteOsFormModel model)
        {
            _model = model;
            _model.Fechar += Fechar;
            DataContext = _model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void CteOSForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Loadded();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            _model.Salvar();
        }

        private void OnClickFechar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
