using FusionCore.FusionAdm.Configuracoes;

namespace FusionCore.FusionNfce.Configuracoes.Extencoes
{
    public static class ExtConfiguracaoFrenteCaixa
    {
        public static ConfiguracaoFrenteCaixaNfce ToNfce(this ConfiguracaoFrenteCaixa configuracaoFrenteCaixa)
        {
            return ConfiguracaoFrenteCaixaNfce.From(configuracaoFrenteCaixa);
        }
    }
}