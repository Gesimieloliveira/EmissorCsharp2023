using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Controles.Checkout
{
    public class CheckoutItem
    {
        public CheckoutItem(ProdutoDTO produto, decimal quantidade, bool codigoBalanca = false)
        {
            Quantidade = quantidade;
            Produto = produto;
            CodigoBalanca = codigoBalanca;
        }

        public ProdutoDTO Produto { get; }
        public decimal Quantidade { get; }
        public bool CodigoBalanca { get; }
    }
}