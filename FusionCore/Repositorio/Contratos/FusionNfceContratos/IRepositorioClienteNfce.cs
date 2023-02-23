using FusionCore.FusionNfce.Cliente;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioClienteNfce : IRepositorio<ClienteNfce, int>
    {
        void Salvar(ClienteNfce cliente);
    }
}