using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    public enum FusionTipoDocumentoTransAnt
    { 
        [XmlEnum("07")]
        ATRE = 07,

        [XmlEnum("08")]
        DTA = 08,

        [XmlEnum("09")]
        ConhecimentoAereoInternacional = 09,

        [XmlEnum("10")]
        ConhecimentoCartaDePorteInternacional = 10,

        [XmlEnum("11")]
        ConhecimentoAvulso = 11,

        [XmlEnum("12")]
        TIF = 12,

        [XmlEnum("13")]
        BL = 13
    }
}