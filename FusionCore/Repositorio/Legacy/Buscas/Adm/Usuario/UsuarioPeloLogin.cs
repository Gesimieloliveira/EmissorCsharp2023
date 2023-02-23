using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Usuario
{
    public class UsuarioPeloLogin : IBuscaUnico<UsuarioDTO>
    {
        private readonly string _login;

        public UsuarioPeloLogin(string login)
        {
            _login = login;
        }

        public UsuarioDTO Busca(ISession sessao)
        {
            var query = sessao.Query<UsuarioDTO>().Where(u => u.Login == _login);
            return (UsuarioDTO) query.FirstOrNull();
        }
    }
}