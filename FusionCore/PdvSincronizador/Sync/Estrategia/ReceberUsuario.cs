using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Usuario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate.Util;
using UsuarioPdvRepositorio = FusionCore.Repositorio.Legacy.Ativos.Pdv.UsuarioRepositorio;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberUsuario : SincronizacaoBase
    {
        public override string Tag { get; } = @"receber-usuario";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var usuarios = ObterUsuariosAlterados(ultimaSincronizacao);
            if (usuarios.Count == 0)
                return;

            var usuariosPdv = ConverterParaPdv(usuarios);
            Persistir(usuariosPdv);
            RegistraEvento = true;
        }

        private IList<UsuarioDTO> ObterUsuariosAlterados(DateTime ultimaSincronizacao)
        {
            var repositorio = new RepositorioComun<UsuarioDTO>(SessaoAdm);
            var usuarios = repositorio.Busca(new UsuariosParaSincronizacao(ultimaSincronizacao));
            return usuarios;
        }

        private static IList<UsuarioPdvDt> ConverterParaPdv(IEnumerable<UsuarioDTO> usuarios)
        {
            var usuariosPdv = new List<UsuarioPdvDt>();

            usuarios.ForEach(usuario =>
            {
                usuariosPdv.Add(new UsuarioPdvDt
                {
                    CadastradoEm = usuario.CadastradoEm,
                    Id = usuario.Id,
                    Login = usuario.Login,
                    Senha = usuario.Senha
                });
            });

            return usuariosPdv;
        }

        private void Persistir(IList<UsuarioPdvDt> usuarios)
        {
            var transacao = SessaoPdv.BeginTransaction();

            try
            {
                var repositorioPdv = new UsuarioPdvRepositorio(SessaoPdv);
                repositorioPdv.SalvarLista(usuarios);
                transacao.Commit();
            }
            catch (Exception)
            {
                transacao?.Rollback();
                throw;
            }
        }
    }
}