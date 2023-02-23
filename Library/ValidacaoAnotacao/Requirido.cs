using System;
using System.ComponentModel.DataAnnotations;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Requirido : ValidationAttribute
    {
        private readonly string _nomePropriedadeCondicional;

        public Requirido(string nomePropriedadeCondicional)
        {
            _nomePropriedadeCondicional = nomePropriedadeCondicional;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_nomePropriedadeCondicional);
            var condicional = (bool) property.GetValue(validationContext.ObjectInstance, null);

            if (!condicional)
            {
                return ValidationResult.Success;
            }

            var errorMessage = FormatErrorMessage(validationContext.DisplayName);

            if (value == null)
            {
                return new ValidationResult(errorMessage);
            }

            var tipo = value.GetType();

            if (tipo == typeof(string) && string.IsNullOrEmpty((string) value))
                return new ValidationResult(errorMessage);


            return ValidationResult.Success;
        }
    }
}