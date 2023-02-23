using FusionCore.Core.Net;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoEmailModel : AutoSaveModel
    {
        private ConfiguracaoEmailDTO _configuracao;
        private bool _userServidorProprio;
        private string _emailResposta;

        public string Smtp
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Senha
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Porta
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public ProtocoloSeguranca Protocolo
        {
            get => GetValue<ProtocoloSeguranca>();
            set => SetValue(value);
        }

        protected override void OnInicializa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(sessao);
                _configuracao = repositorio.Busca(new UnicaConfiguracaoEmail());
            }

            RefreshControls();
        }

        private void RefreshControls()
        {
            if (_configuracao == null)
            {
                return;
            }

            Smtp = _configuracao.UrlServidorEmail;
            Email = _configuracao.Email;
            Senha = SimetricaCrip.Descomputar(_configuracao.Senha);
            Porta = _configuracao.Porta;
            Protocolo = _configuracao.ProtocoloSeguranca;
            UserServidorProprio = _configuracao.UsarFusionZohoo;
            EmailResposta = _configuracao.EmailResposta;
        }

        public bool UserServidorProprio
        {
            get => _userServidorProprio;
            set
            {
                if (value == _userServidorProprio) return;
                _userServidorProprio = value;
                PropriedadeAlterada();
            }
        }

        protected override void OnSalvaAlteracoes()
        {
            if (_configuracao == null)
            {
                _configuracao = new ConfiguracaoEmailDTO();
            }

            _configuracao.Email = Email.TrimOrEmpty();
            _configuracao.Senha = SimetricaCrip.Computar(Senha).TrimOrEmpty();
            _configuracao.Porta = Porta;
            _configuracao.Ssl = true;
            _configuracao.UrlServidorEmail = Smtp.TrimOrEmpty();
            _configuracao.ProtocoloSeguranca = Protocolo;
            _configuracao.UsarFusionZohoo = UserServidorProprio;
            _configuracao.EmailResposta = EmailResposta.TrimOrEmpty();

            if (!_configuracao.IsValido())
            {
                return;
            }

            using (var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                repositorio.Salva(_configuracao);
            }
        }

        public string EmailResposta
        {
            get => _emailResposta;
            set
            {
                if (value == _emailResposta) return;
                _emailResposta = value;
                PropriedadeAlterada();
            }
        }
    }
}