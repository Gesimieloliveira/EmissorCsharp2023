using System;
using System.Text.RegularExpressions;
using FusionCore.Core.Flags;

namespace FusionCore.Core.Tributario
{
    public static class CodigoCfopHelper
    {
        public static bool IsEntrada(string codigo)
        {
            return Regex.IsMatch(codigo, "^[123]");
        }

        public static bool IsSaida(string codigo)
        {
            return Regex.IsMatch(codigo, "^[567]");
        }

        public static bool IsInterMunicipal(string codigo)
        {
            return Regex.IsMatch(codigo, "^[15]");
        }

        public static bool IsInterEstadual(string codigo)
        {
            return Regex.IsMatch(codigo, "^[26]");
        }

        public static bool IsImportacao(string codigo)
        {
            return Regex.IsMatch(codigo, "^[37]");
        }

        public static OpercaoCfop ObtemOperacao(string codigo)
        {
            if (IsEntrada(codigo))
            {
                return OpercaoCfop.Entrada;
            }

            if (IsSaida(codigo))
            {
                return OpercaoCfop.Saida;
            }

            throw new InvalidOperationException(
                $"Não foi possível identificar operação entada/saida do CFOP: {codigo}");
        }

        public static OrigemOperacao ObtemOrigem(string codigo)
        {
            if (IsInterEstadual(codigo))
            {
                return OrigemOperacao.InterEstadual;
            }

            if (IsImportacao(codigo))
            {
                return OrigemOperacao.Importacao;
            }

            if (IsInterMunicipal(codigo))
            {
                return OrigemOperacao.InterMunicipal;
            }

            throw new InvalidOperationException(
                $"Não foi possível identificar origem municipal/estadua/importacao do CFOP: {codigo}");
        }
    }
}