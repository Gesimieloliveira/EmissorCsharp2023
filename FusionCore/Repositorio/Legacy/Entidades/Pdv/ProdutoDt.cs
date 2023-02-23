using System.Collections.Generic;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class ProdutoDt : Entidade, IEntidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal PrecoVenda { get; set; }
        public int Ativo { get; set; }
        public decimal AliquotaIcmsPaf { get; set; }
        public string SituacaoTributariaIcms { get; set; }
        public string IcmsEcf { get; set; }
        public string SiglaUnidade { get; set; }
        public int PodeFracionar { get; set; }
        public string CodigoNcm { get; set; }
        public decimal TributacaoNacional { get; set; }
        public decimal TributacaoImportado { get; set; }
        public decimal TributacaoEstadual { get; set; }
        public string ChaveIbpt { get; set; }
        public decimal Estoque { get; set; }
        public bool SolicitaTotal { get; set; }
        public int CodigoBalanca { get; set; }
        public IList<ProdutoAliasDt> ProdutosAlias { get; set; } = new List<ProdutoAliasDt>();
        protected override int ReferenciaUnica => Id;
    }
}