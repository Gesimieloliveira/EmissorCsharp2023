using System;
using FusionCore.FusionAdm.CteEletronico.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteDocumentoTransporte
    {
        public int Id { get; set; }

        public CteDocumentoAnterior CteDocumentoAnterior { get; set; }

        public TipoDocumentoAnterior TipoDocumentoAnterior { get; set; }

        public short Serie { get; set; }

        public short SubSerie { get; set; }

        public string NumeroDocumentoFiscal { get; set; }

        public DateTime EmissaoEm { get; set; }

        public string ChaveCTe { get; set; }
    }
}