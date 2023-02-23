using FluentMigrator;

namespace FusionCore.MigracaoFluente.Migracoes
{
    [Tags("FN")]
    [Migration(1551375162)]
    public class FN1551375162_AdicionaTotaisCofinsPis : Migration
    {
        public override void Up()
        {
            Alter.Table("nfce").AddColumn("totalBaseCalculoCofins").AsDecimal(15, 2).WithDefaultValue(0.0m);
            Alter.Table("nfce").AddColumn("totalCofins").AsDecimal(15, 2).WithDefaultValue(0.0m);
            Alter.Table("nfce").AddColumn("totalBaseCalculoPis").AsDecimal(15, 2).WithDefaultValue(0.0m);
            Alter.Table("nfce").AddColumn("totalPis").AsDecimal(15, 2).WithDefaultValue(0.0m);

            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalBaseCalculoCofins");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalCofins");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalBaseCalculoPis");
            Delete.DefaultConstraint().OnTable("nfce").InSchema("dbo").OnColumn("totalPis");
        }

        public override void Down()
        {
        }
    }
}