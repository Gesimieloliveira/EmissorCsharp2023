using System;
using System.IO;
using System.Windows.Forms;
using FusionLibrary.Helper.Criptografia;
using Newtonsoft.Json;

namespace FusionCore.FusionPdv.Sessao.Arquivo
{
    public abstract class AtualizarTemplate
    {
        protected string CaminhoArquivo = Application.StartupPath + "\\ArquivoAuxiliar.agil4";
        private dynamic _json;

        private void ExisteArquivo()
        {
            if (!File.Exists(CaminhoArquivo)) throw new InvalidOperationException("Não existe o arquivo, ArquivoAuxiliar.agil4");
        }

        public void Executar()
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

                    _json = NaoCriptografar ? JsonConvert.DeserializeObject(linha) : JsonConvert.DeserializeObject(new DesencriptaBase64(linha).Processa());

                    ManipulaJson(_json);
                }
            }

            PreparaArquioParaEscrever();
        }

        public bool NaoCriptografar { get; set; }

        private void PreparaArquioParaEscrever()
        {
            ExisteArquivo();

            using (var stream = File.Open(CaminhoArquivo, FileMode.Create))
            {
                using (var write = new StreamWriter(stream))
                {
                    EscreveNoArquivo(write);
                }
            }
        }

        private void EscreveNoArquivo(StreamWriter write)
        {

            if (NaoCriptografar)
            {
                write.Write(_json.ToString());
            }
            else
            {
                write.Write(new EncriptaBase64(_json.ToString()).Processa());    
            }
            
        }

        public abstract void ManipulaJson(dynamic json);
    }
}
