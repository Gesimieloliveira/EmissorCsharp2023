using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1631563004)]
    public class FA1631563004_InseridoColunaStatusCadastroPOS : Migration
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