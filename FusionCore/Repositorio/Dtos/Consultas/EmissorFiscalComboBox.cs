using System.Diagnostics.CodeAnalysis;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public sealed class EmissorFiscalComboBox
    {
        public byte Id { get; set; }
        public string Descricao { get; set; }
        public bool IsNfe { get; set; }
        public bool IsNfce { get; set; }

        public override string ToString()
        {
            return $"{Descricao}";
        }

        private bool Equals(EmissorFiscalComboBox other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is EmissorFiscalComboBox box && Equals(box);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(EmissorFiscalComboBox left, EmissorFiscalComboBox right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmissorFiscalComboBox left, EmissorFiscalComboBox right)
        {
            return !Equals(left, right);
        }
    }
}