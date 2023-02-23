using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionValoresPrestacaoServicoCTe
    {
        [XmlElement(ElementName = "vTPrest")]
        public decimal ValorTotal { get; set; }

        [XmlElement(ElementName = "vRec")]
        public decimal ValorAReceber { get; set; }

        [XmlElement("Comp")]
        public List<ComponenteValorPrestacao> ComponenteValorPrestacaos { get; set; }
    }

    [Serializable]
    public class ComponenteValorPrestacao
    {
        [XmlElement("xNome")]
        public string Nome { get; set; }

        [XmlElement("vComp")]
        public decimal Valor { get; set; }
    }
}