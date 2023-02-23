using FusionCore.FusionPdv.Configuracoes;

namespace FusionCore.Repositorio.Contratos.FusionPdvContratos
{
    public interface IRepositorioConfiguracaoFrenteCaixaPdv : IRepositorio<ConfiguracaoFrenteCaixaPdv, byte>
    {
        void Salvar(ConfiguracaoFrenteCaixaPdv configuracaoFrenteCaixa);
        ConfiguracaoFrenteCaixaPdv BuscarUnico();
    }
}