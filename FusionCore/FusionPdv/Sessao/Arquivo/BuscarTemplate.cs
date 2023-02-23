using System.IO;
using System.Windows.Forms;
using FusionLibrary.Helper.Criptografia;
using Newtonsoft.Json;

namespace FusionCore.FusionPdv.Sessao.Arquivo
{
    public abstract class BuscarTemplate
    {
        protected string CaminhoArquivo = Application.StartupPath + "\\ArquivoAuxiliar.agil4";
        private string _dado;
        public bool NaoCriptografar { get; set; }

        private void ExisteArquivo()
        {
            if (!File.Exists(CaminhoArquivo)) ExceptionValidacaoVazio("Não existe o arquivo, ArquivoAuxiliar.agil4");
        }

        public string Executar()
        {
            ExisteArquivo();

            using (var stream = File.Open(CaminhoArquivo, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    var linha = reader.ReadLine();

                    while (linha != null && !reader.EndOfStream)
                    {
                        linha += reader.ReadLine();
                    }

                    dynamic json = NaoCriptografar ? JsonConvert.DeserializeObject(linha) : JsonConvert.DeserializeObject(new DesencriptaBase64(linha).Processa());

                    _dado = RetornoDado(json);

                    if (string.IsNullOrEmpty(_dado)) ExceptionValidacaoVazio("A informação buscada no arquivo não existe.");
                }
            }

            return _dado;
        }

        public abstract string RetornoDado(dynamic json);

        public abstract void ExceptionValidacaoVazio(string mensagem);
    }
}
