using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class LocalEntrega : EntidadeBase<int>
    {
        public int NfeId { get; set; }
        public Nfeletronica Nfe { get; set; }

        public PessoaEndereco Endereco { get; set; }
        protected override int ChaveUnica => NfeId;
    }
}