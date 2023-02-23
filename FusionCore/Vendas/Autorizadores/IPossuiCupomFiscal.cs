using FusionCore.Vendas.Faturamentos;

namespace FusionCore.Vendas.Autorizadores
{
    public interface IPossuiCupomFiscal
    {
        void ExisteCupomFiscal(FaturamentoVenda venda);
    }
}