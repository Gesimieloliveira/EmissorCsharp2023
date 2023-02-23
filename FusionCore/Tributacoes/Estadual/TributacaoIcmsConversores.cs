namespace FusionCore.Tributacoes.Estadual
{
    public static class TributacaoIcmsConversores
    {
        public static string ConverteParaAliquotaEcf(TributacaoIcms icms, decimal aliquota)
        {
            switch (icms.Codigo)
            {
                case "00":
                case "10":
                case "20":
                case "70":
                case "90":
                    return icms.IcmsOpcional() && aliquota <= 0 ? "NN" : aliquota.ToString("####") + "T";
                case "60":
                    return "FF";
                case "40":
                case "30":
                    return "II";
                case "41":
                    return "NN";
            }

            return "NN";
        }
    }
}
