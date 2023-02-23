using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valor = (string)value;

            if(string.IsNullOrEmpty(valor))
                return ValidationResult.Success;

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            var match = regex.Match(valor);
            if (match.Success)
                return ValidationResult.Success;


            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage);
        }
    }
}
