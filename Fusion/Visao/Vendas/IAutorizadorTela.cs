using System;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas
{
    public interface IAutorizadorTela
    {
        void EnviaSefaz(FaturamentoVenda venda, Action acao = null);
    }
}