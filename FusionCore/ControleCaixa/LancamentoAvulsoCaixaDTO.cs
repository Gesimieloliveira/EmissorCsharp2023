using System;
using FusionCore.Core.Flags;
using FusionCore.Helpers.Basico;

namespace FusionCore.ControleCaixa
{
    public class LancamentoAvulsoCaixaDTO
    {
        public Guid Id { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public decimal ValorOperacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Motivo { get; set; }
        public string NomeOperador { get; set; }
        public ELocalEventoCaixa LocalEvento { get; set; }
        public string LocalEventoTexto => LocalEvento.GetDescription();
    }
}