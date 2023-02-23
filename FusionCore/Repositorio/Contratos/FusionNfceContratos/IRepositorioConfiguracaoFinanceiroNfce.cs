using FusionCore.FusionNfce.Financeiro;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioConfiguracaoFinanceiroNfce : IRepositorio<ConfiguracaoFinanceiroNfce, byte>
    {
        void Salvar(ConfiguracaoFinanceiroNfce configuracaoFinanceiro);

        ConfiguracaoFinanceiroNfce BuscarUnico();
    }
}