using FusionCore.FusionAdm.Configuracoes;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioBalanca : IRepositorio<Balanca, byte>
    {
        void Salvar(Balanca balanca);

        Balanca BuscarUnicaBalanca();
    }
}