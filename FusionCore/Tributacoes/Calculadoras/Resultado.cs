namespace FusionCore.Tributacoes.Calculadoras
{
    public struct Resultado
    {
        public Resultado(decimal bc, decimal valor)
        {
            Bc = bc;
            Valor = valor;
        }

        public static Resultado Zerado => new Resultado(0, 0);

        public decimal Bc { get; }
        public decimal Valor { get; }
    }
}