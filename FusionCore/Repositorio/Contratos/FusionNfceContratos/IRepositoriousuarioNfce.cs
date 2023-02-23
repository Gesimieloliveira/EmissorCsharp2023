using FusionCore.FusionNfce.Usuario;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioUsuarioNfce : IRepositorio<UsuarioNfce, int>
    {
        void Salvar(UsuarioNfce usuarioNfce);

        UsuarioNfce FazerLogin(string login, string senha);

        UsuarioNfce BuscaUsuarioPeloLogin(string text);
    }
}