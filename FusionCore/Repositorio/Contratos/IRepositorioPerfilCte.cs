using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronico;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioPerfilCte : IRepositorio<PerfilCte, short>
    {
        void Salvar(PerfilCte perfilCte);

        void Deletar(PerfilCte perfilCte);

        IList<PerfilCteGrid> BuscaTodosParaGrid();
    }
}