using System;
using System.Collections.Generic;
using FusionCore.CadastroUsuario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioUsuario : Repositorio<UsuarioDTO, int>
    {
        public RepositorioUsuario(ISession sessao) : base(sessao)
        {
        }

        public UsuarioDTO PeloLogin(string login)
        {
            var query = Sessao.QueryOver<UsuarioDTO>()
                .Where(u => u.Login == login)
                .Take(1);

            return query.SingleOrDefault();
        }

        public void Salvar(UsuarioDTO usuario)
        {
            if (usuario.Id == 0)
            {
                Sessao.Persist(usuario);
                Sessao.Flush();
                return;
            }

            usuario.AlteradoEm = DateTime.Now;

            Sessao.Update(usuario);
            Sessao.Flush();
        }

        public IEnumerable<GridUsuarioDTO> BuscarTodosUsuarios()
        {
            UsuarioDTO tbUsuario = null;
            GridUsuarioDTO aas = null;

            var qry = Sessao.QueryOver(() => tbUsuario)
                .SelectList(list => list
                    .Select(() => tbUsuario.Id).WithAlias(() => aas.Id)
                    .Select(() => tbUsuario.Login).WithAlias(() => aas.Login)
                );

            qry.TransformUsing(Transformers.AliasToBean<GridUsuarioDTO>());

            return qry.List<GridUsuarioDTO>();
        }
    }
}