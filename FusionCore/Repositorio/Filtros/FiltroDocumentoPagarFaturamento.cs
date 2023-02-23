using System;
using FusionCore.FusionAdm.Financeiro.Flags;

namespace FusionCore.Repositorio.Filtros
{
    public class FiltroDocumentoPagarFaturamento
    {
        public int PessoaId { get; set; }
        public Situacao? Situacao { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime? DataVencimentoInicial { get; set; }
        public DateTime? DataVencimentoFinal { get; set; }
    }
}