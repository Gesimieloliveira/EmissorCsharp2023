using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus50 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "50";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            return new ICMS40
            {
                CST = Csticms.Cst50,
                orig = imposto.OrigemMercadoria.ToZeus()
            };
        }
    }
}