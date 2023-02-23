using Fusion.FastReport.Repositorios;
using FusionCore.Filtros;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RVendasNaNfceComItens : RelatorioBase
    {
        private FiltroPeriodo _periodo;

        public RVendasNaNfceComItens(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        public void ComPeriodo(FiltroPeriodo periodo)
        {
            _periodo = periodo;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RVendasNaNfceComItens>("FrVendasItensNfce.frx");
        }

        protected override void PrepararRelatorio()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioVendasNfce(sessao);
                var vendas = repositorio.ListarItensVendidos(_periodo);

                RegistraDados("dsVenda", vendas);
                RegistraDados("PeriodoFiltro", _periodo.ToString());
            }
        }
    }
}