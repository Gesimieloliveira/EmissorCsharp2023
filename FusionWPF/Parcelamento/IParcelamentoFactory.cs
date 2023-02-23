using FusionCore.Seguranca.Licenciamento.Dominio;

namespace FusionWPF.Parcelamento
{
    public interface IParcelamentoFactory
    {
        ParcelamentoDialog CriaDialog(decimal valor);
        IRepositorioParcelamento CriarRepositorio();
        AcessoConcedido GetAcessoConcedido();
    }
}