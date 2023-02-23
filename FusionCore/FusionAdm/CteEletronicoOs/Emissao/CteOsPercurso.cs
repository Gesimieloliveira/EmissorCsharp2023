using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsPercurso : Entidade
    {
        private CteOsPercurso()
        {
            //nhiberntate
        }

        public CteOsPercurso(EstadoDTO estado) : this()
        {
            Estado = estado;
        }

        protected override int ReferenciaUnica => Id;
        public int Id { get; set; }
        public CteOs CteOs { get; set; }
        public EstadoDTO Estado { get; set; }
    }
}