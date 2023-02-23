using System;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class VendaEcfItemDt : Entidade, IEntidade
    {
        public int Id { get; set; }
        public VendaEcfDt VendaEcfDt { get; set; }
        public EcfDt EcfDt { get; set; }
        public int Coo { get; set; }
        public int Ccf { get; set; }
        public string SerieEcf { get; set; }
        public ProdutoDt ProdutoDt { get; set; }
        public int Cfop { get; set; }
        public string NomeProduto { get; set; }
        public string SiglaUnidadeProduto { get; set; }
        public string CodigoBarra { get; set; }
        public int NumeroItem { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Total { get; set; }
        public decimal BaseIcms { get; set; }
        public decimal TaxaIcms { get; set; }
        public decimal Icms { get; set; }
        public decimal TaxaDesconto { get; set; }
        public decimal Desconto { get; set; }
        public decimal TaxaIssqn { get; set; }
        public decimal Issqn { get; set; }
        public decimal TaxaPis { get; set; }
        public decimal Pis { get; set; }
        public decimal TaxaCofins { get; set; }
        public decimal Cofins { get; set; }
        public decimal TaxaAcrescimo { get; set; }
        public decimal Acrescimo { get; set; }
        public decimal AcrescimoRateio { get; set; }
        public decimal DescontoRateio { get; set; }
        public string TotalizadorParcial { get; set; }
        public string Cst { get; set; }
        public string SituacaoTributariaIcms { get; set; }
        public string IcmsEcf { get; set; }
        public int Cancelado { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public int? IdentificadorRemoto { get; set; }

        public string GetCodigoParaEcf()
        {
            if (string.IsNullOrWhiteSpace(CodigoBarra?.Trim()))
                return ProdutoDt.Id.ToString();

            return CodigoBarra.Trim();
        }

        public string GetNomeProdutoParaEcf()
        {
            return NomeProduto.SubstringWithTrim(0, 30);
        }

        public string GetIcmsParaEcf()
        {
            return IcmsEcf.Trim();
        }

        public decimal GetQuantidadeParaEcf()
        {
            return Quantidade;
        }

        public decimal GetPrecoUnitarioParaEcf()
        {
            return PrecoUnitario;
        }

        public string GetSiglaUnidadeParaEcf()
        {
            return SiglaUnidadeProduto.Trim();
        }

        protected override int ReferenciaUnica => Id;
    }
}