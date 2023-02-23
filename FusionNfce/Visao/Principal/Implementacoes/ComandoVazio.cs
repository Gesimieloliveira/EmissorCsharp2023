using FusionCore.FusionNfce.Venda;
using FusionNfce.Visao.Principal.Contratos;

namespace FusionNfce.Visao.Principal.Implementacoes
{
    public class ComandoVazio : IComandoVenda
    {
        public void ExecutaAcao(VendaModel model, ItemEspera item)
        {
            //ignore
        }
    }
}