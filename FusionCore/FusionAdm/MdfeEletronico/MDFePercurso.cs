using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFePercurso : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeEletronico MDFeEletronico { get; set; }
        public EstadoDTO Estado { get; set; }
    }
}