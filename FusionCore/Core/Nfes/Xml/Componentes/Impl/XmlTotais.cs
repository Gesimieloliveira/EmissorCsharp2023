using NFe.Classes.Informacoes.Total;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlTotais
    {
        public XmlTotais(ICMSTot total)
        {
            BcIcms = total.vBC;
            ValorIcms = total.vICMS;
            ValorIcmsDesonerado = total.vICMSDeson ?? 0.00M;
            BcIcmsSt = total.vBCST;
            ValorIcmsSt = total.vST;
            ValorProdutos = total.vProd;
            ValorFrete = total.vFrete;
            ValorSeguro = total.vSeg;
            ValorDesconto = total.vDesc;
            ValorOutros = total.vOutro;
            ValorIpi = total.vIPI;
            ValorPis = total.vPIS;
            ValorCofins = total.vCOFINS;
            ValorNf = total.vNF;
        }

        public decimal BcIcms { get; }
        public decimal ValorIcms { get; }
        public decimal ValorIcmsDesonerado { get; }
        public decimal BcIcmsSt { get; }
        public decimal ValorIcmsSt { get; }
        public decimal ValorProdutos { get; }
        public decimal ValorFrete { get; }
        public decimal ValorSeguro { get; }
        public decimal ValorDesconto { get; }
        public decimal ValorOutros { get; }
        public decimal ValorIpi { get; }
        public decimal ValorPis { get; }
        public decimal ValorCofins { get; }
        public decimal ValorNf { get; }
    }
}