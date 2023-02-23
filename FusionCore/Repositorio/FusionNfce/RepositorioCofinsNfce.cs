using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioCofinsNfce : Repositorio<NfceCofins, string>
    {
        public RepositorioCofinsNfce(ISession sessao) : base(sessao)
        {
        }
    }
}