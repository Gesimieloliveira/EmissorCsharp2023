using System;
using System.Xml.Serialization;

namespace FusionCore.Helpers.Exe
{
    [Serializable]
    [XmlRoot(Namespace = "www.sistemafusion.com.br", ElementName = "Fusion")]
    public class UrlLicenciamento
    {
        public UrlLicenciamento()
        {
            Servidor = "LOCALHOST";
            Porta = 8561;
        }

        [XmlElement("Porta")]
        public int Porta { get; set; }

        [XmlElement("Servidor")]
        public string Servidor { get; set; }
    }
}