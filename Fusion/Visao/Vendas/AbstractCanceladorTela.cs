using System;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas
{
    public abstract class AbstractCanceladorTela
    {
        public abstract void Cancelar(FaturamentoVenda venda);
        public event EventHandler CancelouComSucesso;
    }
}