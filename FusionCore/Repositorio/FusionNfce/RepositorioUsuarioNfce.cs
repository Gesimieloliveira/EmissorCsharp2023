using FusionCore.FusionNfce.Usuario;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioUsuarioNfce : Repositorio<UsuarioNfce, int>, IRepositorioUsuarioNfce
    {
        public RepositorioUsuarioNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(UsuarioNfce usuarioNfce)
        {
            Sessao.SaveOrUpdate(usuarioNfce);
        }

        public UsuarioNfce FazerLogin(string login, string senha)
        {
            var queryOver = Sessao.QueryOver<UsuarioNfce>().Where(u => u.Login == login && u.Senha == senha);

            var usuario = queryOver.SingleOrDefault<UsuarioNfce>();

            return usuario;
        }

        public UsuarioNfce BuscaUsuarioPeloLogin(string text)
        {
            var queryOver = Sessao.QueryOver<UsuarioNfce>().Where(u => u.Login == text);

            var usuario = queryOver.SingleOrDefault<UsuarioNfce>();

            return usuario;
        }
    }
}