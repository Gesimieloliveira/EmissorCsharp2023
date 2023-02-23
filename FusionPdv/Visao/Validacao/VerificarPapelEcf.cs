using System;
using System.Windows;
using FusionPdv.Ecf;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.Validacao
{
    public class VerificarPapelEcf
    {
        private readonly bool _abrirDialog;

        public VerificarPapelEcf(bool abrirDialog = false)
        {
            _abrirDialog = abrirDialog;
        }

        public bool Executar()
        {
            try
            {
                if (SessaoEcf.EcfFiscal.PoucoPapel)
                {
                    return !_abrirDialog && AbreDialogConfirmacao();
                }
            }
            catch (Exception ex)
            {
                throw new ImpressoraSemPapelException("Impressora está sem papel.\n" + ex.Message, ex);
            }

            return false;
        }

        private static bool AbreDialogConfirmacao()
        {
            var messageBoxPoucoPapel =
                DialogBox.MostraConfirmacao("Impressora está com pouco papel\nDeseja Realmene continuar?");

            return messageBoxPoucoPapel != MessageBoxResult.Yes;
        }
    }
}
