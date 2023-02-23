using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1561298970)]
    public class FA1561298970_AlteracaoEstruturaCaixa : Migration
    {
        public override void Up()
        {
            Alter.Table("caixa_individual")
                .AddColumn("localEvento").AsByte().NotNullable().SetExistingRowsTo(0)
                .AddColumn("terminalOffline_id").AsByte().Nullable();

            Create.ForeignKey("fk_caixainvidual_to_terminaloffline")
                .FromTable("caixa_individual").ForeignColumn("terminalOffline_id")
                .ToTable("terminal_offline").PrimaryColumn("id");

            Alter.Table("caixa_registro").AddColumn("localEvento").AsByte().NotNullable().SetExistingRowsTo(0);
            Alter.Table("caixa_lancamento").AddColumn("localEvento").AsByte().NotNullable().SetExistingRowsTo(0);
        }

        public override void Down()
        {
        }
    }
}