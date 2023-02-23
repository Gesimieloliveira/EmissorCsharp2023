namespace FusionCore.FusionAdm.Fiscal.Transparencia
{
    public class BaseCalculo : IBaseCalculoIbpt
    {
        public decimal ValorIbpt { get; }

        public BaseCalculo(decimal valorIbpt)
        {
            ValorIbpt = valorIbpt;
        }
    }
}