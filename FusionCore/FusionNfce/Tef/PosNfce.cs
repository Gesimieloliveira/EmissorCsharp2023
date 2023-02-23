using FusionCore.FusionAdm.Tef;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Tef
{
    public class PosNfce : Entidade
    {
        public PosNfce()
        {
        }

        public PosNfce(Pos pos)
        {
            Id = pos.Id;
            Descricao = pos.Descricao;
            Serial = pos.Serial;
            EstabelecimentoCodigo = pos.EstabelecimentoCodigo;
            Adquirente = pos.Adquirente;
            CnpjCredenciadora = pos.CnpjCredenciadora;
            FlagMfe = pos.FlagMfe;
            FlagNfce = pos.FlagNfce;
            Credenciadora = pos.Credenciadora;
            Status = pos.Status;
        }

        public short Id { get; set; }
        public string Descricao { get; set; }
        public string Serial { get; set; }
        public string EstabelecimentoCodigo { get; set; }
        public string Adquirente { get; set; }
        public bool FlagNfce { get; set; }
        public bool FlagMfe { get; set; }
        public bool Status {get; set;}
        public string CnpjCredenciadora { get; set; }
        public Credenciadora? Credenciadora { get; set; }

        protected override int ReferenciaUnica => Id;

        public override string ToString()
        {
            return $"{Descricao}";
        }
    }
}