using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Providers;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class ConversorIcmsParaZeus
    {
        public static ICMS ToZeus(this ImpostoIcms imposto)
        {
            var factory = FactoryIcmsZeusProvider.Instance.Get(imposto.Cst.Id);

            return new ICMS
            {
                TipoICMS = factory.Cria(imposto)
            };
        }
    }
}