using System.Diagnostics.CodeAnalysis;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.Tributacoes.Federal
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public class TributacaoIpi : Entidade
    {
        public string Codigo { get; set; }
        public string Descricao { get; private set; }
        public TipoOperacao TipoOperacao { get; set; }
        public string DescricaoCompleta => $"{Codigo} - {Descricao}";
        protected override int ReferenciaUnica => Codigo.GetHashCode();

        public override string ToString()
        {
            return DescricaoCompleta;
        }
    }
}
