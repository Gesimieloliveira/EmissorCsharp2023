using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.Repositorio.Dtos.Consultas;

namespace FusionCore.Repositorio.FusionAdm
{
    public interface IRepositorioCartaCorrecaoCte
    {
        IEnumerable<ICartaCorrecaoCteDTO> ListarCartaCorrecao(ICartaCorrecaoCte correcaoCte);
        byte ObterSequenciaCCe(ICartaCorrecaoCte cte);
    }
}