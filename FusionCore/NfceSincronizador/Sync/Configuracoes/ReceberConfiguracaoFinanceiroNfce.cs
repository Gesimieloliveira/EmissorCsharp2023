using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro.Extencoes;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Configuracoes
{
    public class ReceberConfiguracaoFinanceiroNfce : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ConfiguracaoFinanceiro;
        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioServidor = new RepositorioConfiguracaoFinanceiro(sessaoServidor);
            var repositorioLocal = new RepositorioConfiguracaoFinanceiroNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var configuracaoFinanceiro = repositorioServidor.GetPeloId(byte.Parse(sp.Referencia));

                repositorioLocal.Salvar(configuracaoFinanceiro.ToNfce());

                SincronizacaoPendentesADeletar.Add(sp);
            });
        }
    }
}