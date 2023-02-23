using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Produto
{
    public class ProdutoAliasNfce : Entidade
    {
        public ProdutoAliasNfce() { }
        public ProdutoAliasNfce(ProdutoAlias produtoAlias, ProdutoNfce produtoNfce)
        {
            CopiarInformacoes(produtoAlias, produtoNfce);
        }

        public int Id { get; set; }
        public ProdutoNfce Produto { get; set; }
        public bool IsCodigoBarras { get; set; }
        public string Alias { get; set; }
        protected override int ReferenciaUnica => Id;

        private void CopiarInformacoes(ProdutoAlias produtoAlias, ProdutoNfce produtoNfce)
        {
            Id = produtoAlias.Id;
            Produto = produtoNfce;
            IsCodigoBarras = produtoAlias.IsCodigoBarras;
            Alias = produtoAlias.Alias;
        }
    }
}