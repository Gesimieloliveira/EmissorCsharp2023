using System.Diagnostics.CodeAnalysis;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.NfceSincronizador.SincronizacaoEntidade
{
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public class SincronizacaoPendente
    {
        public string Referencia { get; set; }
        public byte TerminalOfflineId { get; set; }
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; }

        public override bool Equals(object obj)
        {
            var t = obj as SincronizacaoPendente;

            if (t == null)
            {
                return false;
            }

            return Referencia == t.Referencia
                   && TerminalOfflineId == t.TerminalOfflineId
                   && EntidadeSincronizavel == t.EntidadeSincronizavel;
        }

        public override int GetHashCode()
        {
            var hash = GetType().GetHashCode();
            hash = (hash*397) ^ Referencia.GetHashCode();
            hash = (hash*397) ^ TerminalOfflineId.GetHashCode();
            hash = (hash*397) ^ EntidadeSincronizavel.GetHashCode();

            return hash;
        }
    }
}