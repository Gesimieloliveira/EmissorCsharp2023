using System;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento
{
    public interface IOpcaoPagamento
    {
        string Descricao { get; }
        void PagarAsync(decimal valor, Action<Resultado> callback);
        bool PermiteTroco();
    }
}