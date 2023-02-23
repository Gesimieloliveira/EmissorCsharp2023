using FusionCore.FusionAdm.Configuracoes;

namespace FusionCore.FusionPdv.Configuracoes.Extencoes
{
    public static class ExtConfiguracaoFrenteCaixaPdv
    {
        public static ConfiguracaoFrenteCaixaPdv ToPdv(this ConfiguracaoFrenteCaixa configuracaoFrenteCaixa)
        {
            var configuracaoFrenteCaixaPdv = new ConfiguracaoFrenteCaixaPdv
            {
                Logo = configuracaoFrenteCaixa.Logo
            };

            return configuracaoFrenteCaixaPdv;
        }
    }
}