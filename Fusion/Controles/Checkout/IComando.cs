using System.Windows.Media;

namespace Fusion.Controles.Checkout
{
    public interface IComando
    {
        string TextoInformativo { get; }
        Brush Background { get; }

        /// <exception cref="CheckoutException">This exception is thrown if the archive already exists</exception>
        void Executar(CheckoutBox comando);
    }
}