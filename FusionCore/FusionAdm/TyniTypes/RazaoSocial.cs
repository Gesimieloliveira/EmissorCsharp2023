using FusionCore.FusionAdm.Componentes;
using FusionCore.Helpers.Hidratacao;

// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.TyniTypes
{
    public class RazaoSocial : IComponenteValorUnico<string>
    {
        private readonly string _valor;
        public string Valor => _valor;

        public RazaoSocial(string razaoSocial)
        {
            _valor = razaoSocial?.TrimOrEmpty() ?? string.Empty;
        }

        protected bool Equals(RazaoSocial other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RazaoSocial) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }
    }
}