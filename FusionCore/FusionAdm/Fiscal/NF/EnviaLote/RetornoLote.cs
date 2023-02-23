using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FusionCore.FusionAdm.Fiscal.NF.EnviaLote
{
    [Serializable]
    [XmlRoot(ElementName = "retConsReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class RetornoLote
    {
        public static RetornoLote Carregar(string xmlCompleto)
        {
            using (var reader = new StringReader(xmlCompleto))
            {
                var xmlSerializer = new XmlSerializer(typeof(RetornoLote));

                var objeto = xmlSerializer.Deserialize(reader);

                var retornoLote = objeto as RetornoLote;

                return retornoLote;
            }
        }

        [XmlElement(ElementName = "tpAmb")]
        public int Ambiente { get; set; }

        [XmlElement(ElementName = "verAplic")]
        public string VersaoAplicacao { get; set; }

        [XmlElement(ElementName = "nRec")]
        public string NumeroReciboLote { get; set; }

        [XmlElement(ElementName = "cStat")]
        public int CodigoStatus { get; set; }

        [XmlElement(ElementName = "xMotivo")]
        public string Motivo { get; set; }

        [XmlElement(ElementName = "cUF")]
        public int CodigoUf { get; set; }

        [XmlElement(ElementName = "protNFe")]
        public List<ProtocoloRecebimentoNfe> RetornoLoteNfes { get; set; }
    }
}