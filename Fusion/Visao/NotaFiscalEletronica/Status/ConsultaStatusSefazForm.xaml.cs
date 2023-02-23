using System;
using System.Threading.Tasks;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Status
{
    public partial class ConsultaStatusSefazForm
    {
        private readonly ConsultaStatusSefazFormModel _contexto;

        public ConsultaStatusSefazForm(ConsultaStatusSefazFormModel contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.CarregarDadosIniciais();

            DataContext = _contexto;
        }

        private async void VerificarStatusClickHandler(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            await Task.Run(() =>
            {
                try
                {
                    var resposta = _contexto.VerificarStatus();

                    DialogBox.MostraInformacao(resposta.XMotivo);
                }
                catch (Exception ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                }
                finally
                {
                    Dispatcher.Invoke(() => { IsEnabled = true; });
                }
            });
        }
    }
}
