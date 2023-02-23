using FusionCore.Seguranca.LicenciamentoOnline.Config;

#pragma warning disable 162
// ReSharper disable HeuristicUnreachableCode

namespace FusionCore.Seguranca.LicenciamentoOnline
{
    public static class FirebaseFactory
    {
        public static IEndpointCfg CriaCfg()
        {
#if DEBUG
            return GetConfiguracaoDesenvolvimento();
#endif
            return GetConfiguracaoProducao();
        }

        private static FirebaseConfigDev GetConfiguracaoDesenvolvimento()
        {
            return new FirebaseConfigDev();
        }

        private static IEndpointCfg GetConfiguracaoProducao()
        {
            return new FirebaseConfigProd();
        }
    }
}