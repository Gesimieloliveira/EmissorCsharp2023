using System.Collections.Generic;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Produtos
{
    public class ReceberIbpt : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Ibpt;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioServidor = new RepositorioIbpt(sessaoServidor);
            var repositorioNfce = new RepositorioIbptNfce(sessaoNfce);
            var horaDeFlush = 0;

            pendentes.ForEach(sp =>
            {
                var todosIbptPeloNcm = repositorioServidor.GetTodosPeloNcm(sp.Referencia);

                todosIbptPeloNcm.ForEach(ibpt =>
                {
                    repositorioNfce.Salvar(ExtIbptNfce.ToNfce(ibpt));
                });


                SincronizacaoPendentesADeletar.Add(sp);
                horaDeFlush++;

                if (horaDeFlush % 4 != 0) return;

                sessaoServidor.Flush();
                sessaoServidor.Clear();
                sessaoNfce.Flush();
                sessaoNfce.Clear();
            });
        }
    }
}