using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MdfeSeguroAverbacao : EntidadeBase<int>
    {
        public int Id { get; set; }
        public string Averbacao { get; set; }
        public MDFeSeguroCarga SeguroCarga { get; set; }
        protected override int ChaveUnica => Id;
    }
}