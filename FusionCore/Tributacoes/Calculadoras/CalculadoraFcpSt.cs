namespace FusionCore.Tributacoes.Calculadoras
{
    public class CalculadoraFcpSt
    {
        public decimal ValorTributavel { get; set; }
        public decimal Percentual { get; set; }

        public Resultado Calcula()
        {
            if (Percentual == 0)
            {
                return Resultado.Zerado;
            }

            var bc = decimal.Round(ValorTributavel, 2);
            var perc = Percentual / 100;

            var valor = decimal.Round(perc * bc, 2);

            return new Resultado(bc, valor);
        }
    }
}