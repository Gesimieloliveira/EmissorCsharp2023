using FusionCore.Repositorio.Base;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class CancelamentoDFe : EntidadeBase<int>
    {
        public int NfeResumidaId { get; set; }
        public NfeResumida NfeResumida { get; set; }
        public string Xml { get; set; }
        protected override int ChaveUnica => NfeResumidaId;
    }
}