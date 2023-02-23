using System;
using System.Xml.Serialization;

namespace FusionCore.FusionNfce.Setup.Conexao.Entidade
{
    [Serializable]
    [XmlRoot(Namespace = "www.sistemafusion.com.br", ElementName = "Fusion")]
    public class ConexaoBancoDados
    {
        [XmlElement("ConexaoAdm")]
        public ConexaoAdm ConexaoAdm { get; set; }

        [XmlElement("ConexaoNfce")]
        public ConexaoNfce ConexaoNfce { get; set; }

        public ConexaoBancoDados()
        {
        }

        public ConexaoBancoDados(ConexaoAdm conexaoAdm, ConexaoNfce conexaoNfce)
        {
            ConexaoAdm = conexaoAdm;
            ConexaoNfce = conexaoNfce;
        }
    }
}