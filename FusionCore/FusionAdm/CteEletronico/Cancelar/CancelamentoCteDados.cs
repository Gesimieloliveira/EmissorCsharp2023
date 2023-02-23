using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Cancelar
{
    public class CancelamentoCteDados
    {
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public object Cte { get; set; }
        public TipoAmbiente TipoAmbiente { get; set; }
        public short StatusResposta { get; set; }
        public string Justificativa { get; set; }
        public DateTime? OcorreuEm { get; set; }
        public string Motivo { get; set; }
    }
}