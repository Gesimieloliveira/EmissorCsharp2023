using FusionCore.FusionAdm.Importacao.Exception;
using FusionCore.FusionAdm.Sessao;

namespace FusionCore.FusionAdm.Importacao
{
    public class Importador
    {
        public void Importar(string caminhoArquivo, IMecanismoImportacao mecanismoImportacao)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var transacao = sessao.BeginTransaction();

                try
                {
                    mecanismoImportacao.AnexarSesao(sessao);
                    mecanismoImportacao.InformarArquivo(caminhoArquivo);
                    mecanismoImportacao.Importar();
                    transacao.Commit();
                }
                catch (System.Exception e)
                {
                    transacao.Rollback();
                    throw new ImportadorException(e.Message, e);
                }
            }
        }
    }
}