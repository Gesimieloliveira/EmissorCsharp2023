using System.Diagnostics.CodeAnalysis;
using static System.String;

namespace FusionCore.FusionAdm.Componentes
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public class DocumentoRg
    {
        public string Rg { get; private set; }
        public string OrgaoRg { get; private set; }
        public static DocumentoRg Vazio => new DocumentoRg();

        private DocumentoRg()
        {
            Rg = Empty;
            OrgaoRg = Empty;
        }

        public DocumentoRg(string rg, string orgaoRg)
        {
            Rg = rg?.Trim() ?? Empty;
            OrgaoRg = orgaoRg?.Trim() ?? Empty;
        }

        private bool Equals(DocumentoRg other)
        {
            return string.Equals(Rg, other.Rg) && string.Equals(OrgaoRg, other.OrgaoRg);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DocumentoRg) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Rg?.GetHashCode() ?? 0)*397) ^ (OrgaoRg?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(DocumentoRg left, DocumentoRg right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DocumentoRg left, DocumentoRg right)
        {
            return !Equals(left, right);
        }
    }
}