using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.Vendas.Faturamentos
{
    public class FaturamentoSlim : Entidade, IFaturuamentoImprimivel
    {
        private FaturamentoSlim()
        {
            //nhibernate
        }

        protected override int ReferenciaUnica => Id;
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? FinalizadoEm { get; set; }
        public Estado EstadoAtual { get; set; }
        public bool IsFinalizado => EstadoAtual == Estado.Finalizado;
        public string NomeCliente { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal PercentualDesconto { get; set; }
        public decimal Total { get; set; }
        public SituacaoFiscal SituacaoFiscal { get; set; }

        public bool IsNaoEnviado => SituacaoFiscal == SituacaoFiscal.NaoEnviado;

        public bool IsCancelado => SituacaoFiscal == SituacaoFiscal.Cancelado;

        public bool IsAutorizado => SituacaoFiscal == SituacaoFiscal.Autorizado;

        public bool IsAutorizadoDenegada => SituacaoFiscal == SituacaoFiscal.AutorizadoDenegada;

        public bool IsAutorizadoSemInternet => SituacaoFiscal == SituacaoFiscal.AutorizadoSemInternet;

        public bool IsRejeicao => SituacaoFiscal == SituacaoFiscal.ComRejeicao;
    }
}