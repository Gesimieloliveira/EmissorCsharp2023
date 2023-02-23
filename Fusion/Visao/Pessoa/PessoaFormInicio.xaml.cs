using System.Windows;
using System.Windows.Input;

namespace Fusion.Visao.Pessoa
{
    public partial class PessoaFormInicio
    {
        public PessoaFormInicio()
        {
            InitializeComponent();
        }

        private PessoaFormInicioModel Context => (PessoaFormInicioModel) DataContext;

        private void PessoaFisicaClick(object sender, RoutedEventArgs e)
        {
            Context.PessoaFisicaHandler();
        }

        private void PessoaJuridicaClick(object sender, RoutedEventArgs e)
        {
            Context.PessoaJuridicaHandler();
        }

        private void ConsultaCnpjClick(object sender, RoutedEventArgs e)
        {
            Context.ConsultaCnpjHandler();
        }

        private void ControlKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                Context.PessoaFisicaHandler();
            }

            if (e.Key == Key.F3)
            {
                Context.PessoaJuridicaHandler();
            }

            if (e.Key == Key.F4)
            {
                Context.ConsultaCnpjHandler();
            }
        }
    }
}