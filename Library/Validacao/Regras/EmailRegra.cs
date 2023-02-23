using System;
using System.Net.Mail;

namespace FusionLibrary.Validacao.Regras
{
    public class EmailRegra : IRegra
    {
        public bool AplicaRegra(object objeto)
        {
            try
            {
                var strEmail = objeto?.ToString();

                if (string.IsNullOrEmpty(strEmail) || strEmail.Contains(" "))
                {
                    return false;
                }

                var split = strEmail.Split('@');

                if (split.Length < 2 || !split[1].Contains("."))
                {
                    return false;
                }

                var m = new MailAddress(strEmail);
                return true;
            }
            catch (FormatException e)
            {
                return false;
            }
        }
    }
}