using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionNfce.Visao.BarraDeProgresso;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.RecuperarVenda
{
    public partial class RecuperarVendaForm
    {
        private readonly RecuperarVendaFormModel _model;

        public RecuperarVendaForm(RecuperarVendaFormModel model)
        {
            _model = model;
            DataContext = _model;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _model.Inicializar();
        }

        private void TextBoxPesquisa_OnOnSearch(object sender, RoutedEventArgs e)
        {
            _model.AplicarPesquisaRapida();
        }

        private void TextBoxPesquisa_OnOnKeyDown(object sender, RoutedEventArgs e)
        {
            _model.AplicarPesquisaRapida();
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

        private async void SelecionarItem()
        {
            if (_model.ItemSelecionado == null)
            {
                DialogBox.MostraInformacao("Para confirmar porfavor selecionar uma nfc-e.");
                return;
            }

            if (_model.ItemSelecionado.TipoEmissao == TipoEmissao.ContigenciaOfflineNFCe 
                && _model.ItemSelecionado.Status == Status.Aberta
                && _model.ItemSelecionadoTemHistorio())
            {
                DialogBox.MostraInformacao("Esta NFC-e foi emitida offline, sincronizar ela manualmente.");
                return;
            }

            ProgressBarAgil4.ShowProgressBar();

            await Task.Run(() => { _model.OnRetornaNfce(); });
            ProgressBarAgil4.CloseProgressBar();
            Close();
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarItem();
        }

        private void BtConfirmar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarItem();
        }

        private void BtCancelar_OnClick(object sender, RoutedEventArgs e)
        {
            FecharTelaSemSelecionarItem();
        }

        private void FecharTelaSemSelecionarItem()
        {
            _model.ItemSelecionado = null;
            Close();
        }

        private void RecuperarVendaForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    SelecionarItem();
                    break;
                case Key.Escape:
                    FecharTelaSemSelecionarItem();
                    break;
            }
        }       
    }
}
