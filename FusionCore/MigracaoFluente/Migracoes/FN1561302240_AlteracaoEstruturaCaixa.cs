using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1561302240)]
    public class FN1561302240_AlteracaoEstruturaCaixa : Migration
    {
        public override void Up()
        {
            Alter.Table("caixa_individual")
                .AddColumn("localEvento").AsByte().NotNullable().SetExistingRowsTo(1)
                .AddColumn("terminalOffline_id").AsByte().Nullable();

            Execute.Sql("update caixa_individual set terminalOffline_id = (select top 1 c.terminalOffline_id from configuracao_terminal c);");

            Alter.Table("caixa_individual").AlterColumn("terminalOffline_id").AsByte().NotNullable();

            Alter.Table("caixa_registro").AddColumn("localEvento").AsByte().NotNullable().SetExistingRowsTo(1);
            Alter.Table("caixa_lancamento").AddColumn("localEvento").AsByte().NotNullable().SetExistingRowsTo(1);
        }

        public override void Down()
        {
        }
    }
}