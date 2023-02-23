using System;
using System.Windows;
using FusionCore.Helpers.Basico;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAReceber
{
    public partial class QuitarSelecionadosView
    {
        private readonly QuitarSelecionadosContexto _contexto;

        public QuitarSelecionadosView(QuitarSelecionadosContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
            TbValorRecebimento.Focus();
        }

        private void FazerQuitacaoClickHandler(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraDialogoDeConfirmacao(
                    $"Continuar com o recebimento em {_contexto.TipoRecebimento.GetDescription()} de {_contexto.ValorRecebimento:C2} ?"))
            {
                return;
            }

            try
            {
                _contexto.FazerRecebimento();

                DialogBox.MostraInformacao("Pagamento efetuado com sucesso!");
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}