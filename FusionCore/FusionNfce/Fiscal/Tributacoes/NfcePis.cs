using FusionCore.Tributacoes.Federal;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfcePis
    {
        public NfcePis() { }
        public NfcePis(TributacaoPis tributacaoPis)
        {
            CopiaInformacoes(tributacaoPis);
        }

        public string Id { get; set; }
        public string Descricao { get; set; }

        private void CopiaInformacoes(TributacaoPis tributacaoPis)
        {
            Id = tributacaoPis.Id;
            Descricao = tributacaoPis.Descricao;
        }
    }
}