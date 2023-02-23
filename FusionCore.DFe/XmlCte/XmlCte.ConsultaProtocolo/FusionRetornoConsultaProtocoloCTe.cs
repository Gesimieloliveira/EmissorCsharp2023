using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using FusionCore.DFe.XmlCte.XmlCte.RegistroEventos;

namespace FusionCore.DFe.XmlCte.XmlCte.ConsultaProtocolo
{
    [Serializable]
    [XmlRoot(Namespace = "http://www.portalfiscal.inf.br/cte",
        ElementName = "retConsSitCTe")]
    public class FusionRetornoConsultaProtocoloCTe
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        [XmlElement(ElementName = "tpAmb")]
        public FusionTipoAmbienteCTe Ambiente { get; set; }

        [XmlElement(ElementName = "verAplic")]
        public string VersaoAplicacao { get; set; }

        [XmlElement(ElementName = "cStat")]
        public short CodigoStatus { get; set; }

        [XmlElement(ElementName = "xMotivo")]
        public string Motivo { get; set; }

        [XmlElement(ElementName = "cUF")]
        public byte CodigoEstadoUf { get; set; }

        [XmlElement(ElementName = "protCTe")]
        public FusionProtocoloAutorizacaoOuDenegacaoCTe ProtocoloAutorizacaoOuDenegacao { get; set; }

        [XmlElement("procEventoCTe")]
        public List<FusionProcEventoCTe> FusionProcEventoCTe { get; set; }

        [XmlIgnore]
        public XmlDocument Xml { get; set; }
    }

    [Serializable]
    public class FusionProcEventoCTe
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        [XmlElement(ElementName = "eventoCTe")]
        public FusionRegistroEventoCTe FusionEventoCTe { get; set; }

        [XmlElement(ElementName = "retEventoCTe")]
        public FusionRetornoRegistroEventoCTe FusionRetornoEventoCTe { get; set; }

    }

    [Serializable]
    public class FusionProtocoloAutorizacaoOuDenegacaoCTe
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        [XmlElement(ElementName = "CTe")]
        public FusionCTe CTe { get; set; }
    }
}