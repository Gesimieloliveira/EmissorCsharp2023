namespace FusionCore.Sintegra.Dto
{
    public interface IRegistro75Dto
    {
        string GetCodigoProdutoServico();
        string GetCodigoNcm();
        string GetDescricao();
        string GetUnidadeMedida();
        decimal GetAliquotaIpi();
        decimal GetAliquotaIcms();
        decimal GetReducaoIcms();
        decimal GetBaseCalculoIcmsSt();
    }
}