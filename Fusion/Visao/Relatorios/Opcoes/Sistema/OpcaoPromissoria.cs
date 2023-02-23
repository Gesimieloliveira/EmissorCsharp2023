using Fusion.FastReport.Relatorios.Sistema.Financeiro;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoPromissoria : OpcaoRelatorioSistema<RPromissoria>
    {
        public override string Descricao { get; } = "Impressão de promissória";

        protected override RPromissoria CriaRelatorio()
        {
            return new RPromissoria(SessaoManager);
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