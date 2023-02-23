namespace FusionCore.Tributacoes.Calculadoras
{
    public class CalculadoraIpi
    {
        public decimal ValorTributavel { get; set; }
        public decimal Aliquota { get; set; }

        public Resultado Calcula()
        {
            if (Aliquota == 0)
            {
                return new Resultado(0, 0);
            }

            var bc = decimal.Round(ValorTributavel, 2);
            var valor = decimal.Round(ValorTributavel * Aliquota / 100, 2);

            return new Resultado(bc, valor);
        }
    }
}