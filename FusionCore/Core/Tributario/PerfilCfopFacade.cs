using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Core.Tributario
{
    public static class PerfilCfopFacade
    {
        public static PerfilCfopDTO FindPeloCodigo(string codigo)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCfop(sessao);
                return repositorio.PeloCodigo(codigo);
            }
        }

        public static IEnumerable<PerfilCfopDTO> FindPorOperacao(TipoOperacao tipo)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCfop(sessao);
                return repositorio.PorOperacao(tipo);
            }
        }

        public static IEnumerable<PerfilCfopDTO> FindTodos()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilCfop(sessao);
                return repositorio.BuscaTodos();
            }
        }
    }
}