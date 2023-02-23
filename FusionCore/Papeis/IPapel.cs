using System.Collections.Generic;

namespace FusionCore.Papeis
{
    public interface IPapel
    {
        IEnumerable<IPermissaoPapel> Permissoes { get; }
    }
}