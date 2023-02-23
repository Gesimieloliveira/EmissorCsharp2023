using System;
using ACBrFramework.PAF;
using FusionCore.Helpers.Ambiente;

namespace FusionPdv.Acbr.Paf
{
    public class GerarMd5
    {
        private readonly ACBrPAF _acbrPaf;
        public string Md5Final { get; private set; }
        private readonly IGerarMd5Finalizacao _gerarMd5Finalizacao;

        public GerarMd5()
        {
            _acbrPaf = AcbrFactory.ObterAcbrPaf();
            _gerarMd5Finalizacao = new GerarMd5DeArquivoMd5();
        }

        public void Executar()
        {
            new ConfigurarAcbrPaf().Executa();

            var caminho = ManipulaPasta.LocalSistema() + @"\MD5.txt";

            try
            {
                _acbrPaf.SaveFileTXT_N(caminho);
            }
            catch (Exception)
            {
                caminho = @"\MD5.txt";
                _acbrPaf.SaveFileTXT_N(caminho);
            }

            NotificaFinalizacao();
        }

        private void NotificaFinalizacao()
        {
            Md5Final = _gerarMd5Finalizacao.ExecutaFinalizacao();
        }
    }
}
