using System;
using System.ComponentModel.DataAnnotations;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CnpjAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valor = (string)value;

            if (string.IsNullOrEmpty(valor))
                return ValidationResult.Success;


            if (new CnpjRegra().Valido(valor))
                return ValidationResult.Success;


            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage);
        }
    }
}
