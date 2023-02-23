using FusionCore.FusionAdm.Fiscal.Transparencia;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceCodigoIbpt
    {
        public NfceCodigoIbpt()
        {
        }

        public NfceCodigoIbpt(string codigo, TipoIbpt tipo, string excecaoFiscal)
        {
            Codigo = codigo;
            Tipo = tipo;
            ExcecaoFiscal = excecaoFiscal;
        }

        public string Codigo { get; set; }
        public TipoIbpt Tipo { get; set; }
        public string ExcecaoFiscal { get; set; }

        protected bool Equals(NfceCodigoIbpt other)
        {
            return string.Equals(Codigo, other.Codigo) && Tipo == other.Tipo && string.Equals(ExcecaoFiscal, other.ExcecaoFiscal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NfceCodigoIbpt) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Codigo != null ? Codigo.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Tipo;
                hashCode = (hashCode*397) ^ (ExcecaoFiscal != null ? ExcecaoFiscal.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}