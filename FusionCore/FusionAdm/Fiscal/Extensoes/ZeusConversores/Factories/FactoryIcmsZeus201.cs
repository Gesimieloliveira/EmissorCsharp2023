using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus201 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "201";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMSSN201
            {
                orig = imposto.OrigemMercadoria.ToZeus(),
                CSOSN = Csosnicms.Csosn201,
                modBCST = imposto.ModalidadeBcSt.ToZeus(),
                pMVAST = imposto.MvaSt,
                pRedBCST = imposto.ReducaoBcSt,
                pICMSST = imposto.AliquotaSt,
                vBCST = imposto.ValorBcSt,
                vICMSST = imposto.ValorIcmsSt,
                pCredSN = imposto.AliquotaCredito,
                vCredICMSSN = imposto.ValorCredito
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