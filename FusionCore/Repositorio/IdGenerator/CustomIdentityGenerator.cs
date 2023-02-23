using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Id.Insert;

namespace FusionCore.Repositorio.IdGenerator
{
    public class CustomIdentityGenerator : IPostInsertIdentifierGenerator
    {
        public Task<object> GenerateAsync(ISessionImplementor session, object obj, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public object Generate(ISessionImplementor s, object obj)
        {
            if (obj is IEntidadeIdentity entity && entity.Id > 0)
            {
                return entity.Id;
            }

            return IdentifierGeneratorFactory.PostInsertIndicator;
        }

        public IInsertGeneratedIdentifierDelegate GetInsertGeneratedIdentifierDelegate(
            IPostInsertIdentityPersister persister,
            ISessionFactoryImplementor factory,
            bool isGetGeneratedKeysEnabled
        ) {
            if (isGetGeneratedKeysEnabled)
            {
                throw new NotSupportedException();
            }
            else if (factory.Dialect.SupportsInsertSelectIdentity)
            {
                return new IdentityGenerator.InsertSelectDelegate(persister, factory);
            }
            else
            {
                return new IdentityGenerator.BasicDelegate(persister, factory);
            }
        }
    }
}