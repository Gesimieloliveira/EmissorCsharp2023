using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentMigrator;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1649782207)]
    public class FA1649782207_InserirOuAtualizarNcms : Migration
    {
        public override void Up()
        {
            var stream = ObtemStream();

            var ncmTemporario = Path.Combine(DiretorioAssembly.GetPastaTemp(), "ncm_temp.csv");

            using (var fs = File.OpenWrite(ncmTemporario))
            {
                stream.CopyTo(fs);
            }

            using (var reader = new StreamReader(ncmTemporario, Encoding.UTF8))
            {
                var cfg = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = "," };
                var csv = new CsvReader(reader, cfg);

                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    AtualizaNcmDto(csv);
                }
            }
        }

        private void AtualizaNcmDto(CsvReader csv)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNcm = new RepositorioNcm(sessao);

                var ncmDoCsv = csv.GetField(0).Replace(".", string.Empty);

                var ncm = repositorioNcm.GetPeloId(ncmDoCsv);

                if (ncm == null)
                    ncm = new NcmDTO();

                ncm.Id = ncmDoCsv;
                ncm.Descricao = csv.GetField(1).TrimStart('-').Trim();
                ncm.InicioVigencia = csv.GetField(2).IsNullOrEmpty() ? (DateTime?)null : DateTime.Parse(csv.GetField(2), new CultureInfo("pt-BR"));
                ncm.FimVigencia = csv.GetField(3).IsNullOrEmpty() ? (DateTime?)null : DateTime.Parse(csv.GetField(3), new CultureInfo("pt-BR"));
                
                repositorioNcm.Salva(ncm);
                transacao.Commit();
            }
        }

        public override void Down()
        {
        }

        private Stream ObtemStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = "FusionCore.Assets.Ncms.Ncms.csv";
            var stream = assembly.GetManifestResourceStream(resource);

            return stream;
        }
    }
}