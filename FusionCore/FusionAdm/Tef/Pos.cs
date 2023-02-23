using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.Tef
{
    public class Pos : Entidade, ISincronizavelAdm
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public string CnpjCredenciadora { get; set; }
        public string Serial { get; set; }
        public string EstabelecimentoCodigo { get; set; }
        public string Adquirente { get; set; }
        public bool FlagMfe { get; set; } = true;
        public bool FlagNfce { get; set; }
        public bool Status { get; set; }

        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Pos;
        protected override int ReferenciaUnica => Id;
        public Credenciadora? Credenciadora { get; set; }
    }
}