using System.Windows;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public partial class TransmitirNfeOpcoes
    {
        public TransmitirNfeOpcoes()
        {
            InitializeComponent();
        }

        private void ClickBotaoEditar(object sender, RoutedEventArgs e)
        {
            //new NfeletronicaForm(BuscarNfe()).ShowDialog();
            Close();
        }

        private void ClickBotaoCancelar(object sender, RoutedEventArgs e)
        {
            //new NfeCancelarForm(_nfe).ShowDialog();
            Close();
        }

        private void ClickBotaoCartaCorrecao(object sender, RoutedEventArgs e)
        {
            //new NfeCartaCorrecaoForm(_nfe).ShowDialog();
            Close();
        }
    }
}