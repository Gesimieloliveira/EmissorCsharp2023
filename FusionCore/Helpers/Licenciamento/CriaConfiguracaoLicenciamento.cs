using System.ServiceModel;
using FusionCore.Helpers.Exe;

namespace FusionCore.Helpers.Licenciamento
{
    public static class CriaConfiguracaoLicenciamento
    {
        public static ConfiguracaoLicenciamento Criar()
        {
            var licenciamento = ArquivoConexaoLicenciamento.LerArquivo();

            var url = $"http://{licenciamento.Servidor}:{licenciamento.Porta}/fusion/api";
            var configLicenciamento = new ConfiguracaoLicenciamento(new EndpointAddress(url));

            return configLicenciamento;
        } 
    }
}