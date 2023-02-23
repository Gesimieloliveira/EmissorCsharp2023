using System;
using System.Collections.Generic;
using FusionCore.CadastroUsuario;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Usuario
{
    public class GerenciarUsuariosContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public GerenciarUsuariosContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;

            ListaDeUsuarios = new List<GridUsuarioDTO>();
        }

        public IEnumerable<GridUsuarioDTO> ListaDeUsuarios
        {
            get => GetValue<IEnumerable<GridUsuarioDTO>>();
            private set => SetValue(value);
        }

        public GridUsuarioDTO UsuarioSelecionado
        {
            get => GetValue<GridUsuarioDTO>();
            set => SetValue(value);
        }

        public void CarregarDados()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioUsuario(sessao);
                ListaDeUsuarios = repositorio.BuscarTodosUsuarios();
            }
        }

        public UsuarioDTO CarregarUsuarioSelecionado()
        {
            if (UsuarioSelecionado == null)
            {
                throw new InvalidOperationException("Nenhum usuário selecionado para ser carregado!");
            }

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioUsuario(sessao);

                return repositorio.GetPeloId(UsuarioSelecionado.Id);
            }
        }
    }
}