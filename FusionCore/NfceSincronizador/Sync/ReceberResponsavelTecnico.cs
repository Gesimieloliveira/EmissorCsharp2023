using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.Csrt;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.NfceSincronizador.Sync
{
    public class ReceberResponsavelTecnico : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ResponsavelTecnico;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioResponsavelTecnicoServidor = new RepositorioResponsavelTecnico(sessaoServidor);
            var repositorioResponsavelTecnicoLocal = new RepositorioResponsavelTecnicoNfce(sessaoNfce);


            if (pendentes.Count > 0)
                repositorioResponsavelTecnicoLocal.DeletarTodos();


            foreach (var sincronizacaoPendente in pendentes)
            {
                var responsavelTecnicoServidor =
                    repositorioResponsavelTecnicoServidor.GetPeloId(new Guid(sincronizacaoPendente.Referencia));

                if (responsavelTecnicoServidor == null)
                {
                    SincronizacaoPendentesADeletar.Add(sincronizacaoPendente);
                    continue;
                }

                if (responsavelTecnicoServidor.IsNFCe == false)
                {
                    SincronizacaoPendentesADeletar.Add(sincronizacaoPendente);
                    continue;
                }


                var responsavelTecnicoLocal = ResponsavelTecnicoNfce.Instancia(responsavelTecnicoServidor);
                repositorioResponsavelTecnicoLocal.SalvarOuAtualizar(responsavelTecnicoLocal);

                SincronizacaoPendentesADeletar.Add(sincronizacaoPendente);
            }

        }
    }
}