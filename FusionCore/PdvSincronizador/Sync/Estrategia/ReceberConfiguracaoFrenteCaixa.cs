using System;
using FusionCore.FusionPdv.Configuracoes.Extencoes;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionPdv;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberConfiguracaoFrenteCaixa : SincronizacaoBase
    {
        public override string Tag => @"configuracao-frente-caixa";
        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var repositorioServidor = new RepositorioConfiguracaoFrenteCaixa(SessaoAdm);
            var repositorioLocal = new RepositorioConfiguracaoFrenteCaixaPdv(SessaoPdv);

            var configuracaoFrenteCaixa = repositorioServidor.BuscarUnicaParaSincronizacao(ultimaSincronizacao);
            

            var transacaoPdv = SessaoPdv.BeginTransaction();

            using (SessaoPdv)
            using (transacaoPdv)
            {
                if (configuracaoFrenteCaixa == null) return;

                repositorioLocal.Salvar(configuracaoFrenteCaixa.ToPdv());

                transacaoPdv.Commit();
            }
        }
    }
}