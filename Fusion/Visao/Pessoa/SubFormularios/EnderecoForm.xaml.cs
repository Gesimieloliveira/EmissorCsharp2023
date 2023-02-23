using System;
using System.Windows;
using System.Windows.Controls;

namespace Fusion.Visao.Pessoa.SubFormularios
{
    public partial class EnderecoForm
    {
        private readonly EnderecoFormModel _formModel;

        public EnderecoForm(EnderecoFormModel formModel)
        {
            InitializeComponent();
            _formModel = formModel;
            _formModel.Finalizado += FinalizadoHandler;
            DataContext = _formModel;
        }

        private void FinalizadoHandler(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _formModel?.CarregarDados();
        }

        private void ConfirmarHandler(object sender, RoutedEventArgs e)
        {
            _formModel.ConfirmarEndereco();
        }

        private void DeletarHandler(object sender, RoutedEventArgs e)
        {
            _formModel.DeletarEndereco();
        }

        private void FecharHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UfSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            _formModel.LoadCidadesComBaseNoEstado();
        }
    }
}