using System;
using Fusion.FastReport.Relatorios;
using FusionCore.Relatorios;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Relatorios.Comum
{
    public sealed class OpcaoRelatorioProprio : IOpcaoRelatorio
    {
        private readonly RelatorioProprio _relatorio;
        private readonly ISessaoManager _sessaoManager;

        public OpcaoRelatorioProprio(RelatorioProprio relatorio, ISessaoManager sessaoManager)
        {
            _relatorio = relatorio;
            _sessaoManager = sessaoManager;
        }

        public Guid TemplateId => _relatorio.Template.Id;
        public string Descricao => _relatorio.Descricao;
        public string Grupo => _relatorio.Grupo;

        private RelatorioCustomizado CriaRelatorio()
        {
            return new RelatorioCustomizado(_sessaoManager, _relatorio);
        }

        public void Visualizar()
        {
            using (var r = CriaRelatorio())
            {
                r.Visualizar();
            }
        }

        public void EditarDesenho()
        {
            using (var r = CriaRelatorio())
            {
                r.EditarDesenho();
            }
        }

        public void ExportarTemplate()
        {
            using (var exportador = new ExportadorTemplate(CriaRelatorio()))
            {
                exportador.ComDescricao(_relatorio.Descricao);
                exportador.Exportar();
            }
        }

        public void ImportarTemplate()
        {
            var importador = new ImportadorTemplate(_sessaoManager);

            importador.ImportarPorArquivo(TemplateId);
        }

        public void ExcluirRelatorio()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioRelatorio(sessao);
                repositorio.ExcluirRelatorio(_relatorio);

                transacao.Commit();
            }
        }

        public void EditarInforamacoesRelatorio()
        {
            var contexto = new CadastroFastReportContexto(_sessaoManager);
            var dialog = new CadastroFastReport(contexto);

            contexto.Edicao(_relatorio);

            dialog.ShowDialog();
        }

        public void DevEditarDesenho()
        {
            DialogBox.MostraAviso("Relatório próprio");
        }
    }
}