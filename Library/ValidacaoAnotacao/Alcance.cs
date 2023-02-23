using System;
using System.ComponentModel.DataAnnotations;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Alcance : ValidationAttribute
    {
        private readonly string _nomePropriedadeCondicional;
        private readonly int _minimo;
        private readonly int _maximo;

        public Alcance(string nomePropriedadeCondicional, int minimo, int maximo)
        {
            _nomePropriedadeCondicional = nomePropriedadeCondicional;
            _minimo = minimo;
            _maximo = maximo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_nomePropriedadeCondicional);
            var condicional  = (bool) property.GetValue(validationContext.ObjectInstance, null);

            if (condicional == false)
            {
                return ValidationResult.Success;
            }

            var errorMessage = FormatErrorMessage(validationContext.DisplayName);

            if (value == null)
            {
                return new ValidationResult(errorMessage);
            }

            try
            {
                var parsedValue = int.Parse(value.ToString());

                if (parsedValue < _minimo || parsedValue > _maximo)
                {
                    return new ValidationResult(errorMessage);
                }

                return ValidationResult.Success;
            }
            catch (InvalidCastException)
            {
                return new ValidationResult(errorMessage);
            }
        }
    }
}