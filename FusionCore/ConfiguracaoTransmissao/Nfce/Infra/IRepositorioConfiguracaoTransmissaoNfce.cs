using System;
using FusionCore.ConfiguracaoTransmissao.Nfce.Entidade;
using FusionCore.Repositorio.Contratos;

namespace FusionCore.ConfiguracaoTransmissao.Nfce.Infra
{
    public interface IRepositorioConfiguracaoTransmissaoNfce : IRepositorio<ConfiguracaoTransmissaoNfce, Guid>
    {
        Transmissao ObterModoTransmissao();
    }
}