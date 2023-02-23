using FusionCore.FusionNfce.Pagamento;

namespace FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos
{
    public interface IFormaPagamento
    {
        int Id { get; }
        int IdRepositorio { get; set; }
        string Descricao { get; }
        decimal Valor { get; }
        void DescontaTroco(decimal troco);
        FormaPagamentoNfce FormaPagamentoNfce { get; set; }
    }
}