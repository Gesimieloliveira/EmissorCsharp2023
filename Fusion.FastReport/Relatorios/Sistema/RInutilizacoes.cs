using Fusion.FastReport.Repositorios;
using FusionCore.Filtros;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RInutilizacoes : RelatorioBase
    {
        private FiltroPeriodo _periodo;

        public RInutilizacoes(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        public void ComPeriodo(FiltroPeriodo periodo)
        {
            _periodo = periodo;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RInutilizacoes>("FrInutilizacoes.frx");
        }

        protected override void PrepararRelatorio()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioInutilizacao(sessao);
                var ds = repositorio.ListarInutilizacoes(_periodo);

                RegistraDados("dsInutilizacao", ds);
                RegistraParametro("PeriodoFiltro", _periodo);
            }
        }
    }
}