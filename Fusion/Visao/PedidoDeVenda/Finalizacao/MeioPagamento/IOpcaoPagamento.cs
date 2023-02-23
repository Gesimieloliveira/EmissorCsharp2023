using System;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao.MeioPagamento
{
    public interface IOpcaoPagamento
    {
        string Descricao { get; }
        void PagarAsync(decimal valor, Action<Resultado> callback);
    }
}