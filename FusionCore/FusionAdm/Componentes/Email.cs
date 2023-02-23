using System;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using static System.String;

// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.Componentes
{
    public class Email : IComponenteValorUnico<string>
    {
        private readonly string _valor;
        public string Valor => _valor;
        public static Email Vazio => new Email();

        private Email()
        {
            _valor = Empty;
        }

        public Email(string email)
        {
            var valor = email?.TrimOrEmpty() ?? Empty;

            if (new EmailRegra().NaoValido(valor))
            {
                throw new InvalidOperationException(@"E-mail inválido. Não corresponde ao padrão seuemail@dominio.xx");
            }

            _valor = valor.ToLower();
        }

        public static bool IsValido(string email)
        {
            return new EmailRegra().Valido(email);
        }

        public override string ToString()
        {
            return Valor;
        }

        private bool Equals(Email other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Email) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }

        public static bool operator ==(Email left, Email right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Email left, Email right)
        {
            return !Equals(left, right);
        }
    }
}