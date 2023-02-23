using FusionCore.FusionAdm.CteEletronicoOs.Emissao;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioCteOs : IRepositorio<CteOs, int>
    {
        void Salvar(CteOs cteOs);
    }
}