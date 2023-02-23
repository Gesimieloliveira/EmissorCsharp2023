using System.Windows.Forms;

namespace FusionPdv.Acbr.Paf
{
    public class GerarMd5DeArquivoMd5 : IGerarMd5Finalizacao
    {
        public string ExecutaFinalizacao()
        {
            return new Md5().GerarMd5(Application.StartupPath + "\\MD5.txt");
        }
    }
}
