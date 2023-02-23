using DFe.Classes.Entidades;
using DFe.Classes.Extensoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtEstadoDTO
    {
        public static Estado ToZeusMdfe(this EstadoDTO estadoDTO)
        {
            const Estado converteEstadoParaZeus = Estado.GO;
            var estadoUfZeus = converteEstadoParaZeus.CodigoIbgeParaEstado(estadoDTO.CodigoIbge.ToString());

            return estadoUfZeus;
        }
    }
}