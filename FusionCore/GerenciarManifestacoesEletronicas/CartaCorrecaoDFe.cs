using FusionCore.Repositorio.Base;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class CartaCorrecaoDFe : EntidadeBase<int>
    {
        public int Id { get; set; }
        public NfeResumida NfeResumida { get; set; }
        public string Xml { get; set; }
        protected override int ChaveUnica => Id;
    }
}