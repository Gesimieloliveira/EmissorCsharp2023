using System.Diagnostics.CodeAnalysis;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedMember.Global

namespace FusionCore.Tributacoes.Estadual
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class TributacaoIcms : Entidade
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool IsNFe { get; private set; }
        public bool IsCTe { get; private set; }
        public bool IsCTeOs { get; set; }
        public string DescricaoCompleta => $"{Codigo} - {Descricao}";
        protected override int ReferenciaUnica => Codigo?.GetHashCode() ?? 0;

        public override string ToString()
        {
            return $"{Codigo} - {Descricao}";
        }

        public bool IcmsOpcional()
        {
            return Codigo == "90";
        }

        public string ConverteParaAliquotaEcf(decimal aliquota)
        {
            return TributacaoIcmsConversores.ConverteParaAliquotaEcf(this, aliquota);
        }
    }
}
