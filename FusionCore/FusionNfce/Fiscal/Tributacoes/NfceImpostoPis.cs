namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceImpostoPis
    {
        public int Id { get; set; }
        public NfceItem Item { get; set; }
        public NfcePis Pis { get; set; }
        public decimal Aliquota { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal Valor { get; set; }
    }
}