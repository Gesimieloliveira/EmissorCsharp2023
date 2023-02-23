using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoInutilizacoesNfeNfce : OpcaoRelatorioFixo<RInutilizacoesNfeNFce>
    {
        public override string Descricao { get; } = "Relatório de inutilizações feitas na NF-e e NFC-e";

        public override string Grupo { get; } = "Relatório Fiscal";

        protected override RInutilizacoesNfeNFce CriaRelatorio()
        {
            return new RInutilizacoesNfeNFce(SessaoManager, Descricao);
        }

        public override void Visualizar()
        {
            AcaoFiltro.SolicitarPeriodo(periodo =>
            {
                using (var r = CriaRelatorio())
                {
                    r.ComPeriodo(periodo);
                    r.Visualizar();
                }
            });
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            AcaoFiltro.SolicitarPeriodo(periodo =>
            {
                using (var r = CriaRelatorio())
                {
                    r.ComPeriodo(periodo);
                    r.DevEditarDesenho(arquivoFrx);
                }
            });
        }
    }
}