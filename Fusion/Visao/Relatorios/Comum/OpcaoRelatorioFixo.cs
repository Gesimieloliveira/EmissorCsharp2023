using System;
using Fusion.FastReport.Relatorios;
using FusionCore.Relatorios;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Relatorios.Comum
{
    public abstract class OpcaoRelatorioFixo<T> : IOpcaoRelatorio where T : RelatorioBase
    {
        protected ISessaoManager SessaoManager = new SessaoManagerAdm();

        public virtual Guid TemplateId => GuidsDosRelatorios.Obtem(typeof(T));
        public abstract string Descricao { get; }
        public abstract string Grupo { get; }

        protected abstract T CriaRelatorio();

        public virtual void EditarInforamacoesRelatorio()
        {
            DialogBox.MostraAviso("Este relatório não permite edição das informações");
        }

        public virtual void Visualizar()
        {
            using (var relatorio = CriaRelatorio())
            {
                relatorio.Visualizar();
            }
        }

        public virtual void EditarDesenho()
        {
            using (var relatorio = CriaRelatorio())
            {
                relatorio.EditarDesenho();
            }
        }

        public void ImportarTemplate()
        {
            var importador = new ImportadorTemplate(SessaoManager);

            importador.ImportarPorArquivo(TemplateId);
        }

        public virtual void ExcluirRelatorio()
        {
            if (TemplateId == Guid.Empty)
            {
                return;
            }

            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repo = new RepositorioRelatorio(sessao);
                repo.ExcluirTemplatePeloId(TemplateId);

                transacao.Commit();
            }

            DialogBox.MostraInformacao("Template editado do relatório foi excluido com sucesso");
        }

        public virtual void ExportarTemplate()
        {
            using (var exportador = new ExportadorTemplate(CriaRelatorio()))
            {
                exportador.Exportar();
            }
        }

        public virtual void DevEditarDesenho()
        {
            AcaoEscolherArquivoFrx.Escolher(OnDevEditarDesenho);
        }

        protected virtual void OnDevEditarDesenho(string arquivoFrx)
        {
            using (var r = CriaRelatorio())
            {
                r.DevEditarDesenho(arquivoFrx);
            }
        }
    }
}