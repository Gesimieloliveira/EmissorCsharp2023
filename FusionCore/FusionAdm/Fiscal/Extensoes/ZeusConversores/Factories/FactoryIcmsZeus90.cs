using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus90 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "90";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMS90
            {
                CST = Csticms.Cst90,
                modBC = imposto.ModalidadeBcIcms.ToZeus(),
                orig = imposto.OrigemMercadoria.ToZeus(),
                pICMS = imposto.AliquotaIcms,
                vBC = imposto.ValorBcIcms,
                vICMS = imposto.ValorIcms,
                modBCST = imposto.ModalidadeBcSt.ToZeus(),
                pICMSST = imposto.AliquotaSt,
                vBCST = imposto.ValorBcSt,
                pMVAST = imposto.MvaSt,
                vICMSST = imposto.ValorIcmsSt,
                pRedBCST = imposto.ReducaoBcSt,
                pRedBC = imposto.ReducaoBcIcms
            };

            if (imposto.AliquotaFcp > 0)
            {
                icms.pFCP = imposto.AliquotaFcp;
                icms.vBCFCP = imposto.ValorBcFcp;
                icms.vFCP = imposto.ValorFcp;
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