using System;
using System.Text.RegularExpressions;
using Fusion.Conversor.Core.Helpers;

namespace Fusion.Conversor.Core.Resolvedores
{
    public class ResolvedorCodigo
    {
        private StringPreparer StringPreparer = new StringPreparer();

        public int Resolve(string input)
        {
            input = StringPreparer.RemoveNaoNumeros(input) ?? string.Empty;

            if (!Regex.IsMatch(input, @"[0-9]{1,11}"))
            {
                throw new InvalidOperationException($"Código é inválido: {input}");
            }

            if (int.TryParse(input, out var id))
            {
                return id;
            }

            throw new InvalidOperationException($"Não foi possível converter o código: {input} para número inteiro");
        }
    }
}