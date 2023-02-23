using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas
{
    public interface IImprimirCupomFiscal
    {
        void Imprime(FaturamentoVenda venda);
    }
}