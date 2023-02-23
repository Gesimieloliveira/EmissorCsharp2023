using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class Motivo : IComponenteValorUnico<string>
    {
        private readonly string _valor;

        public static Motivo Default = new Motivo(string.Empty);

        private Motivo() { }

        public Motivo(string motivo)
        {
            _valor = motivo;
        }

        protected bool Equals(Motivo other)
        {
            return string.Equals(_valor, other._valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Motivo) obj);
        }

        public override int GetHashCode()
        {
            return (_valor != null ? _valor.GetHashCode() : 0);
        }

        public string Valor => _valor;
    }
}