using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.ConfiguracoesDfe
{
    public class ConfiguracaoDfe 
    {
        public Guid Id { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public bool IsQrCodeCte { get; set; }
        public bool IsQrCodeCteOs { get; set; }
        public bool IsQrCodeMdfe { get; set; }
        public EstadoDTO Uf { get; set; }


        private ConfiguracaoDfe() { }

        public ConfiguracaoDfe(
            KeyValuePair<string, ConfiguracaoDfeFirebaseDTO> configuracaoDfeFirebaseDTO, 
            TipoAmbiente tipoAmbiente,
            EstadoDTO estadoUf)
        {
            var config = configuracaoDfeFirebaseDTO.Value.Homologacao;

            if (tipoAmbiente == TipoAmbiente.Producao)
                config = configuracaoDfeFirebaseDTO.Value.Producao;

            Id = new Guid(configuracaoDfeFirebaseDTO.Key);
            AmbienteSefaz = tipoAmbiente;
            IsQrCodeCte = config.IsQrCodeCte;
            IsQrCodeCteOs = config.IsQrCodeCteOs;
            IsQrCodeMdfe = config.IsQrCodeMdfe;
            Uf = estadoUf;
        }

        protected bool Equals(ConfiguracaoDfe other)
        {
            return Id.Equals(other.Id) && AmbienteSefaz == other.AmbienteSefaz;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConfiguracaoDfe) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (int) AmbienteSefaz;
            }
        }
    }
}