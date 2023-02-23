using FusionCore.Core.Estoque;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Controles.Objetos
{
    public class ProdutoCombo
    {
        public ProdutoCombo(IProdutoSelecionado produto)
        {
            Produto = produto;
            CodigoUtilizado = produto.ProdutoId.ToString();
        }

        public ProdutoCombo(IProdutoSelecionado produto, string codigoUtilizado)
        {
            Produto = produto;
            CodigoUtilizado = codigoUtilizado;
        }

        public IProdutoSelecionado Produto { get; }
        public string CodigoUtilizado { get; }

        private bool Equals(ProdutoCombo other)
        {
            return Equals(Produto, other.Produto) && string.Equals(CodigoUtilizado, other.CodigoUtilizado);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProdutoCombo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Produto != null ? Produto.GetHashCode() : 0) * 397) ^ (CodigoUtilizado != null ? CodigoUtilizado.GetHashCode() : 0);
            }
        }

        public ProdutoDTO CarregaProduto()
        {
            return Produto.CarregaProduto();
        }
    }
}
