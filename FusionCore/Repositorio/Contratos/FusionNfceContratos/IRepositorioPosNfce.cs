using FusionCore.FusionNfce.Tef;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioPosNfce : IRepositorio<PosNfce, short>
    {
        void Salvar(PosNfce pos);
    }
}