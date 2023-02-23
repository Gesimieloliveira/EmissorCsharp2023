using System.IO;
using System.Reflection;
using FluentMigrator;
using FusionCore.FusionAdm.Importacao;
using FusionCore.FusionAdm.Importacao.Estrategia;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1649781872)]
    public class FA1649781872_MigracaoIbptNovo : Migration
    {
        public override void Up()
        {
            var stream = ObtemStream("IBPTGO.csv");

            var csvTemporario = Path.Combine(DiretorioAssembly.GetPastaTemp(), "arquivo.csv");

            using (var fs = File.OpenWrite(csvTemporario))
            {
                stream.CopyTo(fs);
            }

            var importador = new Importador();
            importador.Importar(csvTemporario, new ImportacaoTabelaIbpt());
        }

        public override void Down()
        {
        }

        private Stream ObtemStream(string resourceCsv)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = $"FusionCore.Assets.Ibpts.{resourceCsv}";
            var stream = assembly.GetManifestResourceStream(resource);

            return stream;
        }
    }
}