using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Produtos
{
    public class ProdutoVinculoCompra : Entidade
    {
        public int Id { get; private set; }
        public ProdutoDTO Produto { get; set; }
        public string DocumentoFornecedor { get; set; }
        public string Codigo { get; set; }
        public string UnidadeCompra { get; set; }
        public decimal FatorUtilizado { get; set; }
        protected override int ReferenciaUnica => Id;
    }
}