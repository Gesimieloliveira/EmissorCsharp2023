using System.Collections.Generic;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Flags;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioTributacaoCstNfce : Repositorio<TributacaoCstNfce, string>
    {
        public RepositorioTributacaoCstNfce(ISession sessao) : base(sessao)
        {
        }

        public IList<TributacaoCstNfce> ObterTributacaoPorRegimeTributario(RegimeTributario regimeTributario)
        {
            return Sessao.QueryOver<TributacaoCstNfce>()
                .Where(x => x.RegimeTributario == regimeTributario)
                .OrderBy(x => x.Id).Asc
                .List<TributacaoCstNfce>();
        }

        public TributacaoCstNfce BuscarPorId(string id)
        {
            return Sessao.Get<TributacaoCstNfce>(id);
        }
    }
}