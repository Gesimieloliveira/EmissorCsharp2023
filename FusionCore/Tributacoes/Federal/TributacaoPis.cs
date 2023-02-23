using FusionCore.Core.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.Tributacoes.Federal
{
    public class TributacaoPis : Entidade
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public string DescricaoCompleta => $"{Id} - {Descricao}";
        protected override int ReferenciaUnica => Id.GetHashCode();

        public override string ToString()
        {
            return DescricaoCompleta;
        }
    }
}
