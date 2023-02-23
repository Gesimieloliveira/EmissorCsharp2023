using FusionCore.FusionAdm.Componentes;
using FusionCore.Helpers.Hidratacao;

// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.TyniTypes
{
    public class Numero : IComponenteValorUnico<string>
    {
        private readonly string _valor;
        public string Valor => _valor;

        public Numero(string numero)
        {
            _valor = numero?.TrimOrEmpty() ?? string.Empty;
        }

        protected bool Equals(Numero other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Numero) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }
    }
}