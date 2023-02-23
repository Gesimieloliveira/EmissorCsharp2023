using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.Produtos
{
    public class ProdutoGrid : Entidade
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal MargemLucro { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal Estoque { get; set; }
        public string CstIcms { get; set; }
        public decimal AliquotaIcms { get; set; }
        public string CstIpi { get; set; }
        public decimal AliquotaIpi { get; set; }
        public string CstPis { get; set; }
        public decimal AliquotaPis { get; set; }
        public string CstCofins { get; set; }
        public decimal AliquotaCofins { get; set; }
        public string Unidade { get; set; }
        public string Grupo { get; set; }
        public string Ncm { get; set; }
        public string Cest { get; set; }
        public string ReferenciaInterna { get; set; }

        protected override int ReferenciaUnica => Id;

        public override string ToString()
        {
            return $"{Id}  - {Nome}";
        }
    }
}
