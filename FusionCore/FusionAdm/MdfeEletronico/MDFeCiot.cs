using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeCiot : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeRodoviario Rodoviario { get; set; }
        public string Ciot { get; set; }
        public string DocumentoUnico { get; set; }
    }
}