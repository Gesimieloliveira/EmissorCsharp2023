using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.EmissorFiscalEletronico.Aba
{
    public partial class AbaEmissorFiscalCTe
    {
        public AbaEmissorFiscalCTe()
        {
            InitializeComponent();
        }

        private void HabilitarEdicaoAmbiente_OnClick(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacaoComMensagemDeConfirmacao("Alterar AMBIENTE",
                "Deseja realmente editar o ambiente?\nATENÇÃO: Ao Editar o Ambiente\n" +
                "Confira a Série, Número Atual\n" +
                "QUANDO EDITA UM AMBIENTE ESSAS INFORMAÇÕES DEVEM SER ATUALIZADAS CONFORME O AMBIENTE SELECIONADO!!!",
                "ALTERAR AMBIENTE"))
            {
                ((EmissorFiscalFormModel)DataContext).EditarAmbiente = true;
                ((EmissorFiscalFormModel)DataContext).MensagemEdicaoAmbiente = true;
            }
        }
    }
}
