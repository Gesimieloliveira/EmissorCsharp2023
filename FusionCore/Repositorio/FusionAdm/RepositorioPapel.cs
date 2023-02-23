using System;
using System.Collections.Generic;
using FusionCore.Papeis;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPapel : Repositorio<Papel, Guid>
    {
        public RepositorioPapel(ISession sessao) : base(sessao)
        {
        }

        public void SalvarAlteracoes(Papel papel)
        {
            if (papel.Id == Guid.Empty)
            {
                papel.Id = Guid.NewGuid();

                Sessao.Persist(papel);
                Sessao.Flush();

                return;
            }

            Sessao.Update(papel);
            Sessao.Flush();
        }

        public void VincularUsuario(Papel papel, UsuarioDTO usuario)
        {
            papel.AddUsuario(usuario);

            SalvarAlteracoes(papel);
        }

        public void RemoverUsuario(Papel papel, UsuarioDTO usuario)
        {
            papel.RemoverUsuario(usuario);

            SalvarAlteracoes(papel);
        }

        public void AdicionarVariasPermissoes(Papel papel, IEnumerable<Permissao> permissoes)
        {
            foreach (var permissao in permissoes)
            {
                if (papel.Existe(permissao))
                {
                    continue;
                }

                var nova = new PapelPermissao(papel, permissao);

                papel.AddPermissao(nova);
            }

            SalvarAlteracoes(papel);
        }

        public void AdicionarPermissao(Papel papel, Permissao permissao)
        {
            var nova = new PapelPermissao(papel, permissao);

            papel.AddPermissao(nova);

            SalvarAlteracoes(papel);
        }

        public void RemoverPermissao(Papel papel, Permissao permissao)
        {
            papel.RemoverPermissao(permissao);

            SalvarAlteracoes(papel);
        }

        public void RemoverTodasAsPermissoes(Papel papel)
        {
            papel.RemoverTodasAsPermissoes();

            SalvarAlteracoes(papel);
        }
    }
}