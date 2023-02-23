using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Produto
{
    public class ProdutoUnidadeNfce
    {
        public ProdutoUnidadeNfce() { }

        public ProdutoUnidadeNfce(ProdutoUnidadeDTO unidadeMedida)
        {
            CopiarInformacoes(unidadeMedida);
        }

        public int Id { get; set; }
        public string Sigla { get; set; }
        public bool PodeFracionar { get; set; }
        public bool SolicitaTotal { get; set; }

        private void CopiarInformacoes(ProdutoUnidadeDTO unidadeMedida)
        {
            Id = unidadeMedida.Id;
            Sigla = unidadeMedida.Sigla;
            PodeFracionar = unidadeMedida.PodeFracionar != 0;
            SolicitaTotal = unidadeMedida.SolicitaTotalPdv;
        }
    }
}