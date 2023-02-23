using System;
using FusionCore.FusionAdm.Financeiro.Flags;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class DocumentoReceberGridDto
    {
        public int Id { get; set; }
        public Situacao Situacao { get; set; }
        public string NumeroDocumento { get; set; }
        public string PessoaNome { get; set; }
        public string Descricao { get; set; }
        public DateTime? EmitidoEm { get; set; }
        public DateTime? Vencimento { get; set; }
        public byte Parcela { get; set; }
        public string TipoDocumentoDescricao { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorAjustado { get; set; }
        public decimal Juros { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorQuitado { get; set; }
    }
}