using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte.XmlCte.Processada
{
    [Serializable]
    [XmlRoot(Namespace = "http://www.portalfiscal.inf.br/cte", ElementName = "cteProc")]
    public class FusionCTeProcessamento
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; } = "3.00";

        [XmlElement(ElementName = "CTe")]
        public FusionCTe CTe { get; set; }

        [XmlElement(ElementName = "protCTe")]
        public FusionCteProtocolo Protocolo { get; set; }

        public FusionCTeProcessamento()
        {
            Protocolo = new FusionCteProtocolo();
        }
    }

    [Serializable]
    public class FusionCteProtocolo
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; } = "3.00";

        [XmlElement(ElementName = "infProt")]
        public FusionCteInformacaoProtocolo InformacaoProtocolo { get; set; }

        public FusionCteProtocolo()
        {
            InformacaoProtocolo = new FusionCteInformacaoProtocolo();
        }
    }

    [Serializable]
    public class FusionCteInformacaoProtocolo
    {
        [XmlElement(ElementName = "tpAmb")]
        public FusionTipoAmbienteCTe Ambiente { get; set; }

        [XmlElement(ElementName = "verAplic")]
        public string VersaoAplicativo { get; set; }

        [XmlElement(ElementName = "chCTe")]
        public string Chave { get; set; }

        [XmlElement(ElementName = "dhRecbto")]
        public string ProxyRecebidaEm
        {
            get => RecebidaEm.ToString("yyyy-MM-ddTHH:mm:sszzz");
            set => RecebidaEm = DateTimeOffset.Parse(value);
        }

        [XmlIgnore]
        public DateTimeOffset RecebidaEm { get; set; }

        [XmlElement(ElementName = "nProt")]
        public string Protocolo { get; set; }

        [XmlElement(ElementName = "digVal")]
        public string DigestValue { get; set; }

        [XmlElement(ElementName = "cStat")]
        public short CodigoStatus { get; set; }

        [XmlElement(ElementName = "xMotivo")]
        public string Motivo { get; set; }
    }
}