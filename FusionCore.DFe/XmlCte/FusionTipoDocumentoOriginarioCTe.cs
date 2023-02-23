using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    public enum FusionTipoDocumentoOriginarioCTe
    {
        [XmlEnum("00")]
        Declaracao,

        [XmlEnum("10")]
        Dutoviario,

        [XmlEnum("59")]
        CFeSAT,

        [XmlEnum("65")]
        NFCe,

        [XmlEnum("99")]
        Outros
    }
}