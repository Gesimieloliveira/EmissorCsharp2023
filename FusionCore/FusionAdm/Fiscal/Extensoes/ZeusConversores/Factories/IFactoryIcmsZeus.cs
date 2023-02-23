using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories
{
    public interface IFactoryIcmsZeus
    {
        string Cst { get; }
        ICMSBasico Cria(ImpostoIcms imposto);
    }
}