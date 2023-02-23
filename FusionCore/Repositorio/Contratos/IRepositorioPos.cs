using System.Collections.Generic;
using FusionCore.FusionAdm.Tef;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioPos : IRepositorio<Pos, short>
    {
        void SalvarOuAtualizar(Pos pos);
        IEnumerable<Pos> BuscaComFiltro(string textoPesquisado);

    }
}