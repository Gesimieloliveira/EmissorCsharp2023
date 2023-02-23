using FusionCore.FusionNfce.ConfiguracaoSat;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioConfiguracaoSatFiscalNfce
    {
        void Salvar(ConfiguracaoSatFiscal configuracaoSatFiscal);

        ConfiguracaoSatFiscal BuscarConfiguracao();
    }
}