using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using FusionCore.Helpers.Ambiente;
using log4net;

namespace FusionCore.FusionAdm.Setup.Conexao
{
    public class ConfiguradorConexao
    {
        private const string NomeArquivoConexao = "Conexao.xml";
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _arquivoConexao;

        public ConfiguradorConexao()
        {
            _arquivoConexao = Path.Combine(DiretorioAssembly.GetPastaConfig(), NomeArquivoConexao);
        }

        public bool ArquivoExiste()
        {
            return File.Exists(_arquivoConexao);
        }

        public void ArmazenaEmArquivo(DadosConexao dadosConexao)
        {
            try
            {
                using (var xmlWriter = new XmlTextWriter(_arquivoConexao, null))
                {
                    xmlWriter.Formatting = Formatting.Indented;

                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Agil4");
                    xmlWriter.WriteStartElement("Conexao");
                    xmlWriter.WriteElementString("Servidor", dadosConexao.Servidor);
                    xmlWriter.WriteElementString("Instancia", dadosConexao.Instancia);
                    xmlWriter.WriteElementString("Porta", dadosConexao.Porta.ToString());
                    xmlWriter.WriteElementString("BancoDados", dadosConexao.BancoDados);
                    xmlWriter.WriteElementString("Usuario", dadosConexao.Usuario);
                    xmlWriter.WriteElementString("Senha", dadosConexao.Senha);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw new ConexaoSetupException(e.Message, e);
            }
        }

        public DadosConexao LerArquivo()
        {
            if (!ArquivoExiste())
            {
                throw new ArquivoNaoExisteException("Arquivo de conexão ainda não foi criado!");
            }

            var conexao = new DadosConexao();

            try
            {
                using (var stream = new FileStream(_arquivoConexao, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new XmlTextReader(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;

                        switch (reader.Name)
                        {
                            case "Servidor":
                                reader.Read();
                                conexao.Servidor = reader.Value;
                                break;
                            case "Usuario":
                                reader.Read();
                                conexao.Usuario = reader.Value;
                                break;
                            case "Senha":
                                reader.Read();
                                conexao.Senha = reader.Value;
                                break;
                            case "Instancia":
                                reader.Read();
                                conexao.Instancia = reader.Value;
                                break;
                            case "BancoDados":
                                reader.Read();
                                conexao.BancoDados = reader.Value;
                                break;
                            case "Porta":
                                reader.Read();

                                if (int.TryParse(reader.Value, out var porta))
                                {
                                    conexao.Porta = porta;
                                }

                                break;
                        }
                    }
                }

                return conexao;
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw new ConexaoSetupException(e.Message, e);
            }
        }

        public string ObterStringConexaoSemPull()
        {
            var conexao = LerArquivo();

            var cfg = conexao;
            var connectionString = new StringBuilder();

            connectionString.Append($"Data Source={cfg.Endpoint};");
            connectionString.Append("Initial Catalog=" + cfg.BancoDados + ";");
            connectionString.Append("Persist Security Info=True;");
            connectionString.Append("User ID=" + cfg.Usuario + ";");
            connectionString.Append("Password=" + cfg.Senha + ";");

            return connectionString.ToString();
        }
    }
}