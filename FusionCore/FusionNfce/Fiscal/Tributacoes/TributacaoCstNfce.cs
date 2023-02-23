using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Flags;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class TributacaoCstNfce : Entidade
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public RegimeTributario RegimeTributario { get; set; }
        protected override int ReferenciaUnica => int.Parse(Id);


        public override string ToString()
        {
            return $"{Id} - {Descricao}";
        }
    }
}