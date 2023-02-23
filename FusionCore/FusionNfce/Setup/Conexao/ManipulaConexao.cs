using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FusionCore.FusionNfce.Setup.Conexao.Entidade;
using FusionCore.FusionNfce.Setup.Conexao.Excecao;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.FusionNfce.Setup.Conexao
{
    public class ManipulaConexao
    {
        private readonly string _arquivoConexao;

        public ManipulaConexao()
        {
            _arquivoConexao = Path.Combine(DiretorioAssembly.GetPastaConfig(), "Conexao.xml");
        }

        public bool ArquivoExiste()
        {
            return File.Exists(_arquivoConexao);
        }

        public void SalvaXml(ConexaoBancoDados conexaoBancoDados)
        {
            using (var stream = new StreamWriter(_arquivoConexao))
            {
                var xmlSerializer = new XmlSerializer(typeof (ConexaoBancoDados));

                xmlSerializer.Serialize(XmlWriter.Create(stream), conexaoBancoDados);

                stream.Flush();
            }
        }

        public ConexaoBancoDados CriarArquivo()
        {
            try
            {
                var conexao = new ConexaoBancoDados(new ConexaoAdm(), new ConexaoNfce());

                SalvaXml(conexao);
                return conexao;
            }
            catch (Exception ex)
            {
                throw new ConexaoBancoDadosNfceException(ex.Message, ex);
            }
        }

        public ConexaoBancoDados LerArquivo()
        {
            try
            {
                ConexaoBancoDados conexao;

                using (var reader = new StreamReader(_arquivoConexao))
                {
                    var xmlSerializer = new XmlSerializer(typeof (ConexaoBancoDados));

                    var objeto = xmlSerializer.Deserialize(XmlReader.Create(reader));

                    conexao = objeto as ConexaoBancoDados;
                }

                return conexao;
            }
            catch (Exception e)
            {
                throw new ConexaoBancoDadosNfceException(e.Message, e);
            }
        }
    }
}