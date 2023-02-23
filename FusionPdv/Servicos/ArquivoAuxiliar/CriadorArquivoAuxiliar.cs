using System.IO;
using System.Windows.Forms;
using FusionLibrary.Helper.Criptografia;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class CriadorArquivoAuxiliar
    {
        public static bool ArquivoExiste => File.Exists(Application.StartupPath + "\\ArquivoAuxiliar.agil4");
        private const string TemplateArquivo =
            "{ \"Agil4\": { \"Impressora\": { \"Serie\": \"\", \"Gt\": \"0\", \"Numero\": \"\"}, \"Md5\": \"\" } }";

        public void CriaArquivo()
        {
            if (ArquivoExiste) return;

            File.Create(Application.StartupPath + "\\ArquivoAuxiliar.agil4").Close();
            EscreveTemplateNoArquivo();
        }

        private static void EscreveTemplateNoArquivo()
        {
            using (var stream = File.Open(Application.StartupPath + "\\ArquivoAuxiliar.agil4", FileMode.Create))
            {
                using (var write = new StreamWriter(stream))
                {
                    write.Write(new EncriptaBase64(TemplateArquivo).Processa());
                }
            }
        }
    }
}