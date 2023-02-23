using System;
using System.IO;
using System.Windows.Forms;
using Fusion.FastReport.Relatorios;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Relatorios.Comum
{
    public class ExportadorTemplate : IDisposable
    {
        private readonly RelatorioBase _relatorio;
        private string _descricao;

        public ExportadorTemplate(RelatorioBase relatorio)
        {
            _relatorio = relatorio;
        }

        public void ComDescricao(string descricao)
        {
            _descricao = descricao;
        }

        public void Exportar()
        {
            var dialog = new SaveFileDialog
            {
                Filter = @"Arquivo FRX|*.frx",
                FileName = $"{_descricao}"
            };

            var showDialog = dialog.ShowDialog();

            if (showDialog != DialogResult.OK)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(dialog.FileName))
            {
                DialogBox.MostraInformacao("Não foi selecionado um caminho");
                return;
            }

            using (var fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var template = _relatorio.ExportarTemplate();

                fs.Write(template, 0, template.Length);
                fs.Flush(true);
            }

            DialogBox.MostraInformacao($"Template exportado com sucesso");
        }

        public void Dispose()
        {
            _relatorio.Dispose();
        }
    }
}