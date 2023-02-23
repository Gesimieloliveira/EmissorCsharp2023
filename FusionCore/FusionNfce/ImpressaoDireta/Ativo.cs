using System;
using System.Xml.Serialization;

namespace FusionCore.FusionNfce.ImpressaoDireta
{
    [Serializable]
    public class ImpressaoDiretaAtiva
    {
        [XmlIgnore]
        public static ImpressaoDiretaAtiva Default => new ImpressaoDiretaAtiva {Ativa = false};

        public bool Ativa { get; set; }

        [XmlIgnore]
        public bool Desativa => !Ativa;
    }
}