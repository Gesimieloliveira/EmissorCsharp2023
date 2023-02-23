using FusionCore.FusionNfce.Configuracoes;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioConfiguracaoFrenteCaixaNfce : IRepositorio<ConfiguracaoFrenteCaixaNfce, byte>
    {
        void Salvar(ConfiguracaoFrenteCaixaNfce configuracaoFrenteCaixa);

        ConfiguracaoFrenteCaixaNfce BuscarUnico();
    }
}