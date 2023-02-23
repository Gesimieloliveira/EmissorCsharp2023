namespace FusionCore.Tributacoes.Calculadoras
{
    public class CalculadoraCredito
    {
        public decimal ValorTributavel { get; set; }
        public decimal Aliqutoa { get; set; }

        public Resultado Calcula()
        {
            if (Aliqutoa == 0)
            {
                return Resultado.Zerado;
            }

            var percentual = Aliqutoa / 100;
            var bc = decimal.Round(ValorTributavel, 2);
            var valor = decimal.Round(bc * percentual, 2);

            return new Resultado(bc, valor);
        }
    }
}