using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Modelos.FormaPagamento;

namespace FusionPdv.Modelos.Pagamento
{
    public interface IPagamento
    {
        decimal Valor { get; set; }
        IFormaPagamento FormaPagamento { get; set; }
        bool Pagou { get; set; }
        void Calcula(VendaEcfDt vendaEcfDt);
    }
}
