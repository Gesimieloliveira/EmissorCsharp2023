using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus60 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "60";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            var icms = new ICMS60
            {
                CST = Csticms.Cst60,
                orig = imposto.OrigemMercadoria.ToZeus()
            };

            if (imposto.EhConsumidorFinal() == false)
            {
                icms.vBCSTRet = 0.00M;
                icms.pST = 0.00M;
                icms.vICMSSubstituto = 0.00M;
                icms.vICMSSTRet = 0.00M;
            }

            var produto = imposto.ObterItemNfe().Produto;

            if (!string.IsNullOrWhiteSpace(produto.CodigoAnp))
            {
                var anp = produto.CarregaAnp();

                if (anp != null && anp.GrupoRepasseInterestadualSt)
                {
                    return new ICMSST
                    {
                        CST = Csticms.Cst60,
                        orig = imposto.OrigemMercadoria.ToZeus(),
                        vBCSTRet = 0.00M,
                        pST = 0.0000M,
                        vICMSSubstituto = 0.00M,
                        vICMSSTRet = 0.00M,
                        vBCSTDest = 0.00M,
                        vICMSSTDest = 0.00M,
                        pRedBCEfet = 0.000M,
                        vBCEfet = 0.00M,
                        pICMSEfet = 0.0000M,
                        vICMSEfet = 0.00M
                    };
                }
            }

            return icms;
        }
    }
}