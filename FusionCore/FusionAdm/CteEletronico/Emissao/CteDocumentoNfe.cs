using System;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteDocumentoNfe
    {
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public string Chave { get; set; }
        public int PinSuframa { get; set; }
        public DateTime? PrevisaoEntregaEm { get; set; }
        public decimal Valor { get; set; }
    }
}