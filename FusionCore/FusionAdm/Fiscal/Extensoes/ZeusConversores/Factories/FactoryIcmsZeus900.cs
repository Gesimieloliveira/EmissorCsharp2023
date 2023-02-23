using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus900 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "900";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMSSN900
            {
                orig = imposto.OrigemMercadoria.ToZeus(),
                CSOSN = Csosnicms.Csosn900
            };

            if (imposto.AliquotaIcms > 0)
            {
                icms.modBC = imposto.ModalidadeBcIcms.ToZeus();
                icms.pICMS = imposto.AliquotaIcms;
                icms.pRedBC = imposto.ReducaoBcIcms;
                icms.vBC = imposto.ValorBcIcms;
                icms.vICMS = imposto.ValorIcms;
            }

            if (imposto.AliquotaSt > 0)
            {
                icms.modBCST = imposto.ModalidadeBcSt.ToZeus();
                icms.pICMSST = imposto.AliquotaSt;
                icms.pRedBCST = imposto.ReducaoBcSt;
                icms.vBCST = imposto.ValorBcSt;
                icms.vICMSST = imposto.ValorIcmsSt;
                icms.pMVAST = imposto.MvaSt;
            }

            if (imposto.AliquotaFcpSt > 0)
            {
                icms.pFCPST = imposto.AliquotaFcpSt;
                icms.vBCFCPST = imposto.ValorBcFcpSt;
                icms.vFCPST = imposto.ValorFcpSt;
            }

            return icms;
        }
    }
}