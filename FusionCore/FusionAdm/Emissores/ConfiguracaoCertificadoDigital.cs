using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Emissores
{
    public class ConfiguracaoCertificadoDigital
    {
        public TipoCertificadoDigital Tipo { get; set; }
        public string Arquivo { get; set; }
        public string Serial { get; set; }
        public string Senha { get; set; }
    }
}