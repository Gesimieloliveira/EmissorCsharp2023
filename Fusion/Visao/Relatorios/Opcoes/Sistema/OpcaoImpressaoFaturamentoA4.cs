using System;
using Fusion.FastReport.Relatorios.Sistema.FaturamentoMei;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoImpressaoFaturamentoA4 : OpcaoRelatorioSistema<RImpressaoFaturamentoA4>
    {
        public override string Descricao { get; } = "Impressão do faturamento A4";

        protected override RImpressaoFaturamentoA4 CriaRelatorio()
        {
            return new RImpressaoFaturamentoA4(SessaoManager);
        }

        public override void EditarDesenho()
        {
            SolicitaArgumentos((id, duasVias) =>
            {
                using (var r = CriaRelatorio())
                {
                    r.ComId(id);
                    r.DuplicarImpressao(duasVias?.ToUpper() == "S");
                    r.EditarDesenho();
                }
            });
        }

        private void SolicitaArgumentos(Action<int, string> acao)
        {
            if (!InputBox.ShowInput("Id do Faturamento", out int id)) return;
            if (!InputBox.ShowInput("Duas vias (S/N)?", out string duasVias)) return;

            acao.Invoke(id, duasVias);
        }

        public override void ExportarTemplate()
        {
            AcaoExportarTemplate();
        }

        public override void ImportarTemplate()
        {
            AcaoImportarTemplate();
        }

        public override void ExcluirRelatorio()
        {
            AcaoExcluirTemplateSalvo();
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            SolicitaArgumentos((id, duasVias) =>
            {
                using (var r = CriaRelatorio())
                {
                    r.ComId(id);
                    r.DuplicarImpressao(duasVias?.ToUpper() == "S");
                    r.DevEditarDesenho(arquivoFrx);
                }
            });
        }
    }
}