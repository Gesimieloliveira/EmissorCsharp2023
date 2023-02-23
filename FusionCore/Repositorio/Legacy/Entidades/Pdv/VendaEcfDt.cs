using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class VendaEcfDt : IEntidade
    {
        public int Id { get; set; }
        public EcfDt EcfDt { get; set; }
        public ClienteDt ClienteDt { get; set; }
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
        public string NomeCliente { get; set; } = string.Empty;
        public string DocumentoCliente { get; set; } = string.Empty;
        public int Cancelado { get; set; }
        public int QuantidadeItens { get; set; }
        public string EnderecoCliente { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;
        public DateTime? AlteradoEm { get; set; }
        public IList<VendaEcfItemDt> VendaEcfItens { get; set; } = new List<VendaEcfItemDt>();
        public IList<VendaEcfPagamentoDt> VendaEcfPagamentos { get; set; }
        public Int32? IdentificadorRemoto { get; set; }
        public IndicadorPagamento IndicadorPagamento { get; set; }

        public string UuidVenda { get; set; }

        public VendaEcfDt Copia()
        {
            return (VendaEcfDt) MemberwiseClone();
        }
    }
}