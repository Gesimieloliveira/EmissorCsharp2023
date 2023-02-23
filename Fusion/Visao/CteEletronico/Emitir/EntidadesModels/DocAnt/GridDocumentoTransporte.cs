using System;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels.DocAnt
{
    public class GridDocumentoTransporte
    {
        public GridDocumentoTransporte()
        {
            IsCTe = TipoDocumentoAnterior == TipoDocumentoAnterior.CTe;
            IsNotCTe = !IsCTe;
        }

        public TipoDocumentoAnterior TipoDocumentoAnterior { get; set; }

        public short Serie { get; set; }

        public short SubSerie { get; set; }

        public string NumeroDocumentoFiscal { get; set; }

        public DateTime DataDeEmissao { get; set; }

        public string ChaveCTe { get; set; }

        public CteDocumentoTransporte CteDocumentoTransporte { get; set; }

        public bool IsCTe { get; set; }

        public bool IsNotCTe { get; set; }
    }
}