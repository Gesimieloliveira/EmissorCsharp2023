using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Usuario
{
    public class VincularUsuarioAoPapelFormModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly Papel _papel;
        private ObservableCollection<UsuarioDTO> _usuarioLista;
        private UsuarioDTO _usuarioSelecionado;

        public VincularUsuarioAoPapelFormModel(Papel papel)
        {
            _papel = papel;
            _usuarioLista = new ObservableCollection<UsuarioDTO>();
            _sessaoManager = new SessaoManagerAdm();

            Titulo = $"Vincular usuário no papel {_papel.Descricao}";

            Inicializar();
        }

        public string Titulo
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public ObservableCollection<UsuarioDTO> UsuarioLista
        {
            get => _usuarioLista;
            set
            {
                _usuarioLista = value;
                PropriedadeAlterada();
            }
        }

        public UsuarioDTO UsuarioSelecionado
        {
            get => _usuarioSelecionado;
            set
            {
                _usuarioSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<UsuarioDTO> UsuarioFoiAdicionado;

        private void Inicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioUsuario = new RepositorioUsuario(sessao);
                UsuarioLista = new ObservableCollection<UsuarioDTO>(repositorioUsuario.BuscaTodos());
            }
        }

        public void AdicionarUsuarioAoPapel()
        {
            if (UsuarioSelecionado == null)
            {
                DialogBox.MostraInformacao("Selecione um usuário para vincular");
                return;
            }


            if (_papel.Usuarios.Any(u => u == UsuarioSelecionado))
            {
                DialogBox.MostraInformacao("Este usuário já está vinculado neste papel");
                return;
            }

            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioPapel(sessao);

                repositorio.VincularUsuario(_papel, UsuarioSelecionado);
                repositorio.Commit();
            }


            UsuarioFoiAdicionado?.Invoke(this, UsuarioSelecionado);
        }
    }
}