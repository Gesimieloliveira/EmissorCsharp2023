using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioCentroCusto : Repositorio<CentroCusto, short>, IRepositorioCentroCusto
    {
        public RepositorioCentroCusto(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(CentroCusto centroCusto)
        {
            Sessao.SaveOrUpdate(centroCusto);
        }

        public IList<CentroCusto> ObterCategoriasPai()
        {
            CentroCusto centroCusto = null;

            var queryOver = Sessao.QueryOver(() => centroCusto).Where(() => centroCusto.CentroCustoPai == null);

            var lista = queryOver.List();

            return lista;
        }

        public IList<CentroCusto> ObterCategoriasPorDescricao(string descricao)
        {
            CentroCusto centroCusto = null;

            var queryOver = Sessao.QueryOver(() => centroCusto).WhereRestrictionOn(() => centroCusto.Descricao).IsLike("%"+descricao+"%");

            var lista = queryOver.List();

            return lista;
        }
        
        public void Deletar(CentroCusto centroCusto)
        {
            var centroCustoDeletar = GetPeloId(centroCusto.Id);
            centroCustoDeletar.Itens = null;
            centroCustoDeletar.CentroCustoPai = null;
            Sessao.Delete(centroCustoDeletar);
        }
    }
}