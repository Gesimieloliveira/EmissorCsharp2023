using System;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using FusionCore.Helpers.Basico;
using FusionCore.MigracaoFluente.Tabela;
using FusionCore.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace FusionCore.MigracaoFluente
{
    internal class MigracaoBancoDados : IMigracao
    {
        private readonly IConexaoCfg _conexao;
        private readonly MigracaoTag _tag;
        private IServiceProvider _serviceProvider;
        public long UltimaVersaoInterna => ObterUltimaVersaoInterna();

        private long ObterUltimaVersaoInterna()
        {
            if (_serviceProvider == null)
                CriaServico();

            var runner = _serviceProvider.GetService<IMigrationRunner>() as MigrationRunner;

            if (runner == null)
            {
                throw new Exception("Falha ao verificar atualização");
            }

            var ultimaMigracao = runner.MigrationLoader.LoadMigrations().LastOrDefault();

            return ultimaMigracao.Key;
        }

        public MigracaoBancoDados(IConexaoCfg conexao, MigracaoTag tag)
        {
            _conexao = conexao;
            _tag = tag;
        }

        public bool PrecisaAtualizar => GetPrecisaAtualizar();

        private bool GetPrecisaAtualizar()
        {
            CriaServico();

            var runner = _serviceProvider.GetService<IMigrationRunner>() as MigrationRunner;

            if (runner == null)
            {
                throw new Exception("Falha ao verificar atualização");
            }

            var precisa = runner.MigrationLoader.LoadMigrations()
                .Any(pair => !runner.VersionLoader
                    .VersionInfo.HasAppliedMigration(pair.Key));

            return precisa;
        }

        public void Migracao()
        {
            FazerBackupSeguranca();
            CriaServico();

            using (var scopo = _serviceProvider.CreateScope())
            {
                AtualizarDatabase(scopo.ServiceProvider);
            }
        }

        private void FazerBackupSeguranca()
        {
            var dbUtility = new DatabaseUtility(_conexao);

            if (dbUtility.EhLocalHost)
            {
                dbUtility.BackupDatabase(_conexao.BancoDados, "c:\\sistemafusion\\backup");
                return;
            }

            dbUtility.BackupRemoto(_conexao.BancoDados);
        }

        private void CriaServico()
        {
            var tagstring = _tag.GetDescription();

            _serviceProvider = new ServiceCollection()

                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb

                    .AddSqlServer2008()
                    .WithVersionTable(new TabelaVersao())

                    .WithGlobalConnectionString(_conexao.CriarStringDeConexao(90))

                    .ScanIn(Assembly.GetExecutingAssembly())
                        .For.Migrations()
                        .For.EmbeddedResources()
                )
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .Configure<RunnerOptions>(opt => { opt.Tags = new[] { tagstring }; })
                .BuildServiceProvider(false);
        }

        private void AtualizarDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}