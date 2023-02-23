using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus20 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "20";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMS20
            {
                CST = Csticms.Cst20,
                modBC = imposto.ModalidadeBcIcms.ToZeus(),
                orig = imposto.OrigemMercadoria.ToZeus(),
                pICMS = imposto.AliquotaIcms,
                vBC = imposto.ValorBcIcms,
                vICMS = imposto.ValorIcms,
                pRedBC = imposto.ReducaoBcIcms
            };

            if (imposto.AliquotaFcp > 0)
            {
                icms.pFCP = imposto.AliquotaFcp;
                icms.vBCFCP = imposto.ValorBcFcp;
                icms.vFCP = imposto.ValorFcp;
            }

            return icms;
        }
    }
}