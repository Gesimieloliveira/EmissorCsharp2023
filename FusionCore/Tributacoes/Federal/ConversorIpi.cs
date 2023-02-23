using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;

namespace FusionCore.Tributacoes.Federal
{
    public static class ConversorIpi
    {
        public static TributacaoIpi ToEntrada(this TributacaoIpi ipi)
        {
            var recipiente = RecipienteFactory.Get<RecipienteIpi>();

            switch (ipi.Codigo)
            {
                case "50": return recipiente.Get("00");
                case "51": return recipiente.Get("01");
                case "52": return recipiente.Get("02");
                case "53": return recipiente.Get("03");
                case "54": return recipiente.Get("04");
                case "55": return recipiente.Get("05");
                case "99": return recipiente.Get("49");
            }

            return recipiente.Get("49");
        }
    }
}