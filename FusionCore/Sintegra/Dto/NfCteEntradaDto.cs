using System;
using FusionCore.FusionAdm.EntradaOutras;

namespace FusionCore.Sintegra.Dto
{
    public class NfCteEntradaDto
    {
        public int Id { get; set; }
        public string NomeEmpresa { get; set; }
        public string CnpjEmpresa { get; set; }
        public ModeloDocumentoCteEntrada ModeloDocumento { get; set; } = ModeloDocumentoCteEntrada.Cte;
        public SituacaoFiscal SituacaoFiscal { get; set; } = SituacaoFiscal.Normal;
        public DateTime EmissaoEm { get; set; } = DateTime.Now;
        public DateTime UtilizacaoEm { get; set; } = DateTime.Now;
        public short Serie { get; set; }
        public short Subserie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
    }
}