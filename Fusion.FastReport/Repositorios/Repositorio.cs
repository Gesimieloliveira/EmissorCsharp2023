using System;
using NHibernate;

namespace Fusion.FastReport.Repositorios
{
    public abstract class Repositorio : IDisposable
    {
        protected readonly IStatelessSession Sessao;

        protected Repositorio(IStatelessSession sessao)
        {
            Sessao = sessao;
        }

        public void Dispose()
        {
            Sessao?.Dispose();
        }
    }
}