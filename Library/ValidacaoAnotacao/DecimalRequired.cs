using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FusionLibrary.ValidacaoAnotacao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalRequired : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMessage = FormatErrorMessage(validationContext.DisplayName);

            if (value == null)
            {
                return new ValidationResult(errorMessage);
            }

            try
            {
                var decimalValue = decimal.Parse(value.ToString(), NumberStyles.Currency);

                return decimalValue <= 0 
                    ? new ValidationResult(errorMessage) 
                    : ValidationResult.Success;
            }
            catch (InvalidCastException)
            {
                //ignore
            }

            return new ValidationResult(errorMessage);
        }
    }
}