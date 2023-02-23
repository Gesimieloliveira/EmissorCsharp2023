using FusionCore.Setup;

namespace FusionCore.MigracaoFluente
{
    public static class MigracaoFactory
    {
        public static IMigracao CriaMigrador(IConexaoCfg conexaoCfg, MigracaoTag tag)
        {
            return new MigracaoBancoDados(conexaoCfg, tag);
        }
    }
}