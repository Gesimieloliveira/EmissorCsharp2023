namespace FusionCore.Sintegra.Dto
{
    public interface IRegistro54Dto
    {
        string GetDocumentoUnico();
        int GetModelo();
        string GetSerie();
        int GetNumero();
        int GetCfop();
        string GetCst();
        int GetNumeroItem();
        string GetCodigoProdutoServico();
        decimal GetQuantidade();
        decimal GetValorProduto();
        decimal GetValorTotalDescontos();
        decimal GetBaseCalculoIcms();
        decimal GetBaseCalculoIcmsSt();
        decimal GetValorIpi();
        decimal GetAliquotaIcms();
    }
}