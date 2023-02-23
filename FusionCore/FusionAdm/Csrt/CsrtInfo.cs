using Newtonsoft.Json;

namespace FusionCore.FusionAdm.Csrt
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CsrtInfo
    {
        [JsonProperty("csrt")]
        public string Csrt { get; set; }

        [JsonProperty("csrtId")]
        public string CsrtId { get; set; }

        [JsonProperty("siglaUf")]
        public string SiglaUf { get; set; }

        [JsonProperty("isCTe")]
        public bool IsCTe { get; set; }

        [JsonProperty("isCTeOs")]
        public bool IsCTeOs { get; set; }

        [JsonProperty("isMDFe")]
        public bool IsMDFe { get; set; }

        [JsonProperty("isNFCe")]
        public bool IsNFCe { get; set; }

        [JsonProperty("isNFe")]
        public bool IsNFe { get; set; }
    }
}