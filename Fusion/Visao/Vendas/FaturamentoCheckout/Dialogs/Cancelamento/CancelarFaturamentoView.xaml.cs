using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Cancelamento
{
    public partial class CancelarFaturamentoView
    {
        private readonly CancelarFaturamentoViewModel _modelo;

        public CancelarFaturamentoView(CancelarFaturamentoViewModel modelo)
        {
            InitializeComponent();
            _modelo = modelo;
        }

        private async void FaturamentoCancelamentoForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            await RunTaskWithProgress(() =>
            {
                _modelo.Iniciar();
            });

            DataContext = _modelo;
            Keyboard.Focus(TbJustificativa);
        }

        private async void CancelarFaturamentoView_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                Close();
                return;
            }

            if (e.Key == Key.F2)
            {
                e.Handled = true;
                await ExecutarCancelamento();
                return;
            }
        }

        private async void FazerCancelamentoClickHandler(object sender, RoutedEventArgs e)
        {
            await ExecutarCancelamento();
        }

        private async Task ExecutarCancelamento()
        {
            await RunTaskWithProgress(() =>
            {
                try
                {
                    _modelo.Cancelar();
                    Dispatcher.Invoke(_modelo.OnCancelou);
                }
                catch (InvalidOperationException ex)
                {
                    DialogBox.MostraInformacao(ex.Message);
                }
            });
        }
    }
}
