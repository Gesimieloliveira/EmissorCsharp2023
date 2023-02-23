namespace FusionCore.FusionNfce.Preferencias
{
    public interface IDadosParaEnvioEmailNfce : IDadosParaImpressaoNfce
    {
        string NumeroChave { get; }
    }
}