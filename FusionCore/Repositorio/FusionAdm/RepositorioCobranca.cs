using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Contratos;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioCobranca : Repositorio<Cobranca, int>, IRepositorioCobranca
    {
        public RepositorioCobranca(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(Cobranca cobranca)
        {
            Sessao.SaveOrUpdate(cobranca);
        }

        public void SalvarDuplicata(CobrancaDuplicata cobrancaDuplicata)
        {
            Sessao.SaveOrUpdate(cobrancaDuplicata);
        }

        public void DeletarComDuplicatas(Cobranca cobranca)
        {
            if (cobranca?.CobrancaDuplicatas.Count > 0)
            {
                cobranca.CobrancaDuplicatas.ForEach(Sessao.Delete);
            }
            Sessao.Delete(cobranca);
        }
    }
}