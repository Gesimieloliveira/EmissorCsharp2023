using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RListagemEstoquePorGrupo : RelatorioBase
    {
        public RListagemEstoquePorGrupo(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrListagemEstoquePorGrupo.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}