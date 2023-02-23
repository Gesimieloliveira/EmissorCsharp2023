using System;
using Fusion.FastReport.Repositorios;
using FusionCore.Repositorio.Filtros;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RAniversariantes : RelatorioBase
    {
        private FiltroPeriodoNascimento _periodo;

        public RAniversariantes(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RAniversariantes>("FrAniversariantes.frx");
        }

        public void DefinirPeriodo(FiltroPeriodoNascimento periodo)
        {
            _periodo = periodo;
        }

        protected override void PrepararDados()
        {
            if (_periodo == null)
            {
                throw new InvalidOperationException("Preciso de um periodo");
            }

            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioAniversariante(sessao);
                var result = repositorio.BuscaAniversariantes(_periodo);

                RegistraDados("dsAniversariantes", result);
            }
        }
    }
}