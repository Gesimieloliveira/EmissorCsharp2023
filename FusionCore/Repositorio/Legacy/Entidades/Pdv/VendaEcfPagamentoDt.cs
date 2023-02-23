using System;
using FusionCore.FusionPdv.Flags;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class VendaEcfPagamentoDt : IEntidade
    {
        public VendaEcfPagamentoDt()
        {
            TipoTransacao = TipoTransacao.OutroTipoCartao;
            CodigoAutorizacao = string.Empty;
        }

        public int Id { get; set; }
        public VendaEcfDt VendaEcfDt { get; set; }
        public FormaPagamentoEcfDt FormaPagamentoEcfDt { get; set; }
        public string SerieEcf { get; set; }
        public int Cco { get; set; }
        public int Ccf { get; set; }
        public decimal Valor { get; set; }
        public string Nsu { get; set; }
        public bool Estorno { get; set; }
        public string Rede { get; set; }
        public bool CartaoDebito { get; set; }
        public bool CartaoCredito { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public int? IdentificadorRemoto { get; set; }
        public string CodigoAutorizacao { get; set; }
        public int QuantidadeParcelas { get; set; }
        public DateTime? ComprovanteEmitidoEm { get; set; }
        public decimal Desconto { get; set; }
        public decimal Saque { get; set; }
        public TipoTransacao TipoTransacao { get; set; }
        public TipoParcelamento TipoParcelamento { get; set; }
        public string NomeAdministradora { get; set; }
        public string BandeiraCartao { get; set; }
    }
}