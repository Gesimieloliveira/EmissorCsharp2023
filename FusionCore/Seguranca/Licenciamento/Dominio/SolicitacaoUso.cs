using System;
using System.Runtime.Serialization;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Maquina;
using FusionLibrary.Helper.Criptografia;
using Newtonsoft.Json;

// ReSharper disable All

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class SolicitacaoUso : IEquatable<SolicitacaoUso>
    {
        private SolicitacaoUso()
        {
        }

        [JsonConstructor()]
        internal SolicitacaoUso(
            string ip,
            string nomeMaquina,
            string usuarioDominio,
            string sid,
            string ukMaquina,
            TipoSistema sistema) : this()
        {
            Ip = ip;
            NomeMaquina = nomeMaquina;
            UsuarioDominio = usuarioDominio;
            UkMaquina = ukMaquina;
            TipoSistema = sistema;
            SID = sid;
        }

        [DataMember]
        [JsonProperty("IP")]
        public string Ip { get; private set; }

        [DataMember]
        [JsonProperty("NomeMaquina")]
        public string NomeMaquina { get; private set; }

        [DataMember]
        [JsonProperty("UsuarioDominio")]
        public string UsuarioDominio { get; private set; }

        [DataMember]
        [JsonProperty("SID")]
        public string SID { get; private set; }

        [DataMember]
        [JsonProperty("UkMaquina")]
        public string UkMaquina { get; private set; }

        [DataMember]
        [JsonProperty("TipoSistema")]
        public TipoSistema TipoSistema { get; private set; }

        public static SolicitacaoUso Factory(TipoSistema tipoSistema)
        {
            // TODO 1612 licenciamento 
            /*var su = new SolicitacaoUso(
                AmbienteHelper.GetNetworkIp(),
                AmbienteHelper.GetNomeMaquina(),
                AmbienteHelper.GetNomeUsuarioDominio(),
                AmbienteHelper.GetSID(),
                ChaveMaquinaUkProvider.ProvideUk(),
                tipoSistema
            );*/

            return new SolicitacaoUso(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, tipoSistema);
        }

        public string ComputarHashUnica()
        {
            return Sha1Helper.Computar($"{UkMaquina}:{SID}");
        }

        public bool MesmaMaquina(SolicitacaoUso solicitacao)
        {
            return solicitacao.ComputarHashUnica() == ComputarHashUnica();
        }

        public bool Equals(SolicitacaoUso other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                Ip == other.Ip && 
                NomeMaquina == other.NomeMaquina && 
                UsuarioDominio == other.UsuarioDominio && 
                SID == other.SID && 
                UkMaquina == other.UkMaquina && 
                TipoSistema == other.TipoSistema;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SolicitacaoUso)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Ip != null ? Ip.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NomeMaquina != null ? NomeMaquina.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UsuarioDominio != null ? UsuarioDominio.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SID != null ? SID.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UkMaquina != null ? UkMaquina.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)TipoSistema;
                return hashCode;
            }
        }

        public static bool operator ==(SolicitacaoUso left, SolicitacaoUso right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SolicitacaoUso left, SolicitacaoUso right)
        {
            return !Equals(left, right);
        }
    }
}