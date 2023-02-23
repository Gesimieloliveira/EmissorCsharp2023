using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionNfce.Produto
{
    public class ProdutoBaseDTO : IProdutoTabelaPreco
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Referencia { get; set; }
        public decimal Estoque { get; set; }
        public string SiglaUnidade { get; set; }
        public decimal PrecoVenda { get; set; }
        public string CodigoNcm { get; set; }
        public decimal PrecoOriginal { get; set; }

        public string NomeReferencia => ObterNomeComReferencia();

        private string ObterNomeComReferencia()
        {
            if (Referencia.IsNullOrEmpty())
                return Nome;

            return $"{Nome} - {Referencia}";
        }
    }
}