namespace FusionCore.Tributacoes.Calculadoras
{
    public class CalculadoraIcmsSt
    {
        public decimal ValorTributavel { get; set; }
        public decimal Reducao { get; set; }
        public decimal AliquotaIcmsInterno { get; set; }
        public decimal Aliquota { get; set; }
        public decimal ValorIpi { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorOutros { get; set; }
        public decimal Mva { get; set; }

        private Resultado CalcularIcmsInterno()
        {
            if (AliquotaIcmsInterno == 0)
            {
                return Resultado.Zerado;
            }

            var tributavel = ValorTributavel + ValorFrete + ValorSeguro + ValorOutros;
            var percIcms = AliquotaIcmsInterno / 100;

            var bc = decimal.Round(tributavel, 2);
            var valor = decimal.Round(bc * percIcms, 2);

            return new Resultado(bc, valor);
        }

        public Resultado Calcula()
        {
            if (Aliquota == 0)
            {
                return Resultado.Zerado;
            }

            var icmsInterno = CalcularIcmsInterno();

            var tributavel = ValorTributavel + ValorFrete + ValorSeguro + ValorOutros + ValorIpi;
            var percMva = 1 + Mva / 100;
            var percReducao = 1 - Reducao / 100;

            var bc = decimal.Round(tributavel * percMva * percReducao, 2);
            var valorSt = decimal.Round(bc * Aliquota / 100, 2);

            var valorStAjustado = valorSt - icmsInterno.Valor;
            var valorStFinal = valorStAjustado <= 0 ? 0 : valorStAjustado;

            return new Resultado(bc, valorStFinal);
        }
    }
}