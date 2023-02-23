using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using FusionCore.FusionAdm.Componentes;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.SendMail
{
    public class EnvioEmailBehavior : ViewModel
    {
        public ObservableCollection<Email> Emails { get; set; } = new ObservableCollection<Email>();

        public string EmailDigitado
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Assunto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string CorpoMensagem
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool WindowIsEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public EnvioEmailBehavior()
        {
            WindowIsEnable = true;
        }

        public event EventHandler<IEnumerable<Email>> DespacharEmails;
        internal event EventHandler<Exception> FalhaEnvio;
        internal event EventHandler SucessoEnvio;

        public void AdicionarEmailDigitado()
        {
            if (string.IsNullOrWhiteSpace(EmailDigitado))
            {
                DialogBox.MostraInformacao("Preciso que digite um e-mail");
                return;
            }

            try
            {
                var email = new Email(EmailDigitado);
                Emails.Add(email);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        public void RemoverEmail(Email email)
        {
            Emails.Remove(email);
        }

        public void EnviarEmailAsync()
        {
            if (!Emails.Any())
            {
                throw new InvalidOperationException("É preciso ter no mínimo um e-mail de destino");
            }

            if (string.IsNullOrEmpty(Assunto))
            {
                throw new InvalidOperationException("É preciso informar um Assunto para o e-mail");
            }

            if (string.IsNullOrEmpty(CorpoMensagem))
            {
                throw new InvalidOperationException("É preciso informar a Mensagem para o corpo do e-mail");
            }

            var thread = new Thread(EnviarEmailAction)
            {
                IsBackground = true
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void EnviarEmailAction()
        {
            WindowIsEnable = false;

            try
            {
                DespacharEmails?.Invoke(this, Emails);
                SucessoEnvio?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                FalhaEnvio?.Invoke(this, e);
            }
            finally
            {
                WindowIsEnable = true;
            }
        }
    }
}