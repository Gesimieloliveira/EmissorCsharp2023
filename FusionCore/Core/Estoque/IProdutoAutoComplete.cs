namespace FusionCore.Core.Estoque
{
    public interface IProdutoAutoComplete : IProdutoSelecionado
    {
        decimal Estoque { get; }
        decimal PrecoVenda { get; }
    }
}