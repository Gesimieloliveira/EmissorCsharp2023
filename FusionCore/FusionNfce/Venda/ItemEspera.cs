using FusionCore.FusionNfce.Produto;

namespace FusionCore.FusionNfce.Venda
{
    public class ItemEspera
    {
        public ItemEspera(ProdutoNfce produto, string codigoUtilizado)
        {
            Produto = produto;
            CodigoUtilizado = codigoUtilizado;
        }

        public ProdutoNfce Produto { get; }
        public string CodigoUtilizado { get; }
    }
}