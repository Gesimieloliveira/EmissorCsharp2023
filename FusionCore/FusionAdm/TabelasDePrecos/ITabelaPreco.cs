namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public interface ITabelaPreco
    {
        int Id { get; set; }
        string Descricao { get; set; }
        TipoAjustePreco TipoAjustePreco { get; set; }
        decimal PercentualAjuste { get; set; }
        bool ApenasItensDaLista { get; set; }
        bool Status { get; set; }
    }
}