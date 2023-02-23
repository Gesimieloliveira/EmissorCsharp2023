using System.Collections.Generic;
using FusionCore.FusionNfce.Fiscal.Regras;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Tributacoes.Regras;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Produtos
{
    public class ReceberRegraTributacaoSaida : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.RegraTributacaoSaida;

        protected override void Sincroniza(
            IList<SincronizacaoPendente> pendentes, 
            ISession sessaoServidor, 
            ISession sessaoNfce
        ) {
            var repositorio = new RepositorioRegraTributacao(sessaoServidor);
            pendentes.ForEach(pendente => Sincroniza(pendente, repositorio, sessaoNfce));
        }

        private void Sincroniza(SincronizacaoPendente pendente, RepositorioRegraTributacao repositorio, ISession sessNfce)
        {
            var regra = repositorio.GetPeloId(short.Parse(pendente.Referencia));

            if (regra == null)
            {
                SincronizacaoPendentesADeletar.Add(pendente);
                return;
            }

            var regraNfce = NfceRegraTributacaoSaida.From(regra);

            sessNfce.SaveOrUpdate(regraNfce);
            sessNfce.Flush();

            SincronizacaoPendentesADeletar.Add(pendente);
        }
    }
}