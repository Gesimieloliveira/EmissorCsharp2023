using System;
using System.Collections.Generic;
using System.IO;
using FastReport.Data;
using FastReport.Export.Pdf;
using FusionCore.Sessao;
using FR = FastReport;

namespace Fusion.FastReport.Relatorios
{
    public abstract class RelatorioBase : IDisposable
    {
        protected readonly ISessaoManager SessaoManager;
        protected FornecedorTemplate FornecedorTemplate;
        private readonly FR.Report _report;
        private bool _jaConfigurado;

        protected RelatorioBase(ISessaoManager sessaoManager)
        {
            _report = new FR.Report();
            SessaoManager = sessaoManager;
            FornecedorTemplate = new FornecedorTemplate(sessaoManager);
        }

        public void Dispose()
        {
            _report?.Dispose();
        }

        public void RegistraParametro(string key, object value)
        {
            _report.SetParameterValue(key, value);
        }

        public void RegistraDados<T>(string alias, IEnumerable<T> data)
        {
            _report.RegisterData(data, alias, 20);
            _report.GetDataSource(alias).Enabled = true;
        }

        protected T EncontrarObjeto<T>(string nome) where T : FR.Base
        {
            return (T) _report.FindObject(nome);
        }

        protected void AtivarImpressaoModoSplit()
        {
            _report.PrintSettings.PrintMode = FR.PrintMode.Split;
        }

        protected void NumeroCopias(int copias)
        {
            _report.PrintSettings.Copies = copias;
        }

        public virtual void Imprimir(string printer, int? quantidadeCopia = null)
        {
            PrepararRelatorio();

            _report.PrintSettings.ShowDialog = false;
            _report.PrintSettings.Printer = printer;

            if (quantidadeCopia != null)
            {
                _report.PrintSettings.Copies = quantidadeCopia.Value;
            }

            _report.Print();
        }

        public byte[] ExportarTemplate()
        {
            return FornecerTemplate();
        }

        public void Visualizar()
        {
            PrepararRelatorio();
            _report.Show(true);
        }

        private void PrepararRelatorio()
        {
            _report.Load(new MemoryStream(FornecerTemplate()));
            ConfiguraStringConexao();
            RegistraParametro("JaConfigurado", _jaConfigurado);
            PrepararDados();
        }

        public void ExportarPdf(Stream stream)
        {
            PrepararRelatorio();

            _report.Prepare();
            _report.Export(new PDFExport(), stream);
            stream.Position = 0;
        }

        private void ConfiguraStringConexao()
        {
            if (SessaoManager == null)
            {
                return;
            }

            const string nome = "ConexaoFusion";

            foreach (DataConnectionBase con in _report.Dictionary.Connections)
            {
                if (con.Name != nome)
                {
                    continue;
                }

                con.ConnectionString = SessaoManager.ConnectionString;
                return;
            }

            var mssql = new MsSqlDataConnection
            {
                ConnectionString = SessaoManager.ConnectionString,
                Name = nome
            };

            mssql.CreateAllTables();

            _report.Dictionary.Connections.Add(mssql);
        }

        public void EditarDesenho()
        {
            var template = FornecerTemplate();
            var tmpFile = Path.GetTempFileName();
            var tmpFrxFile = $"{tmpFile}.frx";

            using (var fs = new FileStream(tmpFile, FileMode.Open, FileAccess.Write))
            {
                fs.Write(template, 0, template.Length);
                fs.Flush();
            }

            File.Move(tmpFile, tmpFrxFile);

            _report.Load(tmpFrxFile);

            ConfiguraStringConexao();
            PrepararDados();

            _report.Design(true);
        }

        public void DevEditarDesenho(string arquivoFrx)
        {
            _report.Load(arquivoFrx);

            ConfiguraStringConexao();
            PrepararDados();

            _report.Design(true);
        }

        protected void MarcarComoJaConfigurado()
        {
            _jaConfigurado = true;
        }

        protected void RegistrarDescricao(string descricao)
        {
            RegistraParametro("DescricaoRelatorio", descricao);
        }

        protected abstract byte[] FornecerTemplate();
        protected abstract void PrepararDados();
    }
}