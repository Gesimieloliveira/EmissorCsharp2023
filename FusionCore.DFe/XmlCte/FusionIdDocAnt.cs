using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionIdDocAnt
    {
        [XmlElement(ElementName = "idDocAntPap")]
        public List<FusionIdDocAntPap> FusionIdDocAntPaps { get; set; }

        [XmlElement(ElementName = "idDocAntEle")]
        public List<FusionIdDocAntEle> FusionIdDocAntEles { get; set; }
    }

    [Serializable]
    public class FusionIdDocAntPap
    {
        [XmlElement(ElementName = "tpDoc")]
        public FusionTipoDocumentoTransAnt FusionTipoDocumentoTransAnt { get; set; }

        [XmlElement(ElementName = "serie")]
        public short Serie { get; set; }

        [XmlElement(ElementName = "subser")]
        public short? SubSerie { get; set; }

        public bool SubSerieSpecified => SubSerie.HasValue;

        [XmlElement(ElementName = "nDoc")]
        public string NumeroDocumentoFiscal { get; set; }

        [XmlIgnore]
        public DateTime DataEmissao { get; set; }

        [XmlElement(ElementName = "dEmi")]
        public string ProxyDataEmissao
        {
            get { return DataEmissao.ToString("yyyy-MM-dd"); }
            set { DataEmissao = DateTime.Parse(value); }
        }
    }
}