using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionDocumentoAnterior
    {
        [XmlElement(ElementName = "emiDocAnt")]
        public List<FusionEmiDocAnt> FusionEmiDocAnts { get; set; }
    }

    [Serializable]
    public class FusionEmiDocAnt
    {
        [XmlElement(ElementName = "CNPJ")]
        public string Cnpj { get; set; }

        [XmlElement(ElementName = "CPF")]
        public string Cpf { get; set; }

        [XmlElement(ElementName = "IE")]
        public string IscricaoEstadual { get; set; }

        [XmlElement(ElementName = "UF")]
        public string SiglaUF { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string RazaoSocialOuNomeExpedidor { get; set; }

        [XmlElement(ElementName = "idDocAnt")]
        public List<FusionIdDocAnt> FusionIdDocAnt { get; set; }

    }
}