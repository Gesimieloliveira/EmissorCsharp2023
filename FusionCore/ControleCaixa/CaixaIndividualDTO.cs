using System;
using FusionCore.Helpers.Basico;

namespace FusionCore.ControleCaixa
{
    public class CaixaIndividualDTO
    {
        public Guid Id { get; set; }
        public EEstadoCaixa Estado { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoCalculado { get; set; }
        public decimal SaldoInformado { get; set; }
        public int OperadorId { get; set; }
        public string NomeOperador { get; set; }
        public ELocalEventoCaixa LocalEvento { get; set; }
        public string LocalEventoTexto => LocalEvento.GetDescription();
    }
}