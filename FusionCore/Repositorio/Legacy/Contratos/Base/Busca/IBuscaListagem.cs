using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Contratos.Base.Busca
{
    public interface IBuscaListagem<T> where T : IEntidade
    {
        IList<T> Busca(ISession sessao);
    }
}