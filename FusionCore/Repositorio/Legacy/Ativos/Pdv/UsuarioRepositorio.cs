using System;
using System.Linq;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class UsuarioRepositorio : RepositorioBase<UsuarioPdvDt>
    {
        public UsuarioRepositorio(ISession sessao)
            : base(sessao)
        {
        }

        public UsuarioPdvDt FazerLogin(string login, string senha)
        {
            try
            {
                var usuario = (UsuarioPdvDt)
                    Sessao.Query<UsuarioPdvDt>()
                        .Where(u => u.Login.Equals(login) && u.Senha.Equals(senha))
                        .FirstOrNull();

                return usuario;
            }
            catch (Exception ex)
            {
                throw new RepositorioExeption("Falha ou tentar fazer login.", ex);
            }
        }
    }
}