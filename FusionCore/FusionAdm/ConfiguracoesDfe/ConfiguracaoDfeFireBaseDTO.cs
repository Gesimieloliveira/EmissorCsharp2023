using Newtonsoft.Json;

namespace FusionCore.FusionAdm.ConfiguracoesDfe
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfiguracaoDfeFirebaseDTO
    {
        [JsonProperty("homologacao")]
        public Ambiente Homologacao { get; set; }

        [JsonProperty("producao")]
        public Ambiente Producao { get; set; }

        [JsonProperty("siglaUf")]
        public string SiglaUf { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Ambiente
    {
        [JsonProperty("qrCode_Cte")]
        public bool IsQrCodeCte { get; set; }

        [JsonProperty("qrCode_CteOs")]
        public bool IsQrCodeCteOs { get; set; }

        [JsonProperty("qrCode_Mdfe")]
        public bool IsQrCodeMdfe { get; set; }
    }
}