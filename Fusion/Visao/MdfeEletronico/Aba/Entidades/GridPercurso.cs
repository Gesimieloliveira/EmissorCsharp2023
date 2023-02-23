using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.MdfeEletronico.Aba.Entidades
{
    public class GridPercurso
    {
        public MDFePercurso Percurso { get; set; }
        public EstadoDTO EstadoUf { get; set; }
    }
}