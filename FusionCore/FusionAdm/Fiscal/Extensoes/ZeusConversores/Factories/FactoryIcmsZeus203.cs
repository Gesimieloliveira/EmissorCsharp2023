using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus203 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "203";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMSSN202
            {
                orig = imposto.OrigemMercadoria.ToZeus(),
                CSOSN = Csosnicms.Csosn203,
                modBCST = imposto.ModalidadeBcSt.ToZeus(),
                pMVAST = imposto.MvaSt,
                pRedBCST = imposto.ReducaoBcSt,
                pICMSST = imposto.AliquotaSt,
                vBCST = imposto.ValorBcSt,
                vICMSST = imposto.ValorIcmsSt
            };

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