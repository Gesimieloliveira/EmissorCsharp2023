using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInformacaoCTeNormalCTe
    {
        [XmlElement(ElementName = "infCarga")]
        public FusionInformacaoCargaCTe InformacaoCarga { get; set; }

        [XmlElement(ElementName = "infDoc")]
        public FusionInformacaoDocumentoCTe InformacaoDocumento { get; set; }

        [XmlElement(ElementName = "docAnt")]
        public FusionDocumentoAnterior FusionDocumentoAnterior { get; set; }

        [XmlElement(ElementName = "infModal")]
        public FusionInformacaoModalCTe Modal { get; set; }

        [XmlElement(ElementName = "veicNovos")]
        public List<FusionVeiculoTransportadoCTe> VeiculosTransportados { get; set; }

        [XmlElement(ElementName = "infCteSub")]
        public FusionInfCteSub FusionInfCteSub { get; set; }

        public FusionInformacaoCTeNormalCTe()
        {
            InformacaoCarga = new FusionInformacaoCargaCTe();
            InformacaoDocumento = new FusionInformacaoDocumentoCTe();
            Modal = new FusionInformacaoModalCTe();
            VeiculosTransportados = new List<FusionVeiculoTransportadoCTe>();
        }
    }
}