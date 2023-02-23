using Fusion.FastReport.Repositorios;
using FusionCore.Filtros;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RMovimentacoesVendas : RelatorioBase
    {
        private FiltroPeriodo _periodo;

        public RMovimentacoesVendas(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        public void ComPeriodo(FiltroPeriodo periodo)
        {
            _periodo = periodo;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RMovimentacoesVendas>("FrMovimentacoesVenda.frx");
        }

        protected override void PrepararRelatorio()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioMovimentacao(sessao);

                var movimentos = repositorio.BuscaMovimentos(_periodo);
                var itens = repositorio.BuscaItens(_periodo);

                RegistraDados("dsMovimentacao", movimentos);
                RegistraDados("dsItens", itens);
            }
        }
    }
}