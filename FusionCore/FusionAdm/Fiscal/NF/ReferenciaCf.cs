using System.Diagnostics.CodeAnalysis;

namespace FusionCore.FusionAdm.Fiscal.NF
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public class ReferenciaCf
    {
        public int Id { get; private set; }
        public Nfeletronica Nfe { get; private set; }
        public string ModeloCupom { get; private set; } = "2D";
        public int NumeroEcf { get; set; }
        public int NumeroCoo { get; set; }

        private ReferenciaCf()
        {
            //nhibernate
        }

        public ReferenciaCf(Nfeletronica nfe, int numeroEcf, int numeroCoo) : this()
        {
            Nfe = nfe;
            NumeroEcf = numeroEcf;
            NumeroCoo = numeroCoo;
        }

        private bool Equals(ReferenciaCf other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ReferenciaCf) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(ReferenciaCf left, ReferenciaCf right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReferenciaCf left, ReferenciaCf right)
        {
            return !Equals(left, right);
        }
    }
}