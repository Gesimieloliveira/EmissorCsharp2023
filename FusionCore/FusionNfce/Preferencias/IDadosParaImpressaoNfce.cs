namespace FusionCore.FusionNfce.Preferencias
{
    public interface IDadosParaImpressaoNfce
    {
        int Id { get; }
        string XmlAutorizado { get; }
        bool Cancelada { get; }
        byte[] Logo { get; }
    }
}