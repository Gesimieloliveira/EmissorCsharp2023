using System;
using System.ComponentModel.DataAnnotations;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValorMinimo : ValidationAttribute
    {
        private readonly double _minimo;

        public ValorMinimo(double minimo)
        {
            _minimo = minimo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valor = double.Parse(value.ToString());

            if (valor >= _minimo)
                return ValidationResult.Success;


            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage);
        }
    }
}