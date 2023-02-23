using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioCentroLucro : Repositorio<CentroLucro, short>, IRepositorioCentroLucro
    {
        public RepositorioCentroLucro(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(CentroLucro centroLucro)
        {
            Sessao.SaveOrUpdate(centroLucro);
        }

        public IList<CentroLucro> ObterCategoriasPai()
        {
            CentroLucro centroLucro = null;

            var queryOver = Sessao.QueryOver(() => centroLucro).Where(() => centroLucro.CentroLucroPai == null);

            var lista = queryOver.List();

            return lista;
        }

        public IList<CentroLucro> ObterCategoriasPorDescricao(string descricao)
        {
            CentroLucro centroLucro = null;

            var queryOver = Sessao.QueryOver(() => centroLucro).WhereRestrictionOn(() => centroLucro.Descricao).IsLike("%" + descricao + "%");

            var lista = queryOver.List();

            return lista;
        }

        public void Deletar(CentroLucro centroLucro)
        {
            var centroCustoDeletar = GetPeloId(centroLucro.Id);
            centroCustoDeletar.Itens = null;
            centroCustoDeletar.CentroLucroPai = null;
            Sessao.Delete(centroCustoDeletar);
        }
    }
}