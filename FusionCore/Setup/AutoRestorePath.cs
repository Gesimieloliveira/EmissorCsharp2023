using System.IO;

namespace FusionCore.Setup
{
    public class AutoRestorePath
    {
        public AutoRestorePath()
        {
            CaminhoArquivosBak = Path.Combine("c:\\SistemaFusion", ".restore");
            CaminhoBancoDados = Path.Combine("C:\\SistemaFusion", "BancoDados");
        }

        public string CaminhoArquivosBak { get; }
        public string CaminhoBancoDados { get; }
    }
}
