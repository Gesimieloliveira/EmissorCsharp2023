using System;
using Newtonsoft.Json;

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SituacaoLocal
    {
        [JsonProperty("UltimaValidacao")]
        public DateTime UltimaValidacao { get; set; }

        [JsonProperty("ValidadeDias")]
        public int ValidadeDias { get; set; }

        public bool Expirou => UltimaValidacao.AddDays(ValidadeDias) < DateTime.Now;

        public SituacaoLocal(DateTime ultimaValidacao, int validadeDias)
        {
            UltimaValidacao = ultimaValidacao;
            ValidadeDias = validadeDias;
        }
    }
}