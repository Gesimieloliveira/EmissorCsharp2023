using FusionCore.FusionAdm.Nfce;
using FusionCore.Tributacoes.Federal;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceImpostoCofinsAdm
    {
        public int Id { get; set; }
        public NfceItemAdm Item { get; set; }
        public TributacaoCofins Cofins { get; set; }
        public decimal Aliquota { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal Valor { get; set; }
    }
}