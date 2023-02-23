using FusionCore.FusionPdv.Financeiro;

namespace FusionCore.Repositorio.Contratos.FusionPdvContratos
{
    public interface IRepositorioConfiguracaoFinanceiroPdv : IRepositorio<ConfiguracaoFinanceiroPdv, byte>
    {
        void Salvar(ConfiguracaoFinanceiroPdv configuracaoFinanceiro);

        ConfiguracaoFinanceiroPdv BuscarUnico();
    }
}