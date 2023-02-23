using Fusion.FastReport.Repositorios;
using FusionCore.Filtros;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RMovimentacoesNfce : RelatorioBase
    {
        private FiltroPeriodo _periodo;

        public RMovimentacoesNfce(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        public void ComPeriodo(FiltroPeriodo periodo)
        {
            _periodo = periodo;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RAnaliseLucroPorItem>("FrMovimentacaoNfce.frx");
        }

        protected override void PrepararRelatorio()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioMovimentacao(sessao);
                var movimentacoes = repositorio.BuscaNfces(_periodo);

                RegistraDados("dsMovimentacao", movimentacoes);
            }
        }
    }
}