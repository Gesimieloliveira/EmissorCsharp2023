using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1566477449)]
    public class FA1566477449_TabelaCancelamentoDFe : Migration
    {
        public override void Up()
        {
            Create.Table("mde_cancelamento")
                .WithColumn("mdeResumo_id").AsInt32().PrimaryKey("pk_mde_cancelamento")
                .WithColumn("xml").AsCustom("xml").NotNullable();

            Create.ForeignKey("fk_mde_cancelamento_to_mde_resumo")
                .FromTable("mde_cancelamento").InSchema("dbo").ForeignColumn("mdeResumo_id")
                .ToTable("mde_resumo").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}