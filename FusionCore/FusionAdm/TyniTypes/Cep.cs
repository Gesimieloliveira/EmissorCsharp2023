using FusionCore.FusionAdm.Componentes;
using FusionLibrary.Helper.Diversos;

// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.TyniTypes
{
    public class Cep : IComponenteValorUnico<string>
    {
        private readonly string _valor;
        public string Valor => _valor;

        public Cep(string cep)
        {
            _valor = cep?.RemoveNaoNumericos() ?? string.Empty;
        }

        protected bool Equals(Cep other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Cep) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }
    }
}