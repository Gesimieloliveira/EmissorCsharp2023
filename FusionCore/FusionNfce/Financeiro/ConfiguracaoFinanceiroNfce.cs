using JetBrains.Annotations;

namespace FusionCore.FusionNfce.Financeiro
{
    public class ConfiguracaoFinanceiroNfce
    {
        public byte Id
        {
            get
            {
                return 1;
            }

            [UsedImplicitly]
            // ReSharper disable once ValueParameterNotUsed
            private set { }
        }
        public bool ImprimirComprovanteCrediario { get; set; }
    }
}