using FusionCore.FusionAdm.Configuracoes;

namespace FusionCore.FusionNfce.Configuracoes
{
    public class ConfiguracaoFrenteCaixaNfce
    {
        private ConfiguracaoFrenteCaixaNfce()
        {
            //nhibernate
        }

        public byte Id { get; set; } = 1;

        public byte[] Logo { get; set; }
        public bool IsBloquearVendaParaResolverPendencia { get; set; }
        public decimal? ValorMinimoParaForcarClienteNaVenda { get; set; }
        public bool IsSegundaViaContingencia { get; set; }

        public static ConfiguracaoFrenteCaixaNfce From(ConfiguracaoFrenteCaixa configuracao)
        {
            return new ConfiguracaoFrenteCaixaNfce
            {
                Logo = configuracao.Logo,
                IsBloquearVendaParaResolverPendencia = configuracao.IsBloquearVendaParaResolverPendencia,
                ValorMinimoParaForcarClienteNaVenda = configuracao.ValorMinimoParaForcarClienteNaVenda,
                IsSegundaViaContingencia = configuracao.IsSegundaViaContingencia
            };
        }
    }
}