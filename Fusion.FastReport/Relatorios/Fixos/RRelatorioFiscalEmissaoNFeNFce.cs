using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RRelatorioFiscalEmissaoNFeNFce : RelatorioBase
    {
        private readonly string _descriacao;

        public RRelatorioFiscalEmissaoNFeNFce(ISessaoManager sessaoManager, string descriacao) : base(sessaoManager)
        {
            _descriacao = descriacao;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrRelatorioFiscalEmissaoNFeNFCe.frx");
        }

        protected override void PrepararDados()
        {
            RegistraParametro("DescricaoRelatorio", _descriacao);
        }
    }
}