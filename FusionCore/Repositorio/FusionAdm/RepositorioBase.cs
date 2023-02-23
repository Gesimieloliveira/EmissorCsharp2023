using System;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    [Serializable]
    public abstract class RepositorioBase
    {
        protected readonly ISession Sessao;

        protected RepositorioBase(ISession sessao)
        {
            Sessao = sessao;
        }

        protected void ThrowExceptionSeNaoExisteTransacao()
        {
            if (!Sessao.Transaction.IsActive)
            {
                throw new Exception("Sess�o com banco de dados precisa de uma transa��o aberta");
            }
        }
    }
}