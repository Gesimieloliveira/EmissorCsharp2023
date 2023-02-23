using System;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class PdvVendaDTO : IEntidade
    {
        public int Id { get; set; }
        public PdvEcfDTO PdvEcfDTO { get; set; }
        public Cliente PessoaDTO { get; set; }
        public string SerieEcf { get; set; }
        public int Cfop { get; set; }
        public int Coo { get; set; }
        public int Ccf { get; set; }
        public DateTime? VendidoEm { get; set; }
        public decimal TotalFinal { get; set; }
        public decimal TotalCupom { get; set; }
        public decimal TaxaDesconto { get; set; }
        public decimal Desconto { get; set; }
        public decimal TaxaAcrescimo { get; set; }
        public decimal Acrescimo { get; set; }
        public decimal TotalRecebido { get; set; }
        public decimal Troco { get; set; }
        public decimal TotalCancelado { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalBaseIcms { get; set; }
        public decimal AcrescimoItens { get; set; }
        public decimal DescontoItens { get; set; }
        public int Status { get; set; }
        public string NomeCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public int Cancelado { get; set; }
        public int QuantidadeItens { get; set; }
        public string EnderecoCliente { get; set; }
        public string Observacao { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public Malote Malote { get; set; }
        public IndicadorPagamento IndicadorPagamento { get; set; }
        public string UuidVenda { get; set; }

        // ReSharper disable once EmptyConstructor
        public PdvVendaDTO()
        {
        }
    }
}