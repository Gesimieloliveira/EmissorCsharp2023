using System.Collections.Generic;
using FusionCore.FusionNfce.Usuario;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Usuarios
{
    public class ReceberUsuario : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Usuario;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioUsuarioServidor = new RepositorioUsuario(sessaoServidor);
            var repositorioUsuarioNfce = new RepositorioUsuarioNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var usuario = repositorioUsuarioServidor.GetPeloId(int.Parse(sp.Referencia));

                repositorioUsuarioNfce.Salvar(new UsuarioNfce
                {
                    Id = usuario.Id,
                    Login = usuario.Login,
                    Senha = usuario.Senha,
                    Tema = usuario.Tema
                });

                SincronizacaoPendentesADeletar.Add(sp);

                ExecutarFlush();
            });
        }
    }
}