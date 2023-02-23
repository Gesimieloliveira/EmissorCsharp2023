using System;
using FusionCore.FusionAdm.Financeiro.Flags;

namespace FusionCore.FusionAdm.Financeiro
{
    public class ResumoDocumentoReceberDTO : IDocumentoReceber
    {
        public int Id { get; set; }
        public Situacao Situacao { get; set; }
        public int PessoaId { get; set; }
        public string NomePessoa { get; set; }
        public DateTime? EmitidoEm { get; set; }
        public DateTime Vencimento { get; set; }
        public byte Parcela { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorDocumento { get; set; }
        public decimal ValorRecebido { get; set; }
        public decimal TotalJuros { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal ValorRestante => ValorDocumento + TotalJuros - TotalDesconto - ValorRecebido;
        public decimal ValorRestanteCorrigido => DocumentoReceberHelper.CorrigirValorRestante(this);
        public DateTime? UltimoCalculoJuros { get; set; }
        public bool EstaVencido => Situacao == Situacao.Aberto && Vencimento < DateTime.Today;
        public bool EstaQuitado => Situacao == Situacao.Quitado;
        public bool EstaCancelado => Situacao == Situacao.Cancelado;
        public decimal ValorRestanteVencido => EstaVencido ? ValorRestante : 0.00M;
        public int QuantideDiasVencidos => DocumentoReceberHelper.CalcularQtdeDiasVencidos(this);
        public int MaloteId { get; set; }
        public string Descricao { get; set; }

        public DateTime? DataQuitacao { get; set; }
    }
}