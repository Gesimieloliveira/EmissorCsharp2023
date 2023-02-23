using System;
using FusionCore.FusionAdm.Financeiro.Flags;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class DocumentoPagarDTO
    {
        public int Id { get; set; }
        public Situacao Situacao { get; set; }
        public string Numero { get; set; }
        public string NomeFornecedor { get; set; }
        public byte Parcela { get; set; }
        public DateTime VencimentoEm { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorAjustado { get; set; }
        public decimal Juros { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorQuitado { get; set; }
        public string NomeEmpresa { get; set; }
        public string Descricao { get; set; }
    }
}