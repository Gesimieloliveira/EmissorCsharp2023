using FusionCore.FusionAdm.Componentes;
using FusionCore.Helpers.ChaveFiscal;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class CodigoNumerico : IComponenteValorUnico<int>
    {
        private readonly int _valor;
        private CodigoNumerico() : this(0) { }

        public CodigoNumerico(int codigoNumerico)
        {
            _valor = codigoNumerico;
        }

        public static CodigoNumerico RandomCodigoNumerico => new CodigoNumerico(GerarNumerosAleatoriosFiscal.GetRandom());

        public int Valor => _valor;

        private bool Equals(CodigoNumerico other)
        {
            return _valor == other._valor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CodigoNumerico) obj);
        }

        public override int GetHashCode()
        {
            return _valor;
        }
    }
}