using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1598384809)]
    public class FA1598384809_ExportacaoBuscaRapida : Migration
    {
        public override void Up()
        {
            Create.Table("exportacao_busca_rapida")
                .InSchema("preferencias")
                .WithColumn("id").AsInt32().PrimaryKey("pk_exportacao_busca_rapida").Identity()
                .WithColumn("identificadorMaquina").AsAnsiString(40).NotNullable()
                .WithColumn("localExportacao").AsAnsiString(255).NotNullable()
                .WithColumn("tag").AsAnsiString(25).NotNullable();
        }

        public override void Down()
        {
        }
    }
}