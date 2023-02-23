using FusionCore.Core.Flags;
using FusionCore.Core.Tributario;
using FusionCore.Repositorio.Base;

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class PerfilCfopDTO : Entidade
    {
        public short Id { get; set; }
        public CfopDTO Cfop { get; set; }
        public byte Sufixo { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        protected override int ReferenciaUnica => Id;
        public OrigemOperacao Origem => CodigoCfopHelper.ObtemOrigem(Cfop.Id);
        public OpercaoCfop Operacao => CodigoCfopHelper.ObtemOperacao(Cfop.Id);

        public override string ToString()
        {
            return $"{Codigo} - {Descricao}";
        }
    }
}