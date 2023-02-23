using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551117149)]
    public class FN1551117149_AdicionaColunaRegimeTributario : Migration
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