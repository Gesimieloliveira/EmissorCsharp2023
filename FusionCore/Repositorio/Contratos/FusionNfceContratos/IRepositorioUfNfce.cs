using FusionCore.FusionNfce.Uf;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioUfNfce : IRepositorio<UfNfce, byte>
    {
        void Salvar(UfNfce uf);
    }
}