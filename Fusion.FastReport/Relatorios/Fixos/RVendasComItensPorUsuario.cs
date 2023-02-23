using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RVendasComItensPorUsuario : RelatorioBase
    {
        public RVendasComItensPorUsuario(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrVendasComItensPorUsuario.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}