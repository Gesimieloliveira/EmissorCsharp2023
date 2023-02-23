using FusionCore.FusionAdm.Localidade;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPais : Repositorio<Pais, short>, IRepositorioPais
    {
        public RepositorioPais(ISession sessao) : base(sessao)
        {
        }
    }
}