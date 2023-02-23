using System;

namespace Fusion.Conversor.Core.Helpers
{
    public class IntegerConverter
    {
        public int Converte(string input)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "null")
            {
                return default(int);
            }

            if (int.TryParse(input, out var intValue))
            {
                return intValue;
            }

            throw new InvalidOperationException($"Falha ao converter '{input}' para Inteiro");
        }
    }
}