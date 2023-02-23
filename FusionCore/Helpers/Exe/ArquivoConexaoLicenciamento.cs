using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.Helpers.Exe
{
    public static class ArquivoConexaoLicenciamento
    {
        private static readonly string ArquivoConexao;

        static ArquivoConexaoLicenciamento()
        {
            ArquivoConexao = Path.Combine(DiretorioAssembly.GetPastaConfig(), NomeArquivoConexao);
        }

        private static string NomeArquivoConexao => "ConexaoLicenciamento.xml";
        public static bool Existe => File.Exists(ArquivoConexao);

        public static void UpdateConfigExe(string ipServidor, string portaServidor)
        {
            var urlLicenciamento = new UrlLicenciamento
            {
                Porta = int.Parse(portaServidor),
                Servidor = ipServidor
            };

            Salvar(urlLicenciamento);
        }

        private static void Salvar(UrlLicenciamento urlLicenciamento)
        {
            using (var stream = new StreamWriter(ArquivoConexao))
            {
                var xmlSerializer = new XmlSerializer(typeof(UrlLicenciamento));

                xmlSerializer.Serialize(XmlWriter.Create(stream), urlLicenciamento);

                stream.Flush();
            }
        }

        public static UrlLicenciamento LerArquivo()
        {
            UrlLicenciamento conexaoLicenciamento;

            using (var reader = new StreamReader(ArquivoConexao))
            {
                var xmlSerializer = new XmlSerializer(typeof(UrlLicenciamento));

                var objeto = xmlSerializer.Deserialize(XmlReader.Create(reader));

                conexaoLicenciamento = objeto as UrlLicenciamento;
            }

            return conexaoLicenciamento;
        }

        public static void CriaArquivo()
        {
            if (new ManipulaArquivo(ArquivoConexao).NaoExiste())
            {
                Salvar(new UrlLicenciamento());
            }
        }
    }
}