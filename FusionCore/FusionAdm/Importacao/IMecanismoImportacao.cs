using NHibernate;

namespace FusionCore.FusionAdm.Importacao
{
    public interface IMecanismoImportacao
    {
        void AnexarSesao(ISession sessao);
        void InformarArquivo(string caminhoArquivo);
        void Importar();
    }
}