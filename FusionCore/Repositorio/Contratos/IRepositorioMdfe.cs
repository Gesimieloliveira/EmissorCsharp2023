using FusionCore.FusionAdm.MdfeEletronico;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioMdfe : IRepositorio<MDFeEletronico, int>
    {
        void Salvar(MDFeEletronico eletronico);
    }
}