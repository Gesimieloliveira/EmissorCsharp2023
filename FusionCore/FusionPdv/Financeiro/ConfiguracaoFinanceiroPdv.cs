using JetBrains.Annotations;

namespace FusionCore.FusionPdv.Financeiro
{
    public class ConfiguracaoFinanceiroPdv
    {
        public byte Id {
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