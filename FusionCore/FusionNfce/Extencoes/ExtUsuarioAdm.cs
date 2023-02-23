using FusionCore.FusionNfce.Usuario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtUsuarioAdm
    {
        public static UsuarioDTO ToAdm(this UsuarioNfce usuarioNfce)
        {
            var usuario = new UsuarioDTO
            {
                Id = usuarioNfce.Id,
                Login = usuarioNfce.Login
            };

            return usuario;
        }
    }
}