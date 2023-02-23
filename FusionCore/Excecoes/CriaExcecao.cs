using System;

namespace FusionCore.Excecoes
{
    public static class CriaExcecao
    {
        public static void OperacaoInvalida(string mensagem)
        {
            throw new InvalidOperationException(mensagem);
        }

        public static void ArgumentoInvalido(string mensagem)
        {
            throw new ArgumentException(mensagem);
        }
    }
}