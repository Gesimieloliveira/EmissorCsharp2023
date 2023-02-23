using FusionCore.Core.Flags;
using FusionCore.Core.Tributario;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class CfopDTO : Entidade, ISincronizavelAdm
    {
        public string Id { get; set; }
        protected override int ReferenciaUnica => Id.GetHashCode();
        public string Descricao { get; set; }
        public bool ElegivelNfce { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public string Referencia => Id;
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.Cfop;
        public OrigemOperacao OrigemOperacao => CodigoCfopHelper.ObtemOrigem(Id);

        public override string ToString()
        {
            return $"{Id} - {Descricao}";
        }
    }
}