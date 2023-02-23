using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public class FactoryIcmsZeus101 : IFactoryIcmsZeus
    {
        public string Cst { get; } = "101";

        public ICMSBasico Cria(ImpostoIcms imposto)
        {
            return new ICMSSN101
            {
                orig = imposto.OrigemMercadoria.ToZeus(),
                CSOSN = Csosnicms.Csosn101,
                vCredICMSSN = imposto.ValorCredito,
                pCredSN = imposto.AliquotaCredito
            };
        }
    }
}