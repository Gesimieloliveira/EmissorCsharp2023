using System;
using System.IO;
using System.Reflection;

namespace FusionCore.Helpers.Ambiente
{
    public class ManipulaArquivo : IDisposable
    {
        private string _origem;
        private string _destino;
        private Stream _abreArquivo;
        private StreamWriter _escreveNoArquivo;
        private readonly string _dadosParaArquivo;
        private StreamReader _lerArquivo;

        public ManipulaArquivo()
        {}

        public ManipulaArquivo(string arquivo)
        {
            _origem = arquivo;
        }

        public ManipulaArquivo(string arquivo, string dadosParaArquivo)
        {
            _origem = arquivo;
            _dadosParaArquivo = dadosParaArquivo;
        }

        public ManipulaArquivo Origem(string origem)
        {
            _origem = origem;
            return this;
        }

        public ManipulaArquivo Destino(string destino)
        {
            _destino = destino;
            return this;
        }

        public void Mover(bool deletarArquivoDestinoSeExistir = false)
        {
            if(deletarArquivoDestinoSeExistir)
                DeletarDestino();
            File.Move(_origem, _destino);
        }

        public void Copiar()
        {
            File.Copy(_origem, _destino);
        }

        public ManipulaArquivo VerificaSeExisteECria()
        {
            if (!File.Exists(_origem))
            {
                File.Create(_origem).Close();
            }

            return this;
        }

        public ManipulaArquivo AbreArquivoParaLeitura()
        {
            _abreArquivo = File.OpenRead(_origem);
            return this;
        }

        public ManipulaArquivo AbreArquivo(FileMode fileMode = FileMode.Open)
        {
            _abreArquivo = File.Open(_origem, fileMode);
            return this;
        }

        public ManipulaArquivo Escreve()
        {
            _escreveNoArquivo = new StreamWriter(_abreArquivo);
            _escreveNoArquivo.Write(_dadosParaArquivo);
            return this;
        }

        public ManipulaArquivo FecharArquivo()
        {
            _escreveNoArquivo?.Close();

            _lerArquivo?.Close();

            _abreArquivo?.Close();

            return this;
        }

        public string LerDadosComReplaceDeEspacosParaVazio()
        {
            _lerArquivo = new StreamReader(_abreArquivo);

            var linha = _lerArquivo.ReadLine();

            while (linha != null && !_lerArquivo.EndOfStream)
            {
                linha += _lerArquivo.ReadLine();
            }

            linha = linha?.Replace(" ", "");

            return linha;
        }

        public string LerDados()
        {
            _lerArquivo = new StreamReader(_abreArquivo);

            var linha = _lerArquivo.ReadLine();

            while (linha != null && !_lerArquivo.EndOfStream)
            {
                linha += _lerArquivo.ReadLine();
            }

            return linha;
        }

        public static string LocalAplicacao()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public bool Existe()
        {
            return File.Exists(_origem);
        }

        public bool NaoExiste()
        {
            return !Existe();
        }

        public void DeletaSeExistir()
        {
            if(Existe())
                File.Delete(_origem);
        }

        public void DeletarDestino()
        {
            if (File.Exists(_destino))
                File.Delete(_destino);
        }

        public void Dispose()
        {
            FecharArquivo();
        }

        public virtual bool IsArquivoEmUso()
        {
            FileStream stream = null;

            try
            {
                var file = new FileInfo(_origem);
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }

        public void DeletaSomenteArquivos(string diretorio)
        {
            new ManipulaPasta(diretorio).CriaPastaSeNaoExistir();

            var di = new DirectoryInfo(diretorio);

            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
        }

    }
}