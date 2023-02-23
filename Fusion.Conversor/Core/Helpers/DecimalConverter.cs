using System;
using System.Globalization;

// ReSharper disable RedundantTypeSpecificationInDefaultExpression

namespace Fusion.Conversor.Core.Helpers
{
    public class DecimalConverter
    {
        public string SeparadorDecimal { get; set; } = ".";
        public bool PositivarNegativo { get; set; }

        public decimal Converte(string input, byte precisao)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "null")
            {
                return default(decimal);
            }

            var format = new NumberFormatInfo
            {
                NumberDecimalSeparator = SeparadorDecimal,
                NumberDecimalDigits = precisao
            };

            if (!decimal.TryParse(input, NumberStyles.Number, format, out var output))
            {
                throw new InvalidOperationException($"Não foi possível converter '{input}' para decimal");
            }

            return PositivarNegativo && output < 0 ? 0 : output;
        }
    }
}