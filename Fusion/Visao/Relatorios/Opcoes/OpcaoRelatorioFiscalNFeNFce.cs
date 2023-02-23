using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoRelatorioFiscalNFeNFce : OpcaoRelatorioFixo<RRelatorioFiscalEmissaoNFeNFce>
    {
        public override string Descricao { get; } = "Relatório fiscal de emissões na NF-e e NFC-e";

        public override string Grupo { get; } = "Relatório Fiscal";

        protected override RRelatorioFiscalEmissaoNFeNFce CriaRelatorio()
        {
            return new RRelatorioFiscalEmissaoNFeNFce(SessaoManager, Descricao);
        }
    }
}