using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1567464034)]
    public class FA1567464034_AdicionarEmissorFiscalNfeResumida : Migration
    {
        public override void Up()
        {
            Alter.Table("mde_resumo")
                .AddColumn("emissorFiscal_id")
                .AsByte().NotNullable();

            Create.ForeignKey("fk_mde_resumo_to_emissor_fiscal")
                .FromTable("mde_resumo").InSchema("dbo").ForeignColumn("emissorFiscal_id")
                .ToTable("emissor_fiscal").InSchema("dbo").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}