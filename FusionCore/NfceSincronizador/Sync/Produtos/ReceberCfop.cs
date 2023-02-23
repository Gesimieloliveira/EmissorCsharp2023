using System.Collections.Generic;
using FusionCore.FusionNfce.Cfop;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Produtos
{
    public class ReceberCfop : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Cfop;

        protected override void Sincroniza(
            IList<SincronizacaoPendente> pendentes,
            ISession sessaoServidor,
            ISession sessaoNfce)
        {
            var repositorioCfopServidor = new RepositorioCfop(sessaoServidor);
            var repositorioCfopNfce = new RepositorioCfopNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var cfopServidor = repositorioCfopServidor.GetPeloId(sp.Referencia);
                var cfopNfce = new CfopNfce(cfopServidor.Id, cfopServidor.Descricao, cfopServidor.ElegivelNfce);

                repositorioCfopNfce.Salvar(cfopNfce);

                SincronizacaoPendentesADeletar.Add(sp);

                sessaoServidor.Flush();
                sessaoServidor.Clear();
                sessaoNfce.Flush();
                sessaoNfce.Clear();
            });
        }
    }
}