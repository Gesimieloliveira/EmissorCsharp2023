using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class DigitoVerificador : IComponenteValorUnico<int>
    {
        private readonly int _valor;

        private DigitoVerificador() : this(0) { }

        public DigitoVerificador(int digitoVerificador)
        {
            _valor = digitoVerificador;
        }

        public int Valor => _valor;

        protected bool Equals(DigitoVerificador other)
        {
            return _valor == other._valor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DigitoVerificador) obj);
        }

        public override int GetHashCode()
        {
            return _valor;
        }
    }
}