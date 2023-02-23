using FusionCore.Setup;
using Newtonsoft.Json;

namespace FusionCore.FusionPdv.Setup.BD
{
    public class ConexaoCfg
    {
        public ConexaoCfg(string bancoDados)
        {
            BancoDados = bancoDados;
            Host = "LOCALHOST";
            Instancia = "FUSION";
            Usuario = "sa";
            Senha = "fusion";
        }

        [JsonProperty("Host")]
        public string Host { get; set; }

        [JsonProperty("Instancia")]
        public string Instancia { get; set; }

        [JsonProperty("BancoDados")]
        public string BancoDados { get; set; }

        [JsonProperty("Usuario")]
        public string Usuario { get; set; }

        [JsonProperty("Senha")]
        public string Senha { get; set; }

        public IConexaoCfg ToCfg()
        {
            return new FusionCore.Setup.ConexaoCfg
            {
                Servidor = Host,
                Instancia = Instancia,
                Porta = 1433,
                BancoDados = BancoDados,
                Senha = Senha,
                Usuario = Usuario
            };
        }
    }
}