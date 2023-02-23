using FusionCore.Tributacoes.Federal;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceCofins
    {
        public NfceCofins() { }

        public NfceCofins(TributacaoCofins cofins)
        {
            CopiaInformacoes(cofins);
        }

        public string Id { get; set; }
        public string Descricao { get; set; }

        private void CopiaInformacoes(TributacaoCofins cofins)
        {
            Id = cofins.Id;
            Descricao = cofins.Descricao;
        }
    }
}