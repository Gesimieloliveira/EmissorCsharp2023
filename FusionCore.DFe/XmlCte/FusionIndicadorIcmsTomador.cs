using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    public enum FusionIndicadorIcmsTomador
    {
        [XmlEnum("1")]
        ContribuinteIcms = 1,
        [XmlEnum("2")]
        ContribuienteIsentoDeIE = 2,
        [XmlEnum("9")]
        NaoContribuinte = 9
    }
}