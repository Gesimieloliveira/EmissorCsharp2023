using System.Collections.Generic;
using FusionCore.FusionNfce.Uf;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Municipios
{
    public class ReceberEstadoUf : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.EstadoUf;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioEstadoServidor = new RepositorioEstado(sessaoServidor);
            var repositorioUfNfce = new RepositorioUfNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var estadoServidor = repositorioEstadoServidor.GetPeloId(int.Parse(sp.Referencia));

                repositorioUfNfce.Salvar(new UfNfce
                {
                    CodigoIbge = estadoServidor.CodigoIbge,
                    Id = byte.Parse(estadoServidor.Id.ToString()),
                    Nome = estadoServidor.Nome,
                    Sigla = estadoServidor.Sigla
                });

                SincronizacaoPendentesADeletar.Add(sp);
                ExecutarFlush();
            });
        }
    }
}