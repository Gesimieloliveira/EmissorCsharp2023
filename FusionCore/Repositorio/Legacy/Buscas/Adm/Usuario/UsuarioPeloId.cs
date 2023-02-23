using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Usuario
{
    public class UsuarioPeloId : IBuscaUnico<UsuarioDTO>
    {
        private readonly int _usuarioId;

        public UsuarioPeloId(int usuarioId)
        {
            _usuarioId = usuarioId;
        }

        public UsuarioDTO Busca(ISession sessao)
        {
            return sessao.Get<UsuarioDTO>(_usuarioId);
        }
    }
}