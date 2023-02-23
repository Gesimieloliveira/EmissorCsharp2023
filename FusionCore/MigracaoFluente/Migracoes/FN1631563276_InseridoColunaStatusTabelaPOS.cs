using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1631563276)]
    public class FN1631563276_InseridoColunaStatusTabelaPOS : Migration
    {
        public override void Up()
        {
            Alter.Table("pos").AddColumn("status").AsBoolean().NotNullable().SetExistingRowsTo(true);
        }

        public override void Down()
        {
            
        }
    }
}