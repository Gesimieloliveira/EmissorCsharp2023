using FusionCore.Extencoes;
using FusionCore.Tributacoes.Flags;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlCsoson : IXmlIcms
    {
        public XmlCsoson(ICMS icms)
        {
            if (icms.TipoICMS is ICMSSN101 c101)
            {
                OrigemMercadoria = (short) c101.orig;
                Cst = c101.CSOSN.GetCodigo();
            }

            if (icms.TipoICMS is ICMSSN102 c102)
            {
                OrigemMercadoria = (short) c102.orig;
                Cst = c102.CSOSN.GetCodigo();
            }

            if (icms.TipoICMS is ICMSSN201 c201)
            {
                OrigemMercadoria = (short) c201.orig;
                Cst = c201.CSOSN.GetCodigo();
                ReducaoSt = c201.pRedBCST ?? 0.00M;
                Mva = c201.pMVAST ?? 0.00M;
                AliquotaSt = c201.pICMSST;
                ValorBcSt = c201.vBCST;
                ValorSt = c201.vICMSST;
                ValorFcpSt = c201.vFCPST ?? 0.0m;
                ValorBcFcpSt = c201.vBCFCPST ?? 0.0m;
                AliquotaFcpSt = c201.pFCPST ?? 0.0m;
            }

            if (icms.TipoICMS is ICMSSN202 c202)
            {
                OrigemMercadoria = (short) c202.orig;
                Cst = c202.CSOSN.GetCodigo();
                ReducaoSt = c202.pRedBCST ?? 0.00M;
                Mva = c202.pMVAST ?? 0.00M;
                AliquotaSt = c202.pICMSST;
                ValorBcSt = c202.vBCST;
                ValorSt = c202.vICMSST;
                ValorFcpSt = c202.vFCPST ?? 0.0m;
                ValorBcFcpSt = c202.vBCFCPST ?? 0.0m;
                AliquotaFcpSt = c202.pFCPST ?? 0.0m;
            }

            if (icms.TipoICMS is ICMSSN500 c500)
            {
                OrigemMercadoria = (short) c500.orig;
                Cst = c500.CSOSN.GetCodigo();
            }

            if (icms.TipoICMS is ICMSSN900 c900)
            {
                OrigemMercadoria = (short) c900.orig;
                Cst = c900.CSOSN.GetCodigo();
                ReducaoSt = c900.pRedBCST ?? 0.00M;
                Mva = c900.pMVAST ?? 0.00M;
                AliquotaSt = c900.pICMSST ?? 0.00M;
                ValorBcSt = c900.vBCST ?? 0.00M;
                ValorSt = c900.vICMSST ?? 0.00M;
                Reducao = c900.pRedBC ?? 0.00M;
                Aliquota = c900.pICMS ?? 0.00M;
                Valor = c900.vICMS ?? 0.00M;
                ValorBc = c900.vBC ?? 0.00M;
                ValorFcpSt = c900.vFCPST ?? 0.0m;
                ValorBcFcpSt = c900.vBCFCPST ?? 0.0m;
                AliquotaFcpSt = c900.pFCPST ?? 0.0m;
            }
        }

        public RegimeTributario Regime { get; } = RegimeTributario.SimplesNacional;
        public short OrigemMercadoria { get; }
        public string Cst { get; }
        public decimal Reducao { get; }
        public decimal Aliquota { get; }
        public decimal ValorBc { get; }
        public decimal Valor { get; }
        public decimal ReducaoSt { get; }
        public decimal Mva { get; }
        public decimal AliquotaSt { get; }
        public decimal ValorBcSt { get; }
        public decimal ValorSt { get; }
        public decimal ValorFcpSt { get; }
        public decimal ValorBcFcpSt { get; }
        public decimal AliquotaFcpSt { get; }
    }
}