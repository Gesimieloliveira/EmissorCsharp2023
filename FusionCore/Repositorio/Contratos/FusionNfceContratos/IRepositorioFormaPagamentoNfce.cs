using FusionCore.FusionNfce.Pagamento;

namespace FusionCore.Repositorio.Contratos.FusionNfceContratos
{
    public interface IRepositorioFormaPagamentoNfce : IRepositorio<FormaPagamentoNfce, string>
    {
        void Salvar(FormaPagamentoNfce formaPagamento);
    }
}