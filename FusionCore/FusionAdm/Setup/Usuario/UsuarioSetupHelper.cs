using System;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Comandos.Adm.Usuario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Setup.Usuario
{
    public class UsuarioSetupHelper
    {
        private bool _inicializado;
        private RepositorioComun<UsuarioDTO> _repositorio;

        public void Inicializar()
        {
            _inicializado = true;
            _repositorio = new RepositorioComun<UsuarioDTO>();
        }

        public bool UsuarioAdminExiste()
        {
            ValidarInicializacao();

            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var usuario = new RepositorioUsuario(sessao).GetPeloId(1);

                    return usuario != null;
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Não foi possível verificar existência do usuário admin", e);
            }
        }

        public void ValidarInicializacao()
        {
            if (_inicializado)
            {
                return;
            }

            throw new InvalidOperationException("Setup Usuário não foi inicializado");
        }

        public void CriarUsuarioAdmin()
        {
            ValidarInicializacao();

            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    _repositorio.Sessao = sessao;
                    _repositorio.Executa(new CadastraAdmin());
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Erro ao criar usuário admin", e);
            }
        }
    }
}