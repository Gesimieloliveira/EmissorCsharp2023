using System;
using System.Xml.Serialization;

namespace FusionCore.FusionAdm.Fiscal.NF.EnviaLote
{
    [Serializable]
    [XmlRoot(ElementName = "protNFe")]
    public class ProtocoloRecebimentoNfe
    {
        [XmlElement("infProt")]
        public InformacaoProtocoloResposta InformacaoProtocoloResposta { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "infProt")]
    public class InformacaoProtocoloResposta
    {
        [XmlElement("tpAmb")]
        public int Ambiente { get; set; }

        [XmlElement("verAplic")]
        public string VersaoAplicacao { get; set; }

        [XmlElement("chNFe")]
        public string Chave { get; set; }

        [XmlElement("dhRecbto")]
        public DateTime? DataEHoraDoProcessamento { get; set; }

        [XmlElement("nProt")]
        public string NumeroProtocolo { get; set; }

        [XmlElement("digVal")]
        public string DigestValue { get; set; }

        [XmlElement("cStat")]
        public int CodigoStatus { get; set; }

        [XmlElement("xMotivo")]
        public string Motivo { get; set; }

        [XmlIgnore]
        public bool Autorizado => CodigoStatus == 100 || CodigoStatus == 150 || CodigoStatus == 110;

        [XmlIgnore]
        public bool IsDuplicidade => CodigoStatus == 204;
    }
}