using System;
using System.Windows.Forms;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class FazerBackupArquivoAuxiliar
    {

        private string _origemArquivo = "";
        private string _destinoArquivo = "";
        private readonly string _numeroBackup;

        public FazerBackupArquivoAuxiliar(string numeroBackup)
        {
            _numeroBackup = numeroBackup;
        }

        public void EfetuarBackup()
        {
            try
            {
                PegaDiretorioDosArquivos("ArquivoAuxiliar.agil4", "ArquivoAuxiliar.agil4.bak_" + _numeroBackup);

                System.IO.File.Copy(_origemArquivo, _destinoArquivo, true);

                if (
                    System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, "ArquivoAuxiliar.agil4.bak_1")))
                    return;
                PegaDiretorioDosArquivos("ArquivoAuxiliar.agil4", "ArquivoAuxiliar.agil4.bak_1");

                System.IO.File.Copy(_origemArquivo, _destinoArquivo, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao fazer backup do ArquivoAuxiliar\nPorfavor chamar o suporte.", ex);
            }
            
        }

        private void PegaDiretorioDosArquivos(string origem, string destino)
        {
            try
            {
                _origemArquivo = System.IO.Path.Combine(Application.StartupPath, origem);
                _destinoArquivo = System.IO.Path.Combine(Application.StartupPath, destino);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao fazer backup do ArquivoAuxiliar\nPorfavor chamar o suporte.", ex);
            }
            
        }

        public void RestaurarArquivoAuxiliar()
        {
            try
            {
                PegaDiretorioDosArquivos("ArquivoAuxiliar.agil4.bak_" + _numeroBackup, "ArquivoAuxiliar.agil4");
                System.IO.File.Copy(_origemArquivo, _destinoArquivo, true);
                TestaArquivoAuxiliar();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao fazer backup do ArquivoAuxiliar\nPorfavor chamar o suporte.", ex);
            }
        }

        public void TestaArquivoAuxiliar()
        {
            new BuscarGt().Executar();
        }

    }
}
