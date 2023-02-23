namespace FusionCore.FusionAdm.Fiscal.Contratos
{
    public interface ILacre
    {
        int Id { get; set; }
        string Numero { get; set; }
        IVolume Volume { get; set; }
    }
}