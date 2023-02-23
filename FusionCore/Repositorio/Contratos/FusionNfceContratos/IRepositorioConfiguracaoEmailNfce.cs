using FusionCore.FusionNfce.ConfiguracaoEmail;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioConfiguracaoEmailNfce : IRepositorio<ConfiguracaoEmailNfce, int>
    {
        void Salvar(ConfiguracaoEmailNfce configuracaoEmail);

        ConfiguracaoEmailNfce BuscarUnicaConfiguracao();
    }
}