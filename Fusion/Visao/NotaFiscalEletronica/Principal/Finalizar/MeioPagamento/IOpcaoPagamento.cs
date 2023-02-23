using System;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar.MeioPagamento
{
    public interface IOpcaoPagamento
    {
        string Descricao { get; }
        void PagarAsync(decimal valor, Action<Resultado> callback);
    }
}