using System;
using System.Collections.Generic;
using System.Linq;

namespace Fusion.Conversor.Core.Cache
{
    public class ArrayCache<T>
    {
        private readonly IList<T> _cache;

        public ArrayCache(IList<T> cache = null)
        {
            _cache = cache ?? new List<T>();
        }

        public void Add(T item)
        {
            if (_cache.Any(i => i.Equals(item)))
            {
                return;
            }

            _cache.Add(item);
        }

        public IList<T> GetAll()
        {
            return _cache;
        }

        public T FindItem(Func<T, bool> match)
        {
            return TryGetCache(match, out var item) ? item : default(T);
        }

        public bool TryGetCache(Func<T, bool> match, out T finded)
        {
            finded = _cache.Where(match).SingleOrDefault();

            return finded != null;
        }
    }
}