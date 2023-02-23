using System;
using FusionCore.FusionAdm.Pessoas;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Pessoa.Flyouts
{
    public class PessoaEmailFlyoutModel : ViewModel
    {
        private readonly PessoaEntidade _pessoa;

        public bool IsOpen
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string EmailDigitado
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public PessoaEmailFlyoutModel(PessoaEntidade pessoa)
        {
            _pessoa = pessoa;
        }

        public event EventHandler<PessoaEmail> EmailAdicionado;

        public void AdicionarEmail()
        {
            try
            {
                var email = new FusionCore.FusionAdm.Componentes.Email(EmailDigitado);
                var pessoaEmail = new PessoaEmail(email);

                OnEmailAdicionado(pessoaEmail);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void OnEmailAdicionado(PessoaEmail e)
        {
            EmailAdicionado?.Invoke(this, e);
        }
    }
}