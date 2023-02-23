using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FusionCore.FusionPdv.Models;
using FusionCore.Helpers.Ambiente;

namespace FusionPdv.Servicos.Tef
{
    public class ManipulaTef
    {
        private readonly string _arquivoTef;

        public ManipulaTef()
        {
            _arquivoTef = Path.Combine(DiretorioAssembly.GetPastaConfig(), "Tef.xml");
        }

        private bool ArquivoExiste()
        {
            return File.Exists(_arquivoTef);
        }

        public void CriarArquivoSeNaoExistir()
        {
            if (ArquivoExiste()) return;

            var tef = new EntidadeTef();

            SalvaXml(tef);
        }

        public void SalvaXml(EntidadeTef tef)
        {
            using (var stream = new StreamWriter(_arquivoTef))
            {
                var xmlSerializer = new XmlSerializer(typeof (EntidadeTef));

                xmlSerializer.Serialize(XmlWriter.Create(stream), tef);

                stream.Flush();
            }
        }

        public EntidadeTef LerArquivo()
        {
            EntidadeTef conexao;

            using (var reader = new StreamReader(_arquivoTef))
            {
                var xmlSerializer = new XmlSerializer(typeof (EntidadeTef));

                var objeto = xmlSerializer.Deserialize(XmlReader.Create(reader));

                conexao = objeto as EntidadeTef;
            }

            return conexao;
        }
    }
}