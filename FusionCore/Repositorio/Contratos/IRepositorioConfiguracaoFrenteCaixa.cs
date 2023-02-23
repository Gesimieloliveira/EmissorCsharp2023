using System;
using FusionCore.FusionAdm.Configuracoes;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioConfiguracaoFrenteCaixa : IRepositorio<ConfiguracaoFrenteCaixa, byte>
    {
        void Salvar(ConfiguracaoFrenteCaixa configuracaoFrenteCaixa);
        ConfiguracaoFrenteCaixa BuscarUnica();

        ConfiguracaoFrenteCaixa BuscarUnicaParaSincronizacao(DateTime ultimaSincronizacao);
    }
}