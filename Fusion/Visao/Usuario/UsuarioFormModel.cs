using System;
using System.ComponentModel.DataAnnotations;
using Fusion.Sessao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Usuario
{
    public class UsuarioFormModel : ViewModel
    {
        public UsuarioFormModel(UsuarioDTO usuario)
        {
            Usuario = usuario;
        }

        public bool EdicaoIsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool LoginIsReadOnly
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsNovo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsAdmin
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool AlterarSenhaAtual
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                SetValue(string.Empty, nameof(Senha));
                SetValue(string.Empty, nameof(ConfirmarSenha));
            }
        }

        public UsuarioDTO Usuario
        {
            get => GetValue<UsuarioDTO>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso de um login")]
        public string Login
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso de uma senha")]
        public string Senha
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso que confirme a senha")]
        [Compare("Senha", ErrorMessage = @"Não confere com a senha")]
        public string ConfirmarSenha
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool RestricaoAgenda
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool ApenasFaturamento
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void SalvarModel()
        {
            ThrowExceptionSeExistirErros();

            Usuario.Login = Login;
            Usuario.ApenasFaturamento = ApenasFaturamento;

            if (AlterarSenhaAtual)
            {
                Usuario.Senha = SenhaHelper.CriptografarSenha(Senha);
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioUsuario(sessao);
                var existente = repositorio.PeloLogin(Login);

                if (existente != null && !existente.Equals(Usuario))
                {
                    throw new InvalidOperationException("Já existe um usuário com este login");
                }

                sessao.Evict(existente);
                repositorio.Salvar(Usuario);
            }
        }

        public void Inicializar()
        {
            IsNovo = Usuario.Id == 0;

            SetValue(Usuario.Login, nameof(Login));
            SetValue(Usuario.Senha, nameof(Senha));
            SetValue(Usuario.Senha, nameof(ConfirmarSenha));
            SetValue(Usuario.ApenasFaturamento, nameof(ApenasFaturamento));

            SetValue(IsNovo, nameof(AlterarSenhaAtual));
            SetValue(Usuario.IsAdmin, nameof(IsAdmin));
            SetValue(Usuario.IsAdmin, nameof(LoginIsReadOnly));
            SetValue(EdicaoEstaPermitida(), nameof(EdicaoIsEnabled));
        }

        private bool EdicaoEstaPermitida()
        {
            if (IsNovo || Usuario.IsAdmin == false)
            {
                return true;
            }

            return SessaoSistema.Instancia.UsuarioLogado.IsAdmin;
        }
    }
}