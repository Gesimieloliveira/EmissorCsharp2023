using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsEmissaoFinalizada
    {
        public int CteOsId { get; set; }
        public CteOs CteOs { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public TipoEmissao TipoEmissao { get; set; }
        public string Chave { get; set; }
        public bool Autorizado { get; set; }
        public string XmlAutorizado { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? EnviadoEm { get; set; }
        public string Protocolo { get; set; }
    }
}