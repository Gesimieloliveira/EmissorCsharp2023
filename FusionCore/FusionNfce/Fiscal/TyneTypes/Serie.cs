using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class Serie : IComponenteValorUnico<short>
    {
        private readonly short _valor;

        public Serie(short serie)
        {
            _valor = serie;
        }

        private Serie() : this(0) { }

        public short Valor => _valor;

        protected bool Equals(Serie other)
        {
            return _valor == other._valor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Serie) obj);
        }

        public override int GetHashCode()
        {
            return _valor.GetHashCode();
        }
    }
}