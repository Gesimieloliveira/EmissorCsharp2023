using FusionCore.FusionAdm.Fiscal.Contratos;

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class LacreNfe : ILacre
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public IVolume Volume { get; set; }
    }
}