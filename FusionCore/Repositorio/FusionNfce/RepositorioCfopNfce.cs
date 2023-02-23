using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FusionCore.FusionNfce.Cfop;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    [SuppressMessage("ReSharper", "RedundantBoolCompare")]
    public class RepositorioCfopNfce : Repositorio<CfopNfce, string>
    {
        public RepositorioCfopNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(CfopNfce cfop)
        {
            Sessao.SaveOrUpdate(cfop);
        }

        public override IList<CfopNfce> BuscaTodos()
        {
            var query = Sessao.QueryOver<CfopNfce>().Where(c => c.ElegivelNfce == true);
            return query.List();
        }
    }
}