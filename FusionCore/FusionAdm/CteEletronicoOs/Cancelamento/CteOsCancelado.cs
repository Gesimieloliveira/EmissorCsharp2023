using System;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.CteEletronicoOs.Cancelamento
{
    public class CteOsCancelado
    {
        public int CteOsId { get; set; }
        public CteOs CteOs { get; set; }
        public TipoAmbiente Ambiente { get; set; }
        public short StatusResposta { get; set; }
        public string Justificativa { get; set; }
        public DateTime? OcorreuEm { get; set; }
        public string Motivo { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
    }
}