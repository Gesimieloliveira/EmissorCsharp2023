using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.NfceSincronizador.SincronizacaoEntidade
{
    public class SincronizacaoPendenteNfce
    {
        public string Referencia { get; set; }
        public EntidadeSincronizavel Entidade { get; set; }

        protected bool Equals(SincronizacaoPendenteNfce other)
        {
            return string.Equals(Referencia, other.Referencia) && Entidade == other.Entidade;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SincronizacaoPendenteNfce) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Referencia != null ? Referencia.GetHashCode() : 0)*397) ^ (int) Entidade;
            }
        }
    }
}