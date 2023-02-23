using FusionCore.FusionAdm.Componentes;
using FusionCore.Helpers.Hidratacao;

// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.TyniTypes
{
    public class Complemento : IComponenteValorUnico<string>
    {
        private readonly string _valor;
        public string Valor => _valor;

        public Complemento(string complemneto)
        {
            _valor = complemneto?.TrimOrEmpty() ?? string.Empty;
        }

        protected bool Equals(Complemento other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Complemento) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }
    }
}