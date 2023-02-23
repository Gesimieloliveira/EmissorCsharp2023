using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus500 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "500";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMSSN500
            {
                orig = imposto.OrigemMercadoria.ToZeus(),
                CSOSN = Csosnicms.Csosn500
            };

            if (imposto.EhConsumidorFinal() == false)
            {
                icms.vBCSTRet = 0.00M;
                icms.pST = 0.00M;
                icms.vICMSSubstituto = 0.00M;
                icms.vICMSSTRet = 0.00M;
            }

            return icms;
        }
    }
}