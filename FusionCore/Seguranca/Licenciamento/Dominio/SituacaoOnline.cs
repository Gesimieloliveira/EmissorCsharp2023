using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class SituacaoOnline
    {
        [DataMember]
        [JsonProperty("Bloqueado")]
        public bool Bloqueado { get; set; }

        [DataMember]
        [JsonProperty("MensagemBloqueio")]
        public string MensagemBloqueio { get; set; }

        [DataMember]
        [JsonProperty("UltimaChecagem")]
        public DateTime? UltimaChecagem { get; set; }

        private SituacaoOnline()
        {
            //DATA CONTRACT
        }

        public SituacaoOnline(bool bloqueado, string mensagemBloqueio, DateTime? ultimaChecagem = null)
        {
            Bloqueado = bloqueado;
            MensagemBloqueio = mensagemBloqueio;
            UltimaChecagem = ultimaChecagem;
        }
    }
}