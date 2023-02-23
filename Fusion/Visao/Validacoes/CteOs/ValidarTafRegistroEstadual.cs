using System;
using FusionCore.Helpers.Hidratacao;

namespace Fusion.Visao.Validacoes.CteOs
{
    public static class ValidarTafRegistroEstadual
    {
        public static void Executar(string taf, string registroEstadual)
        {
            if (taf.IsNotNullOrEmpty())
            {
                if (taf.Length != 12)
                    throw new InvalidOperationException("Taf deve ter 12 digitos");
            }

            if (registroEstadual.IsNotNullOrEmpty())
            {
                if (registroEstadual.Length != 25)
                    throw new InvalidOperationException("Número Registro Estadual deve ter 25 digitos");
            }
        }
    }
}