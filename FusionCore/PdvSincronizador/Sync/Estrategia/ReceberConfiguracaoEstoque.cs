using System;
using FusionCore.Configuracoes;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberConfiguracaoEstoque : SincronizacaoBase
    {
        public override string Tag { get; } = @"receber-cfg-estoque";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var config = ObterConfiguracoesAlteradas(ultimaSincronizacao);

            if (config == null)
            {
                return;
            }

            config.EntidadeSincronizavel = EntidadeSincronizavel.NaoSincronizar;

            var repositorio = new RepositorioConfiguracaoEstoque(SessaoPdv);
            repositorio.SaveOrUpdate(config);

            RegistraEvento = true;
        }

        private ConfiguracaoEstoque ObterConfiguracoesAlteradas(DateTime ultimaSincronizacao)
        {
            var repositorio = new RepositorioConfiguracaoEstoque(SessaoAdm);
            var config = repositorio.GetConfiguracaoUnica();

            return config.AlteradoEm >= ultimaSincronizacao ? config : null;
        }
    }
}