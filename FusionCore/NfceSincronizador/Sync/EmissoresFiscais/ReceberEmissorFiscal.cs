using System.Collections.Generic;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.EmissoresFiscais
{
    public class ReceberEmissorFiscal : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.EmissorFiscal;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes,
            ISession sessaoServidor,
            ISession sessaoNfce)
        {
            var repositorioEmissorFiscalServidor = new RepositorioEmissorFiscal(sessaoServidor);
            var repositorioEmissorFiscalNfce = new RepositorioEmissorFiscalNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var emissorFiscalServidor = repositorioEmissorFiscalServidor.GetPeloId(byte.Parse(sp.Referencia));

                if (emissorFiscalServidor == null)
                {
                    SincronizacaoPendentesADeletar.Add(sp);
                    return;
                }

                if (emissorFiscalServidor.FlagNfe || emissorFiscalServidor.FlagCte 
                    || emissorFiscalServidor.FlagMdfe || emissorFiscalServidor.FlagCteOs
                    || emissorFiscalServidor.IsFaturamento)
                {
                    SincronizacaoPendentesADeletar.Add(sp);
                    return;
                }

                var nfceEmissorFiscal = emissorFiscalServidor.ToNfce();

                repositorioEmissorFiscalNfce.SalvarENaoSincronizar(nfceEmissorFiscal);

                sessaoServidor.Flush();
                sessaoServidor.Clear();
                sessaoNfce.Flush();
                sessaoNfce.Clear();

                SincronizacaoPendentesADeletar.Add(sp);
            });
        }


    }
}