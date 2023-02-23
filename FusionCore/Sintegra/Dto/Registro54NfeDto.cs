using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.Sintegra.Dto
{
    public class Registro54NfeDto : IRegistro54Dto
    {
        public string DocumentoUnico { get; set; }
        public ModeloDocumento Modelo { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public string Cfop { get; set; }
        public string Cst { get; set; }
        public int NumeroItem { get; set; }
        public int CodigoProdutoServico { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorProduto { get; set; }
        public decimal ValorTotalDescontos { get; set; }
        public decimal BaseCalculoIcms { get; set; }
        public decimal BaseCalculoIcmsSt { get; set; }
        public decimal ValorIpi { get; set; }
        public decimal AliquotaIcms { get; set; }

        public string GetDocumentoUnico()
        {
            return DocumentoUnico;
        }

        public int GetModelo()
        {
            return (int) ModeloDocumento.NFe;
        }

        public string GetSerie()
        {
            return Serie.ToString("D3");
        }

        public int GetNumero()
        {
            return TrataNumeroSintegra.Trata(Numero);
        }

        public int GetCfop()
        {
            return int.Parse(Cfop);
        }

        public string GetCst()
        {
            return Cst.PadLeft(3, '0');
        }

        public int GetNumeroItem()
        {
            return NumeroItem;
        }

        public string GetCodigoProdutoServico()
        {
            return CodigoProdutoServico.ToString();
        }

        public decimal GetQuantidade()
        {
            return Quantidade;
        }

        public decimal GetValorProduto()
        {
            return ValorProduto;
        }

        public decimal GetValorTotalDescontos()
        {
            return ValorTotalDescontos;
        }

        public decimal GetBaseCalculoIcms()
        {
            return BaseCalculoIcms;
        }

        public decimal GetBaseCalculoIcmsSt()
        {
            return BaseCalculoIcmsSt;
        }

        public decimal GetValorIpi()
        {
            return ValorIpi;
        }

        public decimal GetAliquotaIcms()
        {
            return AliquotaIcms;
        }
    }
}