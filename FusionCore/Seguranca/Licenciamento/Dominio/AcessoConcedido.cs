using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class AcessoConcedido : IEquatable<AcessoConcedido>
    {
        [DataMember]
        [JsonProperty("UltimaChecagem")]
        public DateTime UltimaChecagem { get; set; }

        [DataMember]
        [JsonProperty("SolicitacaoUso")]
        public SolicitacaoUso SolicitacaoUso { get; set; }

        [DataMember]
        [JsonProperty("PossuiFusionCTe")]
        public bool PossuiFusionCTe { get; set; }

        [DataMember]
        [JsonProperty("PossuiFusionStarter")]
        public bool PossuiFusionStarter { get; set; }

        [DataMember]
        [JsonProperty("PossuiFusionGestor")]
        public bool PossuiFusionGestor { get; set; }

        [DataMember]
        [JsonProperty("PossuiFusionMdfe")]
        public bool PossuiFusionMdfe { get; set; }

        [DataMember]
        [JsonProperty("PossuiFusionCteOs")]
        public bool PossuiFusionCteOs { get; set; }

        public AcessoConcedido(SolicitacaoUso solicitacaoUso)
        {
            SolicitacaoUso = solicitacaoUso;
            UltimaChecagem = DateTime.Now;
        }

        public bool Expirou()
        {
            if (SolicitacaoUso.TipoSistema == TipoSistema.FusionAdm)
            {
                return UltimaChecagem.AddSeconds(ConstantesLicenciamento.ExpiracaoLicencaSegundos) < DateTime.Now;
            }

            return false;
        }

        public bool Equals(AcessoConcedido other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                Equals(SolicitacaoUso, other.SolicitacaoUso) &&
                PossuiFusionCTe == other.PossuiFusionCTe &&
                PossuiFusionStarter == other.PossuiFusionStarter &&
                PossuiFusionGestor == other.PossuiFusionGestor &&
                PossuiFusionMdfe == other.PossuiFusionMdfe &&
                PossuiFusionCteOs == other.PossuiFusionCteOs;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AcessoConcedido)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (SolicitacaoUso != null ? SolicitacaoUso.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ PossuiFusionCTe.GetHashCode();
                hashCode = (hashCode * 397) ^ PossuiFusionStarter.GetHashCode();
                hashCode = (hashCode * 397) ^ PossuiFusionGestor.GetHashCode();
                hashCode = (hashCode * 397) ^ PossuiFusionMdfe.GetHashCode();
                hashCode = (hashCode * 397) ^ PossuiFusionCteOs.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(AcessoConcedido left, AcessoConcedido right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AcessoConcedido left, AcessoConcedido right)
        {
            return !Equals(left, right);
        }
    }
}