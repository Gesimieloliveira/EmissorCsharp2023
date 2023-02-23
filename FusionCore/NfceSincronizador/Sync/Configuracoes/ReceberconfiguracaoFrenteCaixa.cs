using System.Collections.Generic;
using FusionCore.FusionNfce.Configuracoes.Extencoes;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Configuracoes
{
    public class ReceberconfiguracaoFrenteCaixa : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ConfiguracaoFrenteCaixa;
        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioServidor = new RepositorioConfiguracaoFrenteCaixa(sessaoServidor);
            var repositorioLocal = new RepositorioConfiguracaoFrenteCaixaNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var configuracaoFinanceiro = repositorioServidor.GetPeloId(byte.Parse(sp.Referencia));

                repositorioLocal.Salvar(configuracaoFinanceiro.ToNfce());

                SincronizacaoPendentesADeletar.Add(sp);
            });
        }
    }
}