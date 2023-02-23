using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FusionCore.Repositorio.Base;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public class EstadoDTO : Entidade
    {
        public int Id { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public byte CodigoIbge { get; set; }
        protected override int ReferenciaUnica => Id;

        public bool CompareSigla(string sigla)
        {
            return string.Compare(sigla, Sigla, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace) == 0;
        }

        public override string ToString()
        {
            return $"{Nome} - {Sigla}";
        }
    }
}