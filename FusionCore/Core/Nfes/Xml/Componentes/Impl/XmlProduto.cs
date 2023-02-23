using System.Linq;
using NFe.Classes.Informacoes.Detalhe;
using static System.Decimal;
using ProdutoCombustivel = NFe.Classes.Informacoes.Detalhe.ProdEspecifico.comb;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlProduto
    {
        private readonly decimal _valorTotal;

        public XmlProduto(prod item)
        {
            Nome = item.xProd.ToUpper();
            Codigo = item.cProd;
            Cean = item.cEAN;
            Ncm = item.NCM;
            Cfop = (short) item.CFOP;
            Unidade = item.uCom.ToUpper();
            Quantidade = item.qCom;
            _valorTotal = item.vProd;
            Cest = item.CEST ?? string.Empty;
            ValorFrete = item.vFrete ?? 0.00M;
            ValorSeguro = item.vSeg ?? 0.00M;
            ValorOutros = item.vOutro ?? 0.00M;
            ValorDesconto = item.vDesc ?? 0.00M;

            var comb = item.ProdutoEspecifico.FirstOrDefault(i => i.GetType() == typeof(ProdutoCombustivel)) as ProdutoCombustivel;

            CodigoAnp = comb?.cProdANP ?? string.Empty;
            DescricaoCodigoAnp = comb?.descANP ?? string.Empty;
            PercentualGlp = comb?.pGLP ?? 0;
            PercentualGasNatural = comb?.pGNn ?? 0;
            PercentualGasNaturalImportado = comb?.pGNi ?? 0;
            ValorPartida = comb?.vPart ?? 0;
        }

        public string Codigo { get; }
        public string Cean { get; }
        public string Nome { get; }
        public string Ncm { get; }
        public string Cest { get; }
        public short Cfop { get; }
        public string Unidade { get; }
        public decimal Quantidade { get; }
        public decimal ValorTotalComDesconto => _valorTotal - ValorDesconto;
        public decimal ValorUnitario => Round(_valorTotal / Quantidade, 4);
        public decimal ValorFrete { get; }
        public decimal ValorSeguro { get; }
        public decimal ValorOutros { get; }
        public decimal ValorDesconto { get; }
        public XmlIpi Ipi { get; set; }
        public XmlPis Pis { get; set; }
        public XmlCofins Cofins { get; set; }
        public IXmlIcms Icms { get; set; }
        public bool PossuiIpi => Ipi != null;
        public bool PossuiPis => Pis != null;
        public bool PossuiCofins => Cofins != null;
        public string CodigoAnp { get; set; }
        public string DescricaoCodigoAnp { get; set; }
        public decimal PercentualGlp { get; set; }
        public decimal PercentualGasNatural { get; set; }
        public decimal PercentualGasNaturalImportado { get; set; }
        public decimal ValorPartida { get; set; }


    }
}