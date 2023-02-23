using System;
using System.ComponentModel.DataAnnotations;
using FusionLibrary.Helper.Diversos;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ApenasNumeros : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = (string) value;

            if (string.IsNullOrEmpty(input))
            {
                return ValidationResult.Success;
            }

            return input.PossuiApenasNumeros()
                ? ValidationResult.Success
                : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}