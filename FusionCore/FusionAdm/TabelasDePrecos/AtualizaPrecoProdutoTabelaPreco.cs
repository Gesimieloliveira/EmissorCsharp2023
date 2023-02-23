using FusionCore.FusionAdm.TabelasDePrecos.Dtos;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class AtualizaPrecoProdutoTabelaPreco
    {
        private readonly ITabelaPreco _tabelaPrecoNfce;

        public AtualizaPrecoProdutoTabelaPreco(ITabelaPreco tabelaPrecoNfce)
        {
            _tabelaPrecoNfce = tabelaPrecoNfce;
        }

        public IProdutoTabelaPreco CalculaProdutoComBaseTabelaPreco(IRepositorioAjusteDiferenciado repositorioAjusteDiferenciado, IProdutoTabelaPreco produto)
        {
            if (produto == null) return null;
            if (_tabelaPrecoNfce == null) return produto;

            var ajusteDiferenciadoDto = repositorioAjusteDiferenciado.BuscarAjusteDiferenciado(produto.Id, _tabelaPrecoNfce.Id);

            if (ajusteDiferenciadoDto != null)
            {
                produto.PrecoVenda = ajusteDiferenciadoDto.Calcula(produto);
                return produto;
            }

            if (_tabelaPrecoNfce.ApenasItensDaLista)
            {
                return produto;
            }

            var tabelaPreco = _tabelaPrecoNfce;

            var ajusteDto = new AjusteDiferenciadoDto
            {
                PercentualAjuste = tabelaPreco.PercentualAjuste,
                TipoAjustePreco = tabelaPreco.TipoAjustePreco
            };

            var valorNovo = ajusteDto.Calcula(produto);

            produto.PrecoVenda = valorNovo;
            return produto;
        }
    }
}