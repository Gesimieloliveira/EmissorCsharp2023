using System;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class Contingencia
    {
        private readonly string _justificativa;
        private readonly DateTime? _entrouEm;

        private Contingencia() { }

        private Contingencia(string justificativa, DateTime? entrouEm)
        {
            _justificativa = justificativa;
            _entrouEm = entrouEm;
        }

        public string Justificativa => _justificativa;

        public DateTime? EntrouEm => _entrouEm;

        public Builder ToBuilder()
        {
            return new Builder(this);
        }

        protected bool Equals(Contingencia other)
        {
            return string.Equals(_justificativa, other._justificativa) && _entrouEm.Equals(other._entrouEm);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Contingencia) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_justificativa != null ? _justificativa.GetHashCode() : 0)*397) ^ _entrouEm.GetHashCode();
            }
        }

        public sealed class Builder
        {
            private string Justificativa { get; set; }
            private DateTime? EntrouEm { get; set; }

            public Builder() { }

            protected internal Builder(Contingencia contingencia)
            {
                Justificativa = contingencia.Justificativa;
                EntrouEm = contingencia.EntrouEm;
            }

            public Builder ComJustificativa(string justificativa)
            {
                Justificativa = justificativa.TrimOrEmpty();
                return this;
            }

            public Builder EntrouNaData(DateTime? entrouEm)
            {
                EntrouEm = entrouEm;
                return this;
            }

            public static implicit operator Contingencia(Builder builder)
            {
                var contingencia = new Contingencia(builder.Justificativa, builder.EntrouEm);

                return contingencia;
            }
        }
    }
}