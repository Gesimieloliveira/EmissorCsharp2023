using System;
using FusionCore.FusionAdm.CteEletronico.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteDocumentoOutro
    {
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string DescricaoOutro { get; set; }
        public string Numero { get; set; }
        public DateTime? EmitidoEm { get; set; }
        public decimal Valor { get; set; }
        public DateTime? PrevisaoEntregaEm { get; set; }
    }
}