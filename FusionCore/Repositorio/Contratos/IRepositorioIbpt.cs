using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Transparencia;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioIbpt
    {
        IList<Ibpt> BuscaTodos();
        void DeletaTodos();
        void Persiste(Ibpt ibpt);
        Ibpt GetPeloNcm(string codigo);
        string GetChaveIbpt();
        IList<Ibpt> GetTodosPeloNcm(string ncm);
    }
}