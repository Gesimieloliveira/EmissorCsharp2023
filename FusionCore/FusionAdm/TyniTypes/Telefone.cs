using FusionCore.FusionAdm.Componentes;
using FusionLibrary.Helper.Diversos;

// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.TyniTypes
{
    public class Telefone : IComponenteValorUnico<string>
    {
        private readonly string _valor;
        public string Valor => _valor;

        public Telefone(string telefone)
        {
            _valor = telefone?.RemoveNaoNumericos() ?? string.Empty;
        }

        private bool Equals(Telefone other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Telefone) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }
    }
}