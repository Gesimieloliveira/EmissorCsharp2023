using FusionCore.FusionAdm.Automoveis;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeVeiculoReboque : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeRodoviario Rodoviario { get; set; }
        public Veiculo Veiculo { get; set; }
    }
}