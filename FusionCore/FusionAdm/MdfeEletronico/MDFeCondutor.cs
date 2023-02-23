using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeCondutor : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeVeiculoTracao VeiculoTracao { get; set; }
        public PessoaEntidade Condutor { get; set; }
    }
}