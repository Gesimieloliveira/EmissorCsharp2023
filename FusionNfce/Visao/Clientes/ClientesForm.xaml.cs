using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Clientes
{
    public partial class ClientesForm
    {
        private readonly ClientesFormModel _model;

        public ClientesForm(ClientesFormModel clienteModel)
        {
            _model = clienteModel;
            DataContext = _model;
            InitializeComponent();
        }

        private void BtConfirmar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarItem();
        }

        private void BtCancelar_OnClick(object sender, RoutedEventArgs e)
        {
            FecharTelaSemSelecionarItem();
        }

        private void TextBoxPesquisa_OnOnSearch(object sender, RoutedEventArgs e)
        {
            _model.BuscaRapida();
        }

        private void FecharTelaSemSelecionarItem()
        {
            _model.ItenSelecionado = null;
            Close();
        }

        private void TextBoxPesquisa_OnOnKeyDown(object sender, RoutedEventArgs e)
        {
            _model.BuscaRapida();
            LbListaDeProdutos.Focus();
        }

        private void LbListaDeProdutos_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    SelecionarItem();
                    break;
            }
        }

        private void SelecionarItem()
        {
            if (_model.ItenSelecionado == null)
            {
                DialogBox.MostraInformacao("Para confirmar porfavor selecionar um produto.");
                return;
            }

            _model.OnRetornaItem();
            Close();
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarItem();
        }

        private void ClientesForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    FecharTelaSemSelecionarItem();
                    break;
                case Key.F2:
                    BtConfirmar_Click(sender, e);
                    break;
            }
        }
    }
}
