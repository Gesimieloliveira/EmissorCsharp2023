using FusionCore.FusionNfce.Financeiro;
using FusionCore.FusionPdv.Financeiro;

namespace FusionCore.FusionAdm.Financeiro.Extencoes
{
    public static class ExtConfiguracaoFinanceiro
    {
        public static ConfiguracaoFinanceiroNfce ToNfce(this ConfiguracaoFinanceiro configuracaoFinanceiro)
        {
            var configuracaoFinanceiroNfce = new ConfiguracaoFinanceiroNfce
            {
                ImprimirComprovanteCrediario = configuracaoFinanceiro.ImprimirComprovanteCrediario
            };

            return configuracaoFinanceiroNfce;
        }

        public static ConfiguracaoFinanceiroPdv ToPdv(this ConfiguracaoFinanceiro configuracaoFinanceiro)
        {
            var configuracaoFinanceiroPdv = new ConfiguracaoFinanceiroPdv
            {
                ImprimirComprovanteCrediario = configuracaoFinanceiro.ImprimirComprovanteCrediario
            };

            return configuracaoFinanceiroPdv;
        }
    }
}