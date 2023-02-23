using System.Collections.Generic;
using FusionCore.FusionNfce.ConfiguracaoEmail;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Configuracoes
{
    public class ReceberConfiguracaoEmail : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.ConfiguracaoEmail;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioServidor = new RepositorioConfiguracaoEmail(sessaoServidor);
            
            var repositorioNfce = new RepositorioConfiguracaoEmailNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var configuracaoEmailServidor = repositorioServidor.GetPeloId(int.Parse(sp.Referencia));

                repositorioNfce.Salvar(new ConfiguracaoEmailNfce
                {
                    Id = 1,
                    Senha = configuracaoEmailServidor.Senha,
                    Email = configuracaoEmailServidor.Email,
                    UrlServidorEmail = configuracaoEmailServidor.UrlServidorEmail,
                    Porta = configuracaoEmailServidor.Porta,
                    Ssl = configuracaoEmailServidor.Ssl,
                    ProtocoloSeguranca = configuracaoEmailServidor.ProtocoloSeguranca,
                    UsarFusionZohoo = configuracaoEmailServidor.UsarFusionZohoo,
                    EmailResposta = configuracaoEmailServidor.EmailResposta
                });

                SincronizacaoPendentesADeletar.Add(sp);
            });
        }
    }
}