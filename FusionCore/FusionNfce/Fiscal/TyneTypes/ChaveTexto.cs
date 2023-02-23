using System;
using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class ChaveTexto : IComponenteValorUnico<string>, ICloneable
    {
        private readonly string _valor;

        private ChaveTexto(){}

        public ChaveTexto(string chaveTexto)
        {
            _valor = chaveTexto;
        }

        public string Valor => _valor;

        protected bool Equals(ChaveTexto other)
        {
            return string.Equals(_valor, other._valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChaveTexto) obj);
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