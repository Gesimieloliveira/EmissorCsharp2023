using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteCancelamento
    {
        public int CteId { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public Cte Cte { get; set; }
        public TipoAmbiente Ambiente { get; set; }
        public short StatusResposta { get; set; }
        public string Justificativa { get; set; }
        public DateTime? OcorreuEm { get; set; }
        public string Motivo { get; set; }
    }
}