using FusionCore.AutorizacaoOperacao;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.CadastroUsuario;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using System;

namespace FusionWPF.SharedViews.AutorizarOperacao
{
    public class AutorizarOperacaoViewModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly IAutorizarUsuario _autorizarUsuario;
        private readonly IPayload _payload;
        private readonly string _agregado;
        private IUsuario _usuarioLogado;

        public AutorizarOperacaoViewModel(ISessaoManager sessaoManager, IAutorizarUsuario autorizarUsuario, IUsuario usuarioLogado, Permissao permissao, IPayload payload, string agregado)
        {
            _autorizarUsuario = autorizarUsuario;
            _usuarioLogado = usuarioLogado;
            _payload = payload;
            _agregado = agregado;
            Permissao = permissao;
            UsuarioLogado = usuarioLogado.Login;
            _sessaoManager = sessaoManager;
        }

        public string Usuario
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Senha
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public Permissao Permissao { get; private set; }

        public string StatusPermissao { get; private set; }

        public string UsuarioLogado { get; private set; }

        public int UsuarioAutorizou { get; private set; }

        public bool VerificarPermissaoUsuarioLogado()
        {

            if (_usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao))
            {
                return true;
            }

            StatusPermissao = "Sem Permissão";
            return false;

        }

        public string Autorizar()
        {
            var resultadoAutorizacao = _autorizarUsuario.Autorizar(Usuario, Senha, Permissao);
            
            if (resultadoAutorizacao.Sucesso)
            {
                UsuarioAutorizou = resultadoAutorizacao.Usuario.Id;
                SalvarEventoAutorizacao();
                return null;
            }

            return resultadoAutorizacao.MensagemErro;
        }

        private void SalvarEventoAutorizacao()
        {
            var eventoCancelamento = new EventoOperacaoAutorizada(
                DateTime.Now,
                _usuarioLogado.Id,
               UsuarioAutorizou,
                Permissao,
                _payload,
                _agregado
            );

            using (var sessao = _sessaoManager.CriaSessao())
            {
                sessao.Persist(eventoCancelamento);
                sessao.Flush();
            }
        }
    }
}
