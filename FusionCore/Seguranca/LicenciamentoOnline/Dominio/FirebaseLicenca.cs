using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionCore.Seguranca.LicenciamentoOnline.Dominio
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FirebaseLicenca
    {
        [JsonObject(MemberSerialization.OptIn)]
        public class Chave
        {
            [JsonProperty("criadaEm")]
            public DateTime CriadaEm { get; set; }
        }

        [JsonProperty("revendaId")]
        public int RevendaId { get; set; }

        [JsonProperty("bloqueio")]
        public string Bloqueio { get; set; }

        [JsonProperty("chaves")]
        public IDictionary<string, Chave> Chaves { get; set; }
    }
}