namespace Fusion.Conversor.Core.Map
{
    public sealed class ProdutoCsvMap : MapBase<ProdutoCsv>
    {
        public ProdutoCsvMap()
        {
            Map(m => m.Codigo).Name("codigo");
            Map(m => m.Nome).Name("nome");
            Map(m => m.Estoque).Name("estoque");
            Map(m => m.Grupo).Name("grupo");
            Map(m => m.SiglaUnidade).Name("unidade");
            Map(m => m.PrecoCusto).Name("preco_custo");
            Map(m => m.PrecoCompra).Name("preco_compra");
            Map(m => m.MargemLucro).Name("margem_lucro");
            Map(m => m.PrecoVenda).Name("preco_venda");
            Map(m => m.CodigoBarra).Name("codigo_barra");
            Map(m => m.Ncm).Name("ncm");
            Map(m => m.Cest).Name("cest");
            Map(m => m.CodigoCst).Name("codigo_cst");
            Map(m => m.AliquotaCst).Name("aliquota_cst");
            Map(m => m.CodigoIpi).Name("codigo_ipi");
            Map(m => m.AliquotaIpi).Name("aliquota_ipi");
            Map(m => m.CodigoPis).Name("codigo_pis");
            Map(m => m.AliquotaPis).Name("aliquota_pis");
            Map(m => m.CodigoCofins).Name("codigo_cofins");
            Map(m => m.AliquotaCofins).Name("aliquota_cofins");
            Map(m => m.Referencia).Name("referencia");
            Map(m => m.CodigoBalanca).Name("codigo_balanca");
            Map(m => m.CodigoAnp).Name("codigo_anp");
            Map(m => m.CodigoEnquadramentoIpi).Name("codigo_enquadramento_ipi");
            Map(m => m.PercentualMva).Name("percentual_mva");
            Map(m => m.ReducaoIcms).Name("reducao_icms");
            Map(m => m.Observacao).Name("observacao");

            ColunasObrigatorias = "nome, preco_venda, ncm";
        }
    }
}