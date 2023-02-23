using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class NumeroFiscal : IComponenteValorUnico<long>
    {
        private readonly long _valor;

        private NumeroFiscal() : this(0) { }

        public NumeroFiscal(long numeroFiscal)
        {
            _valor = numeroFiscal;
        }

        public long Valor => _valor;

        protected bool Equals(NumeroFiscal other)
        {
            return _valor == other._valor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NumeroFiscal) obj);
        }

        public override int GetHashCode()
        {
            return _valor.GetHashCode();
        }
    }
}