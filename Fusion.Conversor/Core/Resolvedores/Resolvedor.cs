using Fusion.Conversor.Core.Helpers;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores
{
    public abstract class Resolvedor
    {
        protected readonly IStatelessSession Session;
        protected readonly StringPreparer StringPreparer = new StringPreparer();
        protected readonly IntegerConverter IntegerConverter = new IntegerConverter();

        protected Resolvedor(IStatelessSession session)
        {
            Session = session;
        }
    }
}