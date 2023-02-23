using System.Collections.Generic;
using FusionCore.FusionNfce.Servico.Estoque;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioEstoqueModelNfce : Repositorio<EstoqueModelNfce, int>, IRepositorioEstoqueModelNfce
    {
        public RepositorioEstoqueModelNfce(ISession sessao) : base(sessao)
        {
        }

        public void SalvarESincronizar(EstoqueModelNfce estoqueModel)
        {
            estoqueModel.Sincronizado = false;
            Sessao.SaveOrUpdate(estoqueModel);
        }

        public void SalvarENaoSincronizar(EstoqueModelNfce estoqueModel)
        {
            estoqueModel.Sincronizado = true;
            Sessao.SaveOrUpdate(estoqueModel);
        }

        public IEnumerable<EstoqueModelNfce> BuscarTodosEstoquesParaSincronizacao()
        {
            var queryOver = Sessao.QueryOver<EstoqueModelNfce>().Where(x => x.Sincronizado == false);

            var lista = queryOver.List<EstoqueModelNfce>();

            return lista;
        }
    }
}