using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus103 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "103";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            return new ICMSSN102
            {
                orig = imposto.OrigemMercadoria.ToZeus(),
                CSOSN = Csosnicms.Csosn103
            };
        }
    }
}