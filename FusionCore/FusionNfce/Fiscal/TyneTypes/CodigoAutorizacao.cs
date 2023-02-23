using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class CodigoAutorizacao : IComponenteValorUnico<short>
    {
        private readonly short _valor;

        public static CodigoAutorizacao Default = new CodigoAutorizacao(0);

        private CodigoAutorizacao() { }

        public CodigoAutorizacao(short codigoAutorizacao)
        {
            _valor = codigoAutorizacao;
        }

        public short Valor => _valor;

        protected bool Equals(CodigoAutorizacao other)
        {
            return _valor == other._valor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CodigoAutorizacao) obj);
        }

        public override int GetHashCode()
        {
            return _valor.GetHashCode();
        }
    }
}