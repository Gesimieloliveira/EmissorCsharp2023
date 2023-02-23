using System;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.FinalizarVenda.CartoesPos
{
    public partial class CartaoPosForm
    {
        public CartaoPosForm()
        {
            InitializeComponent();
        }

        private void EnviarDadosCartaoPos_OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key != Key.Enter) return;

                var model = DataContext as CartaoPosFormModel;
                model?.OnEnviarDadosCartaoPos();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
