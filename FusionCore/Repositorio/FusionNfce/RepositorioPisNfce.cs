using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioPisNfce : Repositorio<NfcePis, string>
    {
        public RepositorioPisNfce(ISession sessao) : base(sessao)
        {
        }
    }
}