using System;
using System.IO;
using System.Xml.Serialization;

namespace FusionCore.FusionAdm.Fiscal.NF.EnviaLote
{
    public static class ExtensionApenasUmaNfce
    {

        public static bool ApenasUmaNfce(this RespostaEnvioDeLote respostaEnvioDeLote) =>
            respostaEnvioDeLote.CodigoStatus == 452;

    }

    [Serializable]
    [XmlRoot(ElementName = "retEnviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class RespostaEnvioDeLote
    {
        public static RespostaEnvioDeLote Carregar(string xmlCompleto)
        {
            using (var reader = new StringReader(xmlCompleto))
            {
                var xmlSerializer = new XmlSerializer(typeof(RespostaEnvioDeLote));

                var objeto = xmlSerializer.Deserialize(reader);

                var retornoLote = objeto as RespostaEnvioDeLote;
                return retornoLote;
            }
        }

        [XmlElement("tpAmb")]
        public int Ambiente { get; set; }

        [XmlElement("verAplic")]
        public string VersaoAplicacao { get; set; }

        [XmlElement("cStat")]
        public short CodigoStatus { get; set; }

        [XmlElement("xMotivo")]
        public string Motivo { get; set; }

        [XmlElement("cUF")]
        public byte CodigoUf { get; set; }

        [XmlElement("dhRecbto")]
        public DateTime? DataEHoraDoProcessamento { get; set; }

        [XmlElement("infRec")]
        public DadosRecebidoDoLote DadosRecebidoDoLote { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "infRec")]
    public class DadosRecebidoDoLote
    {
        [XmlElement("nRec")]
        public string NumeroRecibo { get; set; }

        [XmlElement("tMed")]
        public int TempoMedioResposta { get; set; }
    }
}