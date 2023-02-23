using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionPdv.Ecf;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.MemoriaFiscalEcf
{
    public partial class LeituraMemoriaFiscal
    {
        private readonly LeituraMemoriaFiscalModel _leituraMemoriaFiscalModel;

        public LeituraMemoriaFiscal()
        {
            _leituraMemoriaFiscalModel = new LeituraMemoriaFiscalModel();
            InitializeComponent();
            DataContext = _leituraMemoriaFiscalModel;
        }

        private void LeituraMemoriaFiscal_OnKeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.F2:
                    TirarMemoriaFiscal();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void BtFechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtConfirmar_OnClick(object sender, RoutedEventArgs e)
        {
            TirarMemoriaFiscal();
        }

        private bool ImpressoraPoucoPapel()
        {
            if (!SessaoEcf.EcfFiscal.PoucoPapel) return false;

            var messageBoxPoucoPapel =
                DialogBox.MostraConfirmacao("A impressora está com pouco papel\nDeseja Realmene continuar?");

            return messageBoxPoucoPapel != MessageBoxResult.Yes;
        }

        private async void TirarMemoriaFiscal()
        {
            if (ImpressoraPoucoPapel()) return;

            try
            {
                ProgressBarAgil4.ShowProgressBar();
                await Task.Run(() => _leituraMemoriaFiscalModel.TirarLeituraMemoriaFiscal());

                DialogBox.MostraInformacao(_leituraMemoriaFiscalModel.ESimplificada()
                    ? "Leitura de Memória Fiscal Simplificada\n feita com sucesso."
                    : "Leitura de Memória Fiscal Completa\n feita com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                ProgressBarAgil4.CloseProgressBar();
            }
        }
    }
}
