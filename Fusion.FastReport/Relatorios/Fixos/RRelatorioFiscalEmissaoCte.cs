using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RRelatorioFiscalEmissaoCte : RelatorioBase
    {
        private readonly string _descricao;

        public RRelatorioFiscalEmissaoCte(ISessaoManager sessaoManager, string descricao) : base(sessaoManager)
        {
            _descricao = descricao;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrRelatorioFiscalEmissaoCTe.frx");
        }

        protected override void PrepararDados()
        {
            RegistraParametro("DescricaoRelatorio", _descricao);
        }
    }
}