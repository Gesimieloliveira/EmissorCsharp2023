using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace UnitTest.AppConfig
{
    [TestClass]
    public class TesteArquivoAppConfig
    {
        private const string NomeProjetoTestes = "UnitTest";
        private string _diretorioBaseProjeto;

        [TestInitialize]
        public void Initialize()
        {
            var dirParts = Environment.CurrentDirectory.Split(Path.DirectorySeparatorChar);

            var indexBaseName = -1;
            for (var i = 0; i < dirParts.Length; i++)
            {
                if (dirParts[i] != NomeProjetoTestes) continue;
                indexBaseName = i;
                break;
            }

            if (indexBaseName == -1)
            {
                throw new InternalTestFailureException("Falha identificar diretório base projeto");
            }

            _diretorioBaseProjeto = string.Join(Path.DirectorySeparatorChar.ToString(), dirParts.Take(indexBaseName));
        }

        [TestMethod]
        public void ArquivosAppConfigNaoDevePossuirAssemblyBinding()
        {
            var todosArquivosConfigs = Directory.GetFiles(
                _diretorioBaseProjeto, 
                "APP.CONFIG", 
                SearchOption.AllDirectories
            );

            var existeArquivoConfigComAssemlbyBinding = false;
            foreach (var arquivo in todosArquivosConfigs)
            {
                var content = File.ReadAllText(arquivo).ToLower();
                if (!content.Contains("<assemblybinding")) continue;

                existeArquivoConfigComAssemlbyBinding = true;
                break;
            }

            Assert.IsTrue(todosArquivosConfigs.Length > 0, "Nenhum arquivo app.config encontrado");
            Assert.IsFalse(existeArquivoConfigComAssemlbyBinding, "Existe arquivo app.config com assemblyBinding");
        }
    }
}
