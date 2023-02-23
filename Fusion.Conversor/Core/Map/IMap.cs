using System.Collections.Generic;

namespace Fusion.Conversor.Core.Map
{
    public interface IMap
    {
        string ColunasObrigatorias { get; }
        IEnumerable<string> ColunasPossiveis { get; }
    }
}