using FusionCore.Comum;

namespace FusionCore.FusionAdm.TabelasDePrecos.Dtos
{
    public class TabelaPrecoDto : Comparavel<int>, ITabelaPreco
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public string Descricao { get; set; }
        public TipoAjustePreco TipoAjustePreco { get; set; }
        public decimal PercentualAjuste { get; set; }
        public bool ApenasItensDaLista { get; set; }
        public bool Status { get; set; }

        public override string ToString()
        {
            return Descricao;
        }
    }
}