using FusionCore.FusionNfce.VendasPendentesMensais;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioVendaPendenteMensal : IRepositorio<VendaPendenteMensal, int>
    {
        void Salvar(VendaPendenteMensal vendaPendenteMensal);
    }
}