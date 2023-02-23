using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionRefNF
    {
        [XmlElement(elementName: "CNPJ")]
        public string Cnpj { get; set; }

        [XmlElement(elementName:"CPF")]
        public string Cpf { get; set; }

        [XmlElement(elementName: "mod")]
        public string FusionModeloDocumentoFiscal { get; set; }

        [XmlElement(elementName: "serie")]
        public string Serie { get; set; }

        [XmlElement(elementName: "subserie")]
        public string SubSerie { get; set; }

        [XmlElement(elementName: "nro")]
        public string NumeroDocumentoFiscal { get; set; }

        [XmlElement(elementName: "valor")]
        public decimal Valor { get; set; }

        [XmlElement(elementName: "dEmi")]
        public string EmitidoEm { get; set; }
    }
}