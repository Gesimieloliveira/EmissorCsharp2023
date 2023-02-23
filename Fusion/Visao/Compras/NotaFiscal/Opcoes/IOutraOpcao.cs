namespace Fusion.Visao.Compras.NotaFiscal.Opcoes
{
    public interface IOutraOpcao
    {
        string Titulo { get; }
        bool IsVisible { get; }
        void ExeuctaAcao(NotaFiscalCompraViewModel compraVm);
    }
}