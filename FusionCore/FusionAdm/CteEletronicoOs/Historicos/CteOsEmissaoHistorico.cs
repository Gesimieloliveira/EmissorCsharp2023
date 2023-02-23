using System;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Fiscal.Flags;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronicoOs.Historicos
{
    public class CteOsEmissaoHistorico
    {
        public int Id { get; set; }
        public CteOs CteOs { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public TipoEmissao TipoEmissao { get; set; }
        public string Chave { get; set; } = string.Empty;
        public bool Finalizada { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? EnviadoEm { get; set; }
    }
}