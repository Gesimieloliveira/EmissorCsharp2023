using System;
using System.Runtime.Serialization;
using FusionCore.Seguranca.Licenciamento.JsonConverters;
using Newtonsoft.Json;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class Licenca : ICloneable
    {
        [DataMember]
        [JsonProperty("LiberadoEm")]
        public DateTime LiberadoEm { get; set; }

        [DataMember]
        [JsonProperty("ContraChave")]
        public string ContraChave { get; private set; }

        [DataMember]
        [JsonProperty("Tipo")]
        [JsonConverter(typeof (TipoLicencaConverter))]
        public TipoLicenca Tipo { get; private set; }

        public Licenca(string contraChave, TipoLicenca tipo, DateTime liberadoEm)
        {
            ContraChave = contraChave;
            Tipo = tipo;
            LiberadoEm = liberadoEm;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private bool Equals(Licenca other)
        {
            return string.Equals(ContraChave, other.ContraChave);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Licenca) obj);
        }

        public override int GetHashCode()
        {
            return ContraChave?.GetHashCode() ?? 0;
        }

        public static bool operator ==(Licenca left, Licenca right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Licenca left, Licenca right)
        {
            return !Equals(left, right);
        }
    }
}