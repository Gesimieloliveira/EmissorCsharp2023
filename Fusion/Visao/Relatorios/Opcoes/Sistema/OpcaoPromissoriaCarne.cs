using Fusion.FastReport.Relatorios.Sistema.Financeiro;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoPromissoriaCarne : OpcaoRelatorioSistema<RPromissoriaCarne>
    {
        public override string Descricao { get; } = "Impressão de carnê com promissória";

        protected override RPromissoriaCarne CriaRelatorio()
        {
            return new RPromissoriaCarne(SessaoManager);
        }

        public override void EditarDesenho()
        {
            if (!InputBox.ShowInput("ID do Malote", out int id))
            {
                return;
            }

            using (var r = CriaRelatorio())
            {
                r.ComMaloteId(id);
                r.EditarDesenho();
            }
        }

        protected override void OnDevEditarDesenho(string nomeArquivo)
        {
            if (!InputBox.ShowInput("ID do Malote", out int id))
            {
                return;
            }

            using (var r = CriaRelatorio())
            {
                r.ComMaloteId(id);
                r.DevEditarDesenho(nomeArquivo);
            }
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
    }
}