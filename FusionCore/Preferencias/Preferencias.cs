namespace FusionCore.Preferencias
{
    public static class Preferencias
    {
        public const string Global = "global";

        public static class Balanca
        {
            public static class Checkout
            {
                public const string UsarBalanca = "balanca.checkout.usarBalanca";
                public const string Porta = "balanca.checkout.porta";
                public const string Protocolo = "balanca.checkout.protocolo";
                public const string Baud = "balanca.checkout.baud";
                public const string DelayMonitor = "balanca.checkout.delayMonitor";
            }
        }

        public static class Vendedor
        {
            public const string ObrigarUsoVendedor = "vendas.vendedor.obrigarUso";
        }
    }
}