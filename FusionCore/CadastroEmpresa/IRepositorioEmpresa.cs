using System.Collections.Generic;

namespace FusionCore.CadastroEmpresa
{
    public interface IRepositorioEmpresa
    {
        IEnumerable<IEmpresa> BuscarTodas();
    }
}