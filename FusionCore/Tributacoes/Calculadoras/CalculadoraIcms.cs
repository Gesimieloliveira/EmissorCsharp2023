namespace FusionCore.Tributacoes.Calculadoras
{
    public class CalculadoraIcms
    {
        public decimal ValorTributavel { get; set; }
        public decimal Reducao { get; set; }
        public decimal Aliquota { get; set; }
        public decimal ValorIpi { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorOutros { get; set; }

        public Resultado Calcula()
        {
            if (Aliquota == 0)
            {
                return Resultado.Zerado;
            }

            var valorTributar = ValorTributavel + ValorIpi + ValorFrete + ValorSeguro + ValorOutros;
            var percReducao = 1 - Reducao / 100;

            var bc = decimal.Round(valorTributar * percReducao, 2);
            var valorIcms = decimal.Round(bc * Aliquota / 100, 2);

            return new Resultado(bc, valorIcms);
        }
    }
}