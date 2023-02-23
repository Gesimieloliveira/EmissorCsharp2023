using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FA")]
    [Migration(1551117800)]
    public class FA1551117800_AdicionaColunaRegimeTributario : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("regimeTributario").AsByte().WithDefaultValue(1).NotNullable();
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("regimeTributario");
        }

        public override void Down()
        {
            
        }
    }
}