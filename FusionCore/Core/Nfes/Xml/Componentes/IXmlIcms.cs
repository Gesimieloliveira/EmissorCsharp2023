using FusionCore.Tributacoes.Flags;

namespace FusionCore.Core.Nfes.Xml.Componentes
{
    public interface IXmlIcms
    {
        RegimeTributario Regime { get; }
        short OrigemMercadoria { get; }
        string Cst { get; }
        decimal Reducao { get; }
        decimal Aliquota { get; }
        decimal ValorBc { get; }
        decimal Valor { get; }
        decimal ReducaoSt { get; }
        decimal Mva { get; }
        decimal AliquotaSt { get; }
        decimal ValorBcSt { get; }
        decimal ValorSt { get; }
        decimal ValorFcpSt { get; }
        decimal ValorBcFcpSt { get; }
        decimal AliquotaFcpSt { get; }
    }
}