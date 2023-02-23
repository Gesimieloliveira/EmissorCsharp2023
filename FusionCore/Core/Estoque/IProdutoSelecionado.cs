using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Core.Estoque
{
    public interface IProdutoSelecionado
    {
        int ProdutoId { get; }
        string Nome { get; set; }
        decimal PrecoVenda { get; set; }

        ProdutoDTO CarregaProduto();
    }
}