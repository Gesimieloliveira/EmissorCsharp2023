using System;
using System.Xml.Serialization;
using FusionCore.Setup;

namespace FusionCore.FusionNfce.Setup.Conexao
{
    [Serializable]
    public class ConexaoAdm
    {
        [XmlElement("Porta")]
        public short Porta { get; set; } = 1433;

        [XmlElement("Instancia")]
        public string Instancia { get; set; } = "FUSION";

        [XmlElement("BancoDados")]
        public string BancoDados { get; set; } = "FusionAdm";

        [XmlElement("Servidor")]
        public string Servidor { get; set; } = "LOCALHOST";

        [XmlElement("Usuario")]
        public string Usuario { get; set; } = "sa";

        [XmlElement("Senha")]
        public string Senha { get; set; } = "Fusion@ag4";

        public IConexaoCfg ToCfg()
        {
            return new ConexaoCfg
            {
                Servidor = Servidor,
                Instancia = Instancia,
                Porta = Porta,
                BancoDados = BancoDados,
                Senha = Senha,
                Usuario = Usuario
            };
        }
    }
}