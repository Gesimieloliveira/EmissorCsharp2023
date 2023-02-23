using System;
using FusionCore.ConfiguracaoTransmissao.Nfce.Entidade;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.ConfiguracaoTransmissao.Nfce.Infra
{
    public class RepositorioConfiguracaoTransmissao : Repositorio<ConfiguracaoTransmissaoNfce, Guid>, IRepositorioConfiguracaoTransmissaoNfce
    {
        public RepositorioConfiguracaoTransmissao(ISession sessao) : base(sessao)
        {
        }

        
        public Transmissao ObterModoTransmissao()
        {
            var configuracao = GetPeloId(ConfiguracaoTransmissaoNfce.Identificador);

            return configuracao.Transmissao;
        }
    }
}