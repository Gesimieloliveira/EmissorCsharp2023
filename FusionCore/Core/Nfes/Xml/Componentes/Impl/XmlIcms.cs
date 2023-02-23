using FusionCore.Extencoes;
using FusionCore.Tributacoes.Flags;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlIcms : IXmlIcms
    {
        public XmlIcms(ICMS icms)
        {
            if (icms.TipoICMS is ICMS00 icms00)
            {
                OrigemMercadoria = (short) icms00.orig;
                Cst = icms00.CST.GetCodigo();
                Aliquota = icms00.pICMS;
                ValorBc = icms00.vBC;
                Valor = icms00.vICMS;
            }

            if (icms.TipoICMS is ICMS10 icms10)
            {
                OrigemMercadoria = (short) icms10.orig;
                Cst = icms10.CST.GetCodigo();
                Aliquota = icms10.pICMS;
                ValorBc = icms10.vBC;
                Valor = icms10.vICMS;
                ReducaoSt = icms10.pRedBCST ?? 0.00M;
                Mva = icms10.pMVAST ?? 0.00M;
                AliquotaSt = icms10.pICMSST;
                ValorBcSt = icms10.vBCST;
                ValorSt = icms10.vICMSST;
                ValorFcpSt = icms10.vFCPST ?? 0.0m;
                ValorBcFcpSt = icms10.vBCFCPST ?? 0.0m;
                AliquotaFcpSt = icms10.pFCPST ?? 0.0m;
            }

            if (icms.TipoICMS is ICMS20 icms20)
            {
                OrigemMercadoria = (short) icms20.orig;
                Cst = icms20.CST.GetCodigo();
                Reducao = icms20.pRedBC;
                Aliquota = icms20.pICMS;
                ValorBc = icms20.vBC;
                Valor = icms20.vICMS;
            }

            if (icms.TipoICMS is ICMS30 icms30)
            {
                OrigemMercadoria = (short) icms30.orig;
                Cst = icms30.CST.GetCodigo();
                ReducaoSt = icms30.pRedBCST ?? 0.00M;
                Mva = icms30.pMVAST ?? 0.00M;
                AliquotaSt = icms30.pICMSST;
                ValorBcSt = icms30.vBCST;
                ValorSt = icms30.vICMSST;
                ValorFcpSt = icms30.vFCPST ?? 0.0m;
                ValorBcFcpSt = icms30.vBCFCPST ?? 0.0m;
                AliquotaFcpSt = icms30.pFCPST ?? 0.0m;
            }

            if (icms.TipoICMS is ICMS40 icms40)
            {
                OrigemMercadoria = (short) icms40.orig;
                Cst = icms40.CST.GetCodigo();
            }

            if (icms.TipoICMS is ICMS51 icms51)
            {
                OrigemMercadoria = (short) icms51.orig;
                Cst = icms51.CST.GetCodigo();
                Reducao = icms51.pRedBC ?? 0.00M;
                Aliquota = icms51.pICMS ?? 0.00M;
                ValorBc = icms51.vBC ?? 0.00M;
                Valor = icms51.vICMS ?? 0.00M;
            }

            if (icms.TipoICMS is ICMS60 icms60)
            {
                OrigemMercadoria = (short) icms60.orig;
                Cst = icms60.CST.GetCodigo();
            }

            if (icms.TipoICMS is ICMS70 icms70)
            {
                OrigemMercadoria = (short) icms70.orig;
                Cst = icms70.CST.GetCodigo();
                Reducao = icms70.pRedBC;
                Aliquota = icms70.pICMS;
                ValorBc = icms70.vBC;
                Valor = icms70.vICMS;
                ReducaoSt = icms70.pRedBCST ?? 0.00M;
                Mva = icms70.pMVAST ?? 0.00M;
                AliquotaSt = icms70.pICMSST;
                ValorBcSt = icms70.vBCST;
                ValorSt = icms70.vICMSST;
                ValorFcpSt = icms70.vFCPST ?? 0.0m;
                ValorBcFcpSt = icms70.vBCFCPST ?? 0.0m;
                AliquotaFcpSt = icms70.pFCPST ?? 0.0m;
            }

            if (icms.TipoICMS is ICMS90 icms90)
            {
                OrigemMercadoria = (short) icms90.orig;
                Cst = icms90.CST.GetCodigo();
                Reducao = icms90.pRedBC ?? 0.00M;
                Aliquota = icms90.pICMS ?? 0.00M;
                ValorBc = icms90.vBC ?? 0.00M;
                Valor = icms90.vICMS ?? 0.00M;
                ReducaoSt = icms90.pRedBCST ?? 0.00M;
                Mva = icms90.pMVAST ?? 0.00M;
                AliquotaSt = icms90.pICMSST ?? 0.00M;
                ValorBcSt = icms90.vBCST ?? 0.00M;
                ValorSt = icms90.vICMSST ?? 0.00M;
                ValorFcpSt = icms90.vFCPST ?? 0.0m;
                ValorBcFcpSt = icms90.vBCFCPST ?? 0.0m;
                AliquotaFcpSt = icms90.pFCPST ?? 0.0m;
            }

            if (icms.TipoICMS is ICMSPart icmsPart)
            {
                OrigemMercadoria = (short)icmsPart.orig;
                Cst = icmsPart.CST.GetCodigo();
                Reducao = icmsPart.pRedBC ?? 0.00m;
                Aliquota = icmsPart.pICMS;
                ValorBc = icmsPart.vBC;
                Valor = icmsPart.vICMS;
                Reducao = icmsPart.pRedBCST ?? 0.00m;
                Mva = icmsPart.pMVAST ?? 0.00m;
                AliquotaSt = icmsPart.pICMSST;
                ValorBcSt = icmsPart.vBCST;
                ValorSt = icmsPart.vICMSST;
            }

            if (icms.TipoICMS is ICMSST icmsst)
            {
                OrigemMercadoria = (short) icmsst.orig;
                Cst = icmsst.CST.GetCodigo();
            }
        }

        public RegimeTributario Regime { get; } = RegimeTributario.RegimeNormal;
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