using System;
using Fusion.FastReport.Relatorios;
using FusionCore.Relatorios;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Relatorios.Comum
{
    public abstract class OpcaoRelatorioSistema<T> : IOpcaoRelatorio where T : RelatorioBase
    {
        protected ISessaoManager SessaoManager = new SessaoManagerAdm();
        public Guid TemplateId => GuidsDosRelatorios.Obtem(typeof(T));
        public string Grupo { get; } = "Sistema";

        public abstract string Descricao { get; }

        protected abstract T CriaRelatorio();

        public virtual void Visualizar()
        {
            DialogBox.MostraAviso("Este relatório não permite visualização");
        }

        public virtual void EditarDesenho()
        {
            DialogBox.MostraAviso("Esse relatório não permite edição de desenho");
        }

        public virtual void ExportarTemplate()
        {
            DialogBox.MostraAviso("Esse relatório não permite exportação de template");
        }

        public virtual void ImportarTemplate()
        {
            DialogBox.MostraAviso("Esse relatório não permite importação de template");
        }

        public virtual void ExcluirRelatorio()
        {
            DialogBox.MostraAviso("Esse relatório não permite exclusão de template");
        }

        public virtual void EditarInforamacoesRelatorio()
        {
            DialogBox.MostraAviso("Esse relatório não permite edição de informações");
        }

        public void DevEditarDesenho()
        {
            AcaoEscolherArquivoFrx.Escolher(OnDevEditarDesenho);
        }

        protected void AcaoExportarTemplate()
        {
            using (var exportador = new ExportadorTemplate(CriaRelatorio()))
            {
                exportador.Exportar();
            }
        }

        protected void AcaoImportarTemplate()
        {
            var importador = new ImportadorTemplate(SessaoManager);
            importador.ImportarPorArquivo(TemplateId);
        }

        protected void AcaoExcluirTemplateSalvo()
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioRelatorio(sessao);
                repositorio.ExcluirTemplatePeloId(TemplateId);

                transacao.Commit();
            }

            DialogBox.MostraInformacao("Template editado foi excluido com sucesso.");
        }

        protected abstract void OnDevEditarDesenho(string nomeArquivo);
    }
}