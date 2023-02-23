using Newtonsoft.Json;

namespace FusionCore.FusionPdv.Setup.BD
{
    public class ContainerCfg
    {
        [JsonProperty("ConexaoAdm")]
        public ConexaoCfg ConexaoAdm { get; set; }

        [JsonProperty("ConexaoPdv")]
        public ConexaoCfg ConexaoPdv { get; set; }

        public ContainerCfg(ConexaoCfg conexaoAdm, ConexaoCfg conexaoPdv)
        {
            ConexaoAdm = conexaoAdm;
            ConexaoPdv = conexaoPdv;
        }
    }
}