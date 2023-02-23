using System;
using System.Collections.Generic;
using System.Linq;

namespace FusionCore.Helpers.Listas
{
    public static class ListHelper
    {
        public static void ChangeSingle<T>(this IList<T> lista, Func<T, bool> single, T por)
        {
            var item = lista.SingleOrDefault(single);
            var indexItem = lista.IndexOf(item);

            lista.RemoveAt(indexItem);
            lista.Insert(indexItem, por);
        }
    }
}