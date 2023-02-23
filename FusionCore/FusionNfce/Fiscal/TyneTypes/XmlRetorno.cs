using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class XmlRetorno : IComponenteValorUnico<string>
    {
        private readonly string _valor;

        private XmlRetorno() { }
        public XmlRetorno(string xmlRetorno)
        {
            _valor = xmlRetorno;
        }

        public string Valor => _valor;

        protected bool Equals(XmlRetorno other)
        {
            return string.Equals(_valor, other._valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((XmlRetorno) obj);
        }

        public override int GetHashCode()
        {
            return (_valor != null ? _valor.GetHashCode() : 0);
        }
    }
}