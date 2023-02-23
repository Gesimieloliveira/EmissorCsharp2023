using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Dtos.Consultas;

namespace FusionCore.FusionAdm.Servico.CteEletronicoOs.Perfil
{
    public class ServicoPerfilCteOs
    {
        private readonly IRepositorioPerfilCteOs _repositorioPerfilCteOs;

        public ServicoPerfilCteOs(IRepositorioPerfilCteOs repositorioPerfilCteOs)
        {
            _repositorioPerfilCteOs = repositorioPerfilCteOs;
        }

        public IList<PerfilCteOsGrid> Buscar(string descricao)
        {
            return _repositorioPerfilCteOs.BuscarPor(descricao);
        }

        public PerfilCteOs Buscar(int id)
        {
            return _repositorioPerfilCteOs.GetPeloId(id);
        }

        public IList<AbaPerfilCteOsDTO> BuscarPerfilDTO()
        {
            return _repositorioPerfilCteOs.BuscarAbaPerfilCteOS();
        }
    }
}