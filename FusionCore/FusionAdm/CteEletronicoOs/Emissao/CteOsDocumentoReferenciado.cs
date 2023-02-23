using System;

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsDocumentoReferenciado
    {
        public int Id { get; set; }
        public CteOs CteOs { get; set; }
        public string Numero { get; set; } = string.Empty;
        public short? Serie { get; set; }
        public short? SubSerie { get; set; }
        public DateTime EmitidaEm { get; set; } = DateTime.Now;
        public decimal? Valor { get; set; }
    }
}