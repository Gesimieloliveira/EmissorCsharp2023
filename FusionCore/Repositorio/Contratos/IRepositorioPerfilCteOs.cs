using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.Repositorio.Dtos.Consultas;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioPerfilCteOs : IRepositorio<PerfilCteOs, int>, ISuporteSalvar<PerfilCteOs>
    {
        IList<PerfilCteOsGrid> BuscarPor(string descricao);

        IList<AbaPerfilCteOsDTO> BuscarAbaPerfilCteOS();
    }
}