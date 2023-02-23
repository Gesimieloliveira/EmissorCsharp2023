using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Contigencia
{
    public class ContigenciaFormModel : ModelValidation
    {
        private bool _isEntrarContingencia;
        private bool _isSairContingencia;
        private NfceContingencia _contingencia;
        private bool _isJustificativaEnabled;

        public event EventHandler FecharWindow;

        public ContigenciaFormModel()
        {
            IsSairContingencia = false;
            IsEntrarContingencia = true;
            IsJustificativaEnabled = true;

            InicializaContingenciaSeTiver();
        }

        private void InicializaContingenciaSeTiver()
        {
            BuscaContingenciaAtiva();

            if (_contingencia == null)
            {
                _contingencia = new NfceContingencia
                {
                    EntrouEm = DateTime.Now
                };
                return;
            }

            Justificativa = _contingencia.Motivo;

            IsJustificativaEnabled = false;
            IsEntrarContingencia = false;
            IsSairContingencia = true;
        }

        public bool IsJustificativaEnabled
        {
            get => _isJustificativaEnabled;
            set
            {
                if (value == _isJustificativaEnabled) return;
                _isJustificativaEnabled = value;
                PropriedadeAlterada();
            }
        }

        private void BuscaContingenciaAtiva()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioNfce(sessao);

                _contingencia = repositorio.BuscarContingenciaAtiva();
            }
        }

        public ICommand CommandEntrarContingencia => GetSimpleCommand(ActionEntrarContingencia);

        public ICommand CommandSairContingencia => GetSimpleCommand(ActionSairContingencia);

        public bool IsEntrarContingencia
        {
            get => _isEntrarContingencia; set
            {
                if (value == _isEntrarContingencia) return;
                _isEntrarContingencia = value;
                PropriedadeAlterada();
            }
        }

        public bool IsSairContingencia
        {
            get => _isSairContingencia;
            set
            {
                if (value == _isSairContingencia) return;
                _isSairContingencia = value;
                PropriedadeAlterada();
            }
        }

        [Required(ErrorMessage = @"Porfavor digitar uma justificativa")]
        [MinLength(15, ErrorMessage = @"Porfavor digitar no mínimo 15 caracteres")]
        public string Justificativa
        {
            get => GetValue(() => Justificativa); set => SetValue(value);
        }

        private void ActionEntrarContingencia(object obj)
        {
            Justificativa = Justificativa.TrimOrEmpty();

            if (Justificativa.Length != 0)
                Justificativa = Justificativa.RemoverAcentos();

            if (Justificativa.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Digitar uma justificativa");
                return;
            }

            if (Justificativa.Length < 15)
            {
                DialogBox.MostraInformacao("A justificativa devera ser maior ou igual a 15 digitos");
                return;
            }

            if (!DialogBox.MostraConfirmacao("Deseja realmente entrar em contingência?",
                MessageBoxImage.Question)) return;


            _contingencia.Ativa = true;
            _contingencia.Motivo = Justificativa;
            
            SalvarContingencia();

            SessaoSistemaNfce.Contingencia = _contingencia;

            IsEntrarContingencia = false;
            IsSairContingencia = true;
            IsJustificativaEnabled = false;

            DialogBox.MostraInformacao("Entrei em modo contingência");

            OnFechar();
        }

        private void SalvarContingencia()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfce(sessao);

                repositorio.SalvarContingencia(_contingencia);

                transacao.Commit();
            }
        }

        private void ActionSairContingencia(object obj)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente sair da contingência?",
                MessageBoxImage.Question)) return;

            _contingencia.Ativa = false;
            SalvarContingencia();

            SessaoSistemaNfce.Contingencia = _contingencia;

            Justificativa = string.Empty;
            IsSairContingencia = false;
            IsEntrarContingencia = true;
            IsJustificativaEnabled = true;

            DialogBox.MostraInformacao("Sai em modo contingência");

            OnFechar();
        }

        protected virtual void OnFechar()
        {
            FecharWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}