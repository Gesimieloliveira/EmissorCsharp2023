using System;
using System.Windows;
using System.Windows.Input;
using FusionPdv.Ecf;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.EspelhoMfd
{
    public partial class EspelhoMfdForm
    {
        private readonly EspelhoMfdFormModel _espelhoMfdFormModel;

        public EspelhoMfdForm()
        {
            _espelhoMfdFormModel = new EspelhoMfdFormModel();
            DataContext = _espelhoMfdFormModel;
            InitializeComponent();
        }

        private void LeituraMemoriaFiscal_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    BtConfirmar_OnClick(sender, e);
                    break;

                case Key.Escape:
                    BtFechar_OnClick(sender, e);
                    break;
            }
        }

        private void BtConfirmar_OnClick(object sender, RoutedEventArgs e)
        {
            TirarEspelhoMfd();
        }

        private void BtFechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TirarEspelhoMfd()
        {
            if (ImpressoraPoucoPapel()) return;

            try
            {
                _espelhoMfdFormModel.TirarEspelho();

                DialogBox.MostraInformacao("Espelho Mfd feito com sucesso");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }


        private static bool ImpressoraPoucoPapel()
        {
            if (!SessaoEcf.EcfFiscal.PoucoPapel) return false;

            var messageBoxPoucoPapel =
                DialogBox.MostraConfirmacao("A impressora está com pouco papel\nDeseja Realmene continuar?");

            return messageBoxPoucoPapel != MessageBoxResult.Yes;
        }

    }
}
