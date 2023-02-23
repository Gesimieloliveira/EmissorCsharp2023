using System.Diagnostics.CodeAnalysis;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;

namespace FusionCore.FusionAdm.Fiscal.NF
{
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class ReferenciaNfe
    {
        public int Id { get; private set; }
        public Nfeletronica Nfe { get; private set; }
        public string ChaveReferenciada { get; set; }

        private ReferenciaNfe()
        {
            //nhibernate
        }

        public ReferenciaNfe(Nfeletronica nfe, ChaveSefaz chave) : this()
        {
            Nfe = nfe;
            ChaveReferenciada = chave.Chave;
        }

        private bool Equals(ReferenciaNfe other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ReferenciaNfe) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(ReferenciaNfe left, ReferenciaNfe right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReferenciaNfe left, ReferenciaNfe right)
        {
            return !Equals(left, right);
        }
    }
}