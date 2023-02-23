using System;
using System.IO;
using Fusion.Factories;
using FusionCore.Relatorios;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Relatorios.Comum
{
    public class ImportadorTemplate
    {
        private readonly ISessaoManager _sessaoManager;

        public ImportadorTemplate(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void ImportarPorArquivo(Guid templateId)
        {
            var dialog = FileDialogFactory.CriaDialogFRX();

            if (dialog.ShowDialog() == false)
            {
                return;
            }

            var bytes = File.ReadAllBytes(dialog.FileName);

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioRelatorio(sessao);
                repositorio.SalvarTemplate(new Template(templateId, bytes));

                transacao.Commit();
            }

            DialogBox.MostraInformacao("Template importado com sucesso");
        }
    }
}