namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceIbpt
    {
        public NfceCodigoIbpt Id { get; set; }

        public string Descricao { get; set; }
        public decimal Nacional { get; set; }
        public decimal Importado { get; set; }
        public decimal Estadual { get; set; }
        public string ChaveIbpt { get; set; }

        public decimal ImpostoFederalAproximado(NfceItem bc)
        {
            return decimal.Round(bc.GetValorBaseCalculo() * Nacional / 100, 2);
        }

        public decimal ImpostoEstadualAproximado(NfceItem bc)
        {
            return decimal.Round(bc.GetValorBaseCalculo() * Estadual / 100, 2);
        }

        protected bool Equals(NfceIbpt other)
        {
            return Equals(Id, other.Id) && string.Equals(Descricao, other.Descricao) && Nacional == other.Nacional && Importado == other.Importado && Estadual == other.Estadual && string.Equals(ChaveIbpt, other.ChaveIbpt);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NfceIbpt) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Descricao != null ? Descricao.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Nacional.GetHashCode();
                hashCode = (hashCode*397) ^ Importado.GetHashCode();
                hashCode = (hashCode*397) ^ Estadual.GetHashCode();
                hashCode = (hashCode*397) ^ (ChaveIbpt != null ? ChaveIbpt.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}