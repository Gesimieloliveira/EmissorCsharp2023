using Fusion.FastReport.Repositorios;
using FusionCore.Repositorio.Filtros;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RInutilizacoesNfeNFce : RelatorioBase
    {
        private readonly string _descricao;
        private FiltroPeriodo _periodo;

        public RInutilizacoesNfeNFce(ISessaoManager sessaoManager, string descricao) : base(sessaoManager)
        {
            _descricao = descricao;
        }

        public void ComPeriodo(FiltroPeriodo periodo)
        {
            _periodo = periodo;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrInutilizacoesNFeNFCe.frx");
        }

        protected override void PrepararDados()
        {
            RegistraParametro("DescricaoRelatorio", _descricao);

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