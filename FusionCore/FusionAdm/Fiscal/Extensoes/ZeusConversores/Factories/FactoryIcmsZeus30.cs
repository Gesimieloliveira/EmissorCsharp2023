using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus30 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "30";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMS30
            {
                CST = Csticms.Cst30,
                orig = imposto.OrigemMercadoria.ToZeus(),
                modBCST = imposto.ModalidadeBcSt.ToZeus(),
                pICMSST = imposto.AliquotaSt,
                vBCST = imposto.ValorBcSt,
                pMVAST = imposto.MvaSt,
                vICMSST = imposto.ValorIcmsSt,
                pRedBCST = imposto.ReducaoBcSt
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