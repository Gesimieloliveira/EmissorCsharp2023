using System.Windows;
using System.Windows.Media;

namespace Fusion.Controles.Checkout
{
    public class ComandoQuantidade : IComando
    {
        public string TextoInformativo { get; } = "QUANTIDADE";
        public Brush Background { get; } = (Brush)Application.Current.FindResource("WarningBrush");

        public void Executar(CheckoutBox checkout)
        {
            decimal.TryParse(checkout.GetTextoComando(), out var quantidade);

            if (quantidade <= 0)
            {
                return;
            }

            checkout.SetQuantidade(quantidade);
        }
    }
}