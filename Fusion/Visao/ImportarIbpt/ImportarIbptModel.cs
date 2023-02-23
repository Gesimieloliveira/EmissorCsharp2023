using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using FusionCore.FusionAdm.Importacao;
using FusionCore.FusionAdm.Importacao.Estrategia;
using FusionLibrary.VisaoModel;
using NHibernate.Hql.Ast.ANTLR;

namespace Fusion.Visao.ImportarIbpt
{
    public class ImportarIbptModel : ViewModel
    {
        private string _caminhoArquivo;
        private bool _importacaoEmAndamento;
        public ICommand CommandBuscarArquivo => GetSimpleCommand(BuscaArquivoHandler);

        public bool ImportacaoEmAndamento
        {
            get { return _importacaoEmAndamento; }
            set
            {
                if (value.Equals(_importacaoEmAndamento)) return;
                _importacaoEmAndamento = value;
                PropriedadeAlterada();
            }
        }

        public string CaminhoArquivo
        {
            get { return _caminhoArquivo; }
            set
            {
                if (value == _caminhoArquivo) return;
                _caminhoArquivo = value;
                PropriedadeAlterada();
            }
        }

        private void BuscaArquivoHandler(object obj)
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Arquivos CSV(*.csv)|*.csv"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                CaminhoArquivo = dialog.FileName;
        }

        public void FazerImportacao()
        {
            ImportacaoEmAndamento = true;

            try
            {
                var fileInfo = new FileInfo(CaminhoArquivo ?? string.Empty);

                if (!fileInfo.Exists)
                    throw new InvalidPathException(string.Concat(CaminhoArquivo, " não foi localizado"));

                var importador = new Importador();
                importador.Importar(fileInfo.ToString(), new ImportacaoTabelaIbpt());
            }
            catch (Exception)
            {
                ImportacaoEmAndamento = false;
                throw;
            }
        }
    }
}