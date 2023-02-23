using System.Collections.Generic;
using FusionCore.FusionNfce.Tef;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.TefsPos
{
    public class ReceberPos : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Pos;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repoistorioLocal = new RepositorioPosNfce(sessaoNfce);
            var repositorioServidor = new RepositorioPos(sessaoServidor);

            pendentes.ForEach(p =>
            {
                var posServidor = repositorioServidor.GetPeloId(short.Parse(p.Referencia));

                if (posServidor == null)
                {
                    SincronizacaoPendentesADeletar.Add(p);
                    return;
                }

                var posLocal = new PosNfce(posServidor);

                repoistorioLocal.Salvar(posLocal);

                SincronizacaoPendentesADeletar.Add(p);

                ExecutarFlush();
            });
        }
    }
}