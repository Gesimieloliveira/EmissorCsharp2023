using System;

namespace Fusion.Controles.Checkout
{
    public class CheckoutException : InvalidOperationException
    {
        public CheckoutException(string message) : base(message)
        {
        }
    }
}