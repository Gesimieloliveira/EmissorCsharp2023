using System;
using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class XmlEnvio : IComponenteValorUnico<string>, ICloneable
    {
        private readonly string _valor;

        private XmlEnvio() { }
        public XmlEnvio(string xmlEnvio)
        {
            _valor = xmlEnvio;
        }

        public string Valor => _valor;

        protected bool Equals(XmlEnvio other)
        {
            return string.Equals(_valor, other._valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((XmlEnvio) obj);
        }

        public override int GetHashCode()
        {
            return (_valor != null ? _valor.GetHashCode() : 0);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}