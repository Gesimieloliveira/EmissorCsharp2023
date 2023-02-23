using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567022179)]
    public class FA1567022179_TabelaEventoManifestacao : Migration
    {
        public override void Up()
        {
            Create.Table("mde_evento_manifestacao")
                .WithColumn("id").AsInt32().Identity().PrimaryKey("pk_mde_evento_manifestacao")
                .WithColumn("xml").AsCustom("xml").NotNullable()
                .WithColumn("mdeResumo_id").AsInt32().NotNullable();

            Create.ForeignKey("fk_mde_evento_manifestacao_to_mde_resumo").FromTable("mde_evento_manifestacao")
                .InSchema("dbo").ForeignColumn("mdeResumo_id")
                .ToTable("mde_resumo").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}