using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Contratos.Base.Busca
{
    public interface IBuscaUnico<out T> where T : IEntidade
    {
        T Busca(ISession sessao);
    }
}