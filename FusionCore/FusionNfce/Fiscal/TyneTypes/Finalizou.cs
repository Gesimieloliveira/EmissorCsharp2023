using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class Finalizou : IComponenteValorUnico<bool>
    {
        private readonly bool _valor;

        public static Finalizou Default => new Finalizou(false);

        private Finalizou() { }

        public Finalizou(bool finalizou)
        {
            _valor = finalizou;
        }

        public bool Valor => _valor;

        protected bool Equals(Finalizou other)
        {
            return _valor == other._valor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Finalizou) obj);
        }

        public override int GetHashCode()
        {
            return _valor.GetHashCode();
        }
    }
}