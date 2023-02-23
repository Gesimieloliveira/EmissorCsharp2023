using FusionCore.FusionAdm.TabelasDePrecos.Dtos;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class AtualizaPrecosComTabelaPreco
    {
        public static void AjusteTabelaPreco(ITabelaPreco tabelaPreco,
            IRepositorioAjusteDiferenciado repositorioAjusteDiferenciado,
            IProdutoTabelaPreco produto,
            AtualizaPrecosCalculadosPorTabelaPreco atualizaPreco)
        {
            if (tabelaPreco == null) return;

            var ajusteDiferenciado = repositorioAjusteDiferenciado.BuscarAjusteDiferenciado(produto.Id, tabelaPreco.Id);

            if (ajusteDiferenciado != null)
            {
                var valorAjustado = ajusteDiferenciado.Calcula(produto);

                atualizaPreco.AtualizarPrecos(valorAjustado);
                return;
            }

            if (tabelaPreco.ApenasItensDaLista) return;


            var ajusteDto = new AjusteDiferenciadoDto
            {
                PercentualAjuste = tabelaPreco.PercentualAjuste,
                TipoAjustePreco = tabelaPreco.TipoAjustePreco
            };

            var valorNovo = ajusteDto.Calcula(produto);

            atualizaPreco.AtualizarPrecos(valorNovo);
        }
    }
}