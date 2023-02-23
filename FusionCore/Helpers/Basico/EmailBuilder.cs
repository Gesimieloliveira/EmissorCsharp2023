using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Componentes;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;

namespace FusionCore.Helpers.Basico
{
    public interface IAnexo
    {
        Stream Content { get; }
        string Nome { get; }
    }

    public class Anexo : IAnexo
    {
        public Anexo(Stream content, string nome)
        {
            Content = content;
            Nome = nome;
        }

        public Stream Content { get; }
        public string Nome { get; }
    }

    public class EmailBuilder : IDisposable
    {
        private readonly IConfiguracaoEmail _cfgEmail;
        private readonly List<string> _destinatarios;
        private readonly List<string> _anexos;
        private readonly List<Anexo> _anexosStream;
        private string _assuntoEmail;
        private string _mensagemDoEmail;

        public EmailBuilder(IConfiguracaoEmail cfgEmail)
        {
            _cfgEmail = cfgEmail;
            _destinatarios = new List<string>();
            _anexos = new List<string>();
            _anexosStream = new List<Anexo>();
        }

        public void Dispose()
        {
            foreach (var anexo in _anexosStream)
            {
                anexo.Content.Close();
            }

            _anexosStream.Clear();
        }

        public EmailBuilder AddDestinatarios(IEnumerable<string> emails)
        {
            foreach (var email in emails)
            {
                AddDestinatario(email);
            }

            return this;
        }

        public EmailBuilder AddDestinatario(string email)
        {
            if (new EmailRegra().NaoValido(email))
            {
                throw new ArgumentException("E-mail é inválido");
            }

            _destinatarios.Add(email);

            return this;
        }

        public EmailBuilder AddEmail(Email email)
        {
            _destinatarios.Add(email.Valor);
            return this;
        }

        public EmailBuilder Assunto(string assuntoDoEmail)
        {
            _assuntoEmail = assuntoDoEmail;
            return this;
        }

        public EmailBuilder Mensagem(string mensagemDoEmail)
        {
            _mensagemDoEmail = mensagemDoEmail;
            return this;
        }

        public EmailBuilder AddAnexo(string anexo)
        {
            _anexos.Add(anexo);
            return this;
        }

        public EmailBuilder AddAnexo(Stream anexo, string nome)
        {
            _anexosStream.Add(new Anexo(anexo, nome));
            return this;
        }

        public void Enviar()
        {
            if (_cfgEmail == null)
            {
                throw new InvalidOperationException("Porfavor configurar o envio de e-mail.");
            }

            if (_cfgEmail.UsarFusionZohoo && _cfgEmail.EmailResposta.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Adicione um email de resposta em configurações de e-mail.");
            }

            var senhaEmail = SimetricaCrip.Descomputar(_cfgEmail.Senha);
            ServicePointManager.SecurityProtocol = _cfgEmail.UsarFusionZohoo ? SecurityProtocolType.Tls12 : _cfgEmail.ProtocoloSeguranca.ToSecurityProtocol();

            using (var mailMessage = new MailMessage())
            {
                // TODO 1612 - AWS EMAIL
                mailMessage.From = new MailAddress(_cfgEmail.UsarFusionZohoo ? "email@email.com" : _cfgEmail.Email);
                mailMessage.Subject = _assuntoEmail;
                mailMessage.Body = _mensagemDoEmail;
                mailMessage.IsBodyHtml = true;

                foreach (var destinatario in _destinatarios)
                {
                    mailMessage.To.Add(destinatario);
                }

                foreach (var anexo in _anexos)
                {
                    var attach = new Attachment(anexo, MediaTypeNames.Application.Octet);
                    mailMessage.Attachments.Add(attach);
                }

                foreach (var anexo in _anexosStream)
                {
                    var attach = new Attachment(anexo.Content, anexo.Nome);
                    mailMessage.Attachments.Add(attach);
                }

                if (_cfgEmail.UsarFusionZohoo)
                    mailMessage.ReplyToList.Add(new MailAddress(_cfgEmail.EmailResposta));

                var smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Host = _cfgEmail.UsarFusionZohoo ? "email-smtp.us-east-1.amazonaws.com" : _cfgEmail.UrlServidorEmail,
                    Port = _cfgEmail.UsarFusionZohoo ? 587 : _cfgEmail.Porta,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //TODO 1612 - AWS EMAIL CREDENCIAIS
                    Credentials = new NetworkCredential(_cfgEmail.UsarFusionZohoo ? "AWS_EMAIL_USER" : _cfgEmail.Email, _cfgEmail.UsarFusionZohoo ? "AWS_EMAIL_PASSWORD" : senhaEmail)
                };

                smtpClient.Send(mailMessage);
            }
        }
    }
}