using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioConfiguracaoEmail : IRepositorio<ConfiguracaoEmailDTO, int>
    {
        ConfiguracaoEmailDTO BuscarUnicaConfiguracai();
    }
}