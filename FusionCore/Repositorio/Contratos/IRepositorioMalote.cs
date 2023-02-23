using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioMalote : IRepositorio<Malote, int>
    {
        void Salvar(Malote malote);
    }
}