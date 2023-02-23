using System;
using FusionCore.FusionAdm.Financeiro.Extencoes;
using FusionCore.FusionPdv.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionPdv;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberConfiguracaoFinanceiro : SincronizacaoBase
    {
        public override string Tag => "enviar-configuracao-financeiro";
        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            NaoRegistrarEvento();

            var transacao = SessaoPdv.BeginTransaction();

            try
            {
                var configuracao = ObterConfiguracaoFinanceiro(ultimaSincronizacao);

                if (configuracao == null) return;

                var repositorio = new RepositorioConfiguracaoFinanceiroPdv(SessaoPdv);

                repositorio.Salvar(configuracao);

                SessaoPdv.Flush();

                transacao.Commit();
            }
            catch (Exception)
            {
                transacao.Rollback();
                throw;
            }

        }

        private ConfiguracaoFinanceiroPdv ObterConfiguracaoFinanceiro(DateTime ultimaSincronizacao)
        {
            var repositorio = new RepositorioConfiguracaoFinanceiro(SessaoAdm);

            var configuracao = repositorio.BuscarParaSincronizar(ultimaSincronizacao);

            return configuracao.ToPdv();
        }
    }
}