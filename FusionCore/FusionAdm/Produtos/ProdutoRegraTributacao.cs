using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Produtos
{
    public sealed class ProdutoRegraTributacao
    {
        public ProdutoDTO Produto { get; private set; }
        public EstadoDTO Uf { get; private set; }
        public decimal Aliquota { get; set; }
        public decimal AliquotaFcp { get; set; }

        private ProdutoRegraTributacao()
        {
            //nhibernate
        }

        public ProdutoRegraTributacao(ProdutoDTO produto, EstadoDTO uf, decimal aliquota, decimal fcp = 0) : this()
        {
            Uf = uf;
            Produto = produto;
            Aliquota = aliquota;
            AliquotaFcp = fcp;
        }

        private bool Equals(ProdutoRegraTributacao other)
        {
            return Equals(Uf.Id, other.Uf.Id) && Equals(Produto.Id, other.Produto.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ProdutoRegraTributacao) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Uf?.GetHashCode() ?? 0)*397) ^ (Produto?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(ProdutoRegraTributacao left, ProdutoRegraTributacao right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProdutoRegraTributacao left, ProdutoRegraTributacao right)
        {
            return !Equals(left, right);
        }
    }
}