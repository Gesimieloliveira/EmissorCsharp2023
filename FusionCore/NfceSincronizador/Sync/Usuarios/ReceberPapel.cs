using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.Usuario.Papeis;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.NfceSincronizador.Sync.Usuarios
{
    public class ReceberPapel : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Papel;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioServidor = new RepositorioPapel(sessaoServidor);
            var repositorio = new RepositorioPapelNfce(sessaoNfce);

            foreach (var sincronizacaoPendente in pendentes)
            {
                var papelServidor = repositorioServidor.GetPeloId(new Guid(sincronizacaoPendente.Referencia));

                if (papelServidor == null)
                {
                    SincronizacaoPendentesADeletar.Add(sincronizacaoPendente);
                    return;
                }

                repositorio.DeletarPapel(papelServidor.Id);
                repositorio.PersistirPapel(PapelNfce.CriarApartir(papelServidor));

                SincronizacaoPendentesADeletar.Add(sincronizacaoPendente);
            }
        }
    }
}