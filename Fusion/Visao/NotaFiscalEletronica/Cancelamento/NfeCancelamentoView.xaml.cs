using System;
using System.Threading.Tasks;
using System.Windows;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Cancelamento
{
    public partial class NfeCancelamentoView
    {
        private readonly NfeCancelamentoContexto _contexto;

        public NfeCancelamentoView(NfeCancelamentoContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.Inicializa();

            DataContext = _contexto;
            TbJustificativa.Focus();
        }

        private void FazerCancelamentoClickHandler(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();

            Task.Run(() =>
            {
                try
                {
                    _contexto.FazCancelamento();

                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraInformacao("Cancelamento feito com sucesso");
                        Close();
                    });
                }
                catch (InvalidOperationException ex)
                {
                    Dispatcher.Invoke(() => DialogBox.MostraAviso(ex.Message));
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => DialogBox.MostraErro("Falha ao cancelar o documento", ex));
                }
                finally
                {
                    Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                }
            });
        }
    }
}
