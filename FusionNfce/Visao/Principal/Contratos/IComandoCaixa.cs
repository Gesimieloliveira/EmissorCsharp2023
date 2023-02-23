using FusionCore.FusionNfce.Produto;

namespace FusionNfce.Visao.Principal.Contratos
{
    public interface IComandoCaixa
    {
        void ExecutaAcao(VendaModel model, string cmd, ProdutoNfce produtoBuscaManual = null);
    }
}