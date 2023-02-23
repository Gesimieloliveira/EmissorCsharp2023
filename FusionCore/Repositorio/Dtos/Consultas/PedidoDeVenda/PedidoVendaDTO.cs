using System;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda
{
    public sealed class PedidoVendaDTO
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public int IdCliente { get; set; }
        public EstadoAtual EstadoAtual { get; set; }
        public string Referencia { get; set; }
        public DateTime CriadoEm { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal PercentualDesconto { get; set; }
        public decimal Total { get; set; }
        public TipoPedido TipoPedido { get; set; }

        public bool IsFaturado => EstadoAtual == EstadoAtual.Faturado;
        public bool IsCancelado => EstadoAtual == EstadoAtual.Cancelado;
        public string NomeVisitante { get; set; }
        public bool IsFinalizado => EstadoAtual == EstadoAtual.Finalizado;

        public bool IsTemVisitante()
        {
            return NomeVisitante.IsNotNullOrEmpty();
        }
    }
}