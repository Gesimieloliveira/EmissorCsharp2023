using System.Collections.Generic;
using FusionCore.Configuracoes;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Configuracoes
{
    public class ReceberConfiguracaoEstoque : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ConfiguracaoEstoque;

        protected override void Sincroniza(
            IList<SincronizacaoPendente> pendentes,
            ISession sessaoServidor,
            ISession sessaoNfce)
        {
            if (!pendentes.Any())
            {
                return;
            }

            var cfg = GetConfiguracaoAlterada(sessaoServidor);

            cfg.EntidadeSincronizavel = EntidadeSincronizavel.NaoSincronizar;

            var repositorio = new RepositorioConfiguracaoEstoque(sessaoNfce);
            repositorio.SaveOrUpdate(cfg);

            pendentes.ForEach(SincronizacaoPendentesADeletar.Add);
        }

        private ConfiguracaoEstoque GetConfiguracaoAlterada(ISession sessao)
        {
            var repositorio = new RepositorioConfiguracaoEstoque(sessao);
            return repositorio.GetConfiguracaoUnica();
        }
    }
}