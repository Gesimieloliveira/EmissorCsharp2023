namespace FusionCore.MigracaoFluente
{
    public interface IMigracao
    {
        bool PrecisaAtualizar { get; }
        long UltimaVersaoInterna { get; }
        void Migracao();
    }
}