using System;
using System.ComponentModel.DataAnnotations;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TamanhoMinimo : ValidationAttribute
    {
        private readonly double _minimo;

        public TamanhoMinimo(double minimo)
        {
            _minimo = minimo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.Equals(""))
            {
                return ValidationResult.Success;
            }

            if (!(value is string v) || !(v.Length < _minimo))
            {
                return ValidationResult.Success;
            }

            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage);
        }
    }
}