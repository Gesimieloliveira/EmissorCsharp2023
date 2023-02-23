namespace FusionCore.Vendas.Faturamentos
{
    public interface IFaturuamentoImprimivel
    {
        int Id { get; }
        Estado EstadoAtual { get; }
    }
}