using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus41 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "41";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            return new ICMS40
            {
                CST = Csticms.Cst41,
                orig = imposto.OrigemMercadoria.ToZeus()
            };
        }
    }
}