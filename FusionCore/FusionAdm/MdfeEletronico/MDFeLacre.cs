using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeLacre : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeEletronico MDFeEletronico { get; set; }
        public string Numero { get; set; }
    }
}