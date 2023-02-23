using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeContratante : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeRodoviario Rodoviario { get; set; }
        public PessoaEntidade PessoaEntidade { get; set; }
    }
}