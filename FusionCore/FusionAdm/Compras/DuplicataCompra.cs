using System;

namespace FusionCore.FusionAdm.Compras
{
    public class DuplicataCompra
    {
        public int Id { get; set; }
        public NotaFiscalCompra NfCompra { get; set; }
        public string Numero { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Valor { get; set; }
    }
}