namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceImpostoCofins
    {
        public int Id { get; set; }
        public NfceItem Item { get; set; }
        public NfceCofins Cofins { get; set; }
        public decimal Aliquota { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal Valor { get; set; }
    }
}