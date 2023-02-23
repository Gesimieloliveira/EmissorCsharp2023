using FusionCore.FusionNfce.Fiscal;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioNfceItem : IRepositorio<NfceItem, int>
    {
        void Salvar(NfceItem item);
    }
}