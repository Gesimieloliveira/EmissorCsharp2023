using FusionCore.FusionAdm.CteEletronico.Flags;

namespace FusionCore.FusionAdm.CteEletronico
{
    public class PerfilCteGrid
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public TipoCte TipoCte { get; set; }
        public TipoServico TipoServico { get; set; }
    }
}