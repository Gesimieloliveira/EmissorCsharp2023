using System;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.FinalizarVenda.AlteraTotais
{
    public partial class AdicionaDescontoOuAcrescimoForm 
    {
        public AdicionaDescontoOuAcrescimoForm()
        {
            InitializeComponent();
            
        }

        private void EnviarNovoTotal_OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var model = DataContext as AdicionaDescontoOuAcrescimoFormModel;

                model?.OnEnviarDescontoOuAcrescimo();
            }
            catch (OverflowException)
            {
                DialogBox.MostraInformacao("Valor está muito grande");
            }
            catch (FormatException)
            {
                DialogBox.MostraInformacao("Porfavor digitar valores somente com uma virgula ex: 10,50 ou 1009,20 ou 10000,53");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
