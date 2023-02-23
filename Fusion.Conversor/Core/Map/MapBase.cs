using System.Collections.Generic;
using CsvHelper.Configuration;

namespace Fusion.Conversor.Core.Map
{
    public abstract class MapBase<T> : ClassMap<T>, IMap
    {
        public string ColunasObrigatorias { get; protected set; }

        public IEnumerable<string> ColunasPossiveis
        {
            get
            {
                foreach (var map in MemberMaps)
                {
                    yield return map.Data.Names[0];
                }
            }
        }
    }
}