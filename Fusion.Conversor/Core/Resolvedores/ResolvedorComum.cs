using System;
using Fusion.Conversor.Core.Cache;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores
{
    public abstract class ResolvedorComum<T> : ResolvedorCacheable<T>
    {
        private bool _forcarDefault;

        protected ResolvedorComum(IStatelessSession session, ArrayCache<T> cache) : base(session, cache)
        {
        }

        public ResolvedorComum<T> ForcarUsoDefault(bool forcar)
        {
            _forcarDefault = forcar;

            return this;
        }

        public T Resolve(string input, T @default)
        {
            try
            {
                input = input?.Trim();

                if (_forcarDefault || string.IsNullOrEmpty(input))
                {
                    return @default;
                }

                return DoResolve(input, @default);
            }
            catch (Exception e)
            {
                throw new Exception($"Falha ao resolver {typeof(T).Name}", e);
            }
        }

        protected abstract T DoResolve(string input, T @default);
    }
}