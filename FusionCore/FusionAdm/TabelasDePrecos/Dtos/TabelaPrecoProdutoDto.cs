using Amazon.S3.Model;

namespace FusionCore.FusionAdm.TabelasDePrecos.Dtos
{
    public class TabelaPrecoProdutoDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string Descricao { get; set; }
        public TipoAjustePreco TipoAjustePreco { get; set; }
        public decimal PercentualAjuste { get; set; }
        public decimal PercentualAjusteDiferenciado { get; set; }
    }
}