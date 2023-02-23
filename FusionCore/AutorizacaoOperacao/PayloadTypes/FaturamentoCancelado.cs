namespace FusionCore.AutorizacaoOperacao.PayloadTypes
{
    public class FaturamentoCancelado : IPayload
    {
        public FaturamentoCancelado(int id, decimal valor)
        {
            Id = id;
            Valor = valor;
        }

        public int Id { get; private set; }
        public decimal Valor { get; private set; }
    }
}
