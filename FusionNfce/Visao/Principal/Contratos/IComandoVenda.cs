using FusionCore.FusionNfce.Venda;

namespace FusionNfce.Visao.Principal.Contratos
{
    public interface IComandoVenda
    {
        void ExecutaAcao(VendaModel model, ItemEspera item);
    }
}