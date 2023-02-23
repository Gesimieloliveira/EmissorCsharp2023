using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda
{
    public class FormCancelamentoPedidoModel : ViewModel
    {
        public string MotivoCancelamento
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}