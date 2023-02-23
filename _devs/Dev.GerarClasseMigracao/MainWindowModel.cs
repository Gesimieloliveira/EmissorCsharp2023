using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Input;
using FusionCore.Helpers.Hidratacao;
using FusionCore.MigracaoFluente;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;

namespace Dev.GerarClasseMigracao
{
    public class MainWindowModel : ModelBase
    {
        private string _nomeClasse;

        public string NomeClasse
        {
            get => _nomeClasse;
            set
            {
                _nomeClasse = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandGerarMigracaoFusionAdm => GetSimpleCommand(GerarMigracaoFusionAdmAction);
        public ICommand CommandGerarMigracaoFusionNfce => GetSimpleCommand(GerarMigracaoFusionNfceAction);
        public ICommand CommandGerarMigracaoFusionRelatorio => GetSimpleCommand(CommandGerarMigracaoFusionRelatorioAction);

        private void CommandGerarMigracaoFusionRelatorioAction(object obj)
        {
            GerarClasse("FusionCore.MigracaoFluente.Migracoes", MigracaoTag.Relatorio);
        }

        private void GerarMigracaoFusionNfceAction(object obj)
        {
            GerarClasse("FusionCore.MigracaoFluente.Migracoes", MigracaoTag.Nfce);
        }

        private void GerarMigracaoFusionAdmAction(object obj)
        {
            GerarClasse("FusionCore.MigracaoFluente.Migracoes", MigracaoTag.Adm);
        }

        private void GerarClasse(string stringNamespace, MigracaoTag fluentTag)
        {
            if (NomeClasse.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Digite um nome para a classe");
                return;
            }

            var tag = FusionCore.Helpers.Basico.EnumHelper.GetDescription(fluentTag);
            var timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var className = $"{tag}{timestamp}_{NomeClasse}";

            var migracaoTemplate = LocalAssets.ObterMigracaoTemplate();

            var fileName = Path.Combine(Path.GetTempPath(), $"{className}.cs");

            using (var fs = new FileStream(fileName, FileMode.CreateNew))
            using (var sw = new StreamWriter(fs))
            {
                var classContent = migracaoTemplate
                    .Replace("#{Namespace}", stringNamespace)
                    .Replace("#{Tag}", "\"" + tag + "\"")
                    .Replace("#{Versao}", timestamp.ToString())
                    .Replace("#{NomeClasse}", className);

                sw.Write(classContent);
                fs.Flush();
            }

            Clipboard.Clear();
            Clipboard.SetFileDropList(new StringCollection {fileName});

            DialogBox.MostraInformacao($"Classe {className} copiada para o clipboard");
        }

        private SaveFileDialog CriaDialogCs()
        {
            var dialog = new SaveFileDialog
            {
                Filter = @"Arquivo Classe (.cs)|*.cs",
                FileName = NomeClasse.TrimOrEmpty()
            };
            return dialog;
        }
    }
}