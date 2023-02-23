using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterThan : ValidationAttribute
    {
        private string NumeroParaComprarNome { get; }

        public GreaterThan(string numeroParaComprarNome)
        {
            NumeroParaComprarNome = numeroParaComprarNome;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valor = (int) value;

            var parametro = (int)validationContext.ObjectType.GetProperty(NumeroParaComprarNome).GetValue(validationContext.ObjectInstance, null);


            if (Comparer<object>.Default.Compare(valor, parametro) >= 1)
            {
                return ValidationResult.Success;
            }

            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage);
        }
    }
}
