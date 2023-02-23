using System;
using Fusion.FastReport.Relatorios.Sistema.FaturamentoMei;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoImpressaoFaturamento80 : OpcaoRelatorioSistema<RImpressaoFaturamento80>
    {
        public override string Descricao { get; } = "Impressão do faturamento 80 mm";

        protected override RImpressaoFaturamento80 CriaRelatorio()
        {
            return new RImpressaoFaturamento80(SessaoManager);
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