using System;
using System.Windows;

namespace Fusion.Visao.Pessoa.SubFormularios
{
    public partial class TelefoneForm
    {
        private readonly TelefoneFormModel _formModel;

        public TelefoneForm(TelefoneFormModel formModel)
        {
            InitializeComponent();

            _formModel = formModel;
            _formModel.Finalizado += FinalizadoHandler;
            DataContext = formModel;
        }

        private void FinalizadoHandler(object sender, EventArgs e)
        {
            Close();
        }

        private void OnClickConfirmar(object sender, RoutedEventArgs e)
        {
            _formModel.ConfirmarTelefone();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _formModel?.CarregarDados();
        }
    }
}